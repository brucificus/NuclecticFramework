#region CPL License

/*
Nuclex Framework
Copyright (C) 2002-2011 Nuclex Development Labs

This library is free software; you can redistribute it and/or
modify it under the terms of the IBM Common Public License as
published by the IBM Corporation; either version 1.0 of the
License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
IBM Common Public License for more details.

You should have received a copy of the IBM Common Public
License along with this library
*/

#endregion

using Microsoft.Xna.Framework.Input;
using Moq;
using Nuclectic.Tests.Mocks;
#if UNITTEST
using NUnit.Framework;

namespace Nuclectic.Tests.Input.Devices
{

	/// <summary>Unit tests for the mocked keyboard</summary>
	[TestFixture]
	public class MockedKeyboardTest
	{

		#region interface IKeyboardSubscriber

		/// <summary>Subscriber to the </summary>
		public interface IKeyboardSubscriber
		{

			/// <summary>Called when a key has been pressed</summary>
			/// <param name="key">Key that has been pressed</param>
			void KeyPressed(Keys key);

			/// <summary>Called when a key has been released</summary>
			/// <param name="key">Key that has been released</param>
			void KeyReleased(Keys key);

			/// <summary>Called when a character has been entered</summary>
			/// <param name="character">Character that has been entered</param>
			void CharacterEntered(char character);

		}

		#endregion // interface IKeyboardSubscriber

		/// <summary>Verifies that the GetState() method is working</summary>
		[Test]
		public void TestGetState()
		{
			var keyboard = new MockedKeyboard();

			keyboard.GetState();
			// No exception means success
		}

		/// <summary>Verifies that the game pad can be attached and detached</summary>
		[Test]
		public void TestAttachAndDetach()
		{
			var keyboard = new MockedKeyboard();

			Assert.IsFalse(keyboard.IsAttached);
			keyboard.Attach();
			Assert.IsTrue(keyboard.IsAttached);
			keyboard.Detach();
			Assert.IsFalse(keyboard.IsAttached);
		}

		/// <summary>Verifies that the mocked keyboard's name can be retrieved</summary>
		[Test]
		public void TestName()
		{
			var keyboard = new MockedKeyboard();

			StringAssert.Contains("mock", keyboard.Name.ToLower());
		}

		/// <summary>Verifies that key presses can be simulation</summary>
		[Test]
		public void TestPressKey()
		{
			var keyboard = new MockedKeyboard();
			Mock<IKeyboardSubscriber> subscriber = mockSubscriber(keyboard);

			keyboard.Press(Keys.H);

			subscriber.Setup(x => x.KeyPressed(Keys.H)).Verifiable();

			keyboard.Update();

			subscriber.VerifyAll();
		}

		/// <summary>Verifies that key releases can be simulated</summary>
		[Test]
		public void TestReleaseKey()
		{
			var keyboard = new MockedKeyboard();
			Mock<IKeyboardSubscriber> subscriber = mockSubscriber(keyboard);

			keyboard.Release(Keys.W);

			subscriber.Setup(x => x.KeyReleased(Keys.W)).Verifiable();

			keyboard.Update();

			subscriber.VerifyAll();
		}

		/// <summary>Verifies that character entries can be simulated</summary>
		[Test]
		public void TestEnterCharacter()
		{
			var keyboard = new MockedKeyboard();
			Mock<IKeyboardSubscriber> subscriber = mockSubscriber(keyboard);

			keyboard.Enter('!');

			subscriber.Setup(x => x.CharacterEntered('!')).Verifiable();

			keyboard.Update();

			subscriber.VerifyAll();
		}

		/// <summary>Verifies that text entries can be simulated</summary>
		[Test]
		public void TestTypeText()
		{
			var keyboard = new MockedKeyboard();
			Mock<IKeyboardSubscriber> subscriber = mockSubscriber(keyboard);

			keyboard.Type("Xyz™");

			ExpectMappedCapital(subscriber, Keys.X, 'X');

			ExpectMapped(subscriber, Keys.Y, 'y');

			ExpectMapped(subscriber, Keys.Z, 'z');

			ExpectUnmapped(subscriber);

			keyboard.Update();

			subscriber.VerifyAll();
		}

		private static void ExpectUnmapped(Mock<IKeyboardSubscriber> subscriber)
		{
			subscriber.Setup(x => x.KeyPressed(Keys.None)).Verifiable();
			subscriber.Setup(x => x.CharacterEntered('™')).Verifiable();
			subscriber.Setup(x => x.KeyReleased(Keys.None)).Verifiable();
		}

		private static void ExpectMapped(Mock<IKeyboardSubscriber> subscriber, Keys key, char character)
		{
			subscriber.Setup(x => x.KeyPressed(key)).Verifiable();
			subscriber.Setup(x => x.CharacterEntered(character)).Verifiable();
			subscriber.Setup(x => x.KeyReleased(key)).Verifiable();
		}

		private static void ExpectMappedCapital(Mock<IKeyboardSubscriber> subscriber, Keys key, char character)
		{
			subscriber.Setup(x => x.KeyPressed(Keys.LeftShift)).Verifiable();
			subscriber.Setup(x => x.KeyPressed(key)).Verifiable();
			subscriber.Setup(x => x.CharacterEntered(character)).Verifiable();
			subscriber.Setup(x => x.KeyReleased(key)).Verifiable();
			subscriber.Setup(x => x.KeyReleased(Keys.LeftShift)).Verifiable();
		}

		/// <summary>Mocks a subscriber for the buffered keyboard</summary>
		/// <returns>A subscriber registered to the events of the keyboard</returns>
		private Mock<IKeyboardSubscriber> mockSubscriber(MockedKeyboard keyboard)
		{
			Mock<IKeyboardSubscriber> subscriber = new Mock<IKeyboardSubscriber>();

			keyboard.KeyPressed += subscriber.Object.KeyPressed;
			keyboard.KeyReleased += subscriber.Object.KeyReleased;
			keyboard.CharacterEntered += subscriber.Object.CharacterEntered;

			return subscriber;
		}
	}

} // namespace Nuclex.Input.Devices

#endif // UNITTEST
