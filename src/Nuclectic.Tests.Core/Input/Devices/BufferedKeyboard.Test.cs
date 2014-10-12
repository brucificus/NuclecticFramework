﻿#region CPL License

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
using Nuclectic.Input.Devices;
#if UNITTEST
using NUnit.Framework;

namespace Nuclectic.Tests.Input.Devices
{

	/// <summary>Unit tests for the buffered keyboard class</summary>
	[TestFixture]
	public  class BufferedKeyboardTest
	{

		#region class TestBufferedKeyboard

		/// <summary>Test implementation of a buffered keyboard</summary>
		private class TestBufferedKeyboard : BufferedKeyboard
		{

			/// <summary>Whether the input device is connected to the system</summary>
			public override bool IsAttached
			{
				get { return true; }
			}

			/// <summary>Human-readable name of the input device</summary>
			public override string Name
			{
				get { return "Test keyboard"; }
			}

			/// <summary>Records a key press in the event queue</summary>
			/// <param name="key">Key that has been pressed</param>
			public new void BufferKeyPress(Keys key)
			{
				base.BufferKeyPress(key);
			}

			/// <summary>Records a key release in the event queue</summary>
			/// <param name="key">Key that has been released</param>
			public new void BufferKeyRelease(Keys key)
			{
				base.BufferKeyRelease(key);
			}

			/// <summary>Records a character in the event queue</summary>
			/// <param name="character">Character that has been entered</param>
			public new void BufferCharacterEntry(char character)
			{
				base.BufferCharacterEntry(character);
			}

		}

		#endregion // class TestBufferedKeyboard

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

		/// <summary>Verifies that key presses can be buffered</summary>
		[Test]
		public void TestBufferKeyPress()
		{
			var keyboard = new TestBufferedKeyboard();
			Mock<IKeyboardSubscriber> subscriber = mockSubscriber(keyboard);

			keyboard.BufferKeyPress(Keys.H);

			subscriber.Setup(s => s.KeyPressed(Keys.H)).Verifiable();

			keyboard.Update();

			subscriber.VerifyAll();
		}

		/// <summary>Verifies that key releases can be buffered</summary>
		[Test]
		public void TestBufferKeyRelease()
		{
			var keyboard = new TestBufferedKeyboard();
			Mock<IKeyboardSubscriber> subscriber = mockSubscriber(keyboard);

			keyboard.BufferKeyRelease(Keys.W);

			subscriber.Setup(s => s.KeyReleased(Keys.W)).Verifiable();

			keyboard.Update();

			subscriber.VerifyAll();
		}

		/// <summary>Verifies that character entries can be buffered</summary>
		[Test]
		public void TestBufferCharacterEntry()
		{
			var keyboard = new TestBufferedKeyboard();
			Mock<IKeyboardSubscriber> subscriber = mockSubscriber(keyboard);

			keyboard.BufferCharacterEntry('!');

			subscriber.Setup(s => s.CharacterEntered('!')).Verifiable();

			keyboard.Update();

			subscriber.VerifyAll();
		}

		/// <summary>Mocks a subscriber for the buffered keyboard</summary>
		/// <returns>A subscriber registered to the events of the keyboard</returns>
		private Mock<IKeyboardSubscriber> mockSubscriber(TestBufferedKeyboard keyboard)
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
