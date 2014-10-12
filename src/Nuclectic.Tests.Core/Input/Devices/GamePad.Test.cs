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
using Nuclectic.Input.Devices;
using GamePad = Nuclectic.Input.Devices.GamePad;
#if UNITTEST
using NUnit.Framework;

namespace Nuclectic.Tests.Input.Devices
{

	/// <summary>Unit tests for the game pad base class</summary>
	[TestFixture]
	public class GamePadTest
	{

		#region interface IGamePadSubscriber

		/// <summary>Subscriber for the game pad's events</summary>
		public interface IGamePadSubscriber
		{

			/// <summary>Called when a button on the game pad is pressed</summary>
			/// <param name="buttons">Button that has been pressed</param>
			void ButtonPressed(Buttons buttons);

			/// <summary>Called when a button on the game pad has been released</summary>
			/// <param name="buttons">Button that has been released</param>
			void ButtonReleased(Buttons buttons);

			/// <summary>Called when an extended button on the game pad is pressed</summary>
			/// <param name="buttons1">Button or buttons that have been pressed</param>
			/// <param name="buttons2">Button or buttons that have been pressed</param>
			void ExtendedButtonPressed(ulong buttons1, ulong buttons2);

			/// <summary>Called when an extended button on the game pad is released</summary>
			/// <param name="buttons1">Button or buttons that have been released</param>
			/// <param name="buttons2">Button or buttons that have been released</param>
			void ExtendedButtonReleased(ulong buttons1, ulong buttons2);

		}

		#endregion interface IGamePadSubscriber

		#region class TestGamePad

		/// <summary>Implementation of a game pad for unit testing</summary>
		private class TestGamePad : GamePad
		{

			/// <summary>Retrieves the current state of the game pad</summary>
			/// <returns>The current state of the game pad</returns>
			public override GamePadState GetState()
			{
				return new GamePadState();
			}

			/// <summary>Retrieves the current DirectInput joystick state</summary>
			/// <returns>The current state of the DirectInput joystick</returns>
			public override IExtendedGamePadState GetExtendedState()
			{
				return new ExtendedGamePadState();
			}

			/// <summary>Whether the input device is connected to the system</summary>
			public override bool IsAttached
			{
				get { return true; }
			}

			/// <summary>Human-readable name of the input device</summary>
			public override string Name
			{
				get { return "Test dummy"; }
			}

			/// <summary>Update the state of all input devices</summary>
			public override void Update() { }

			/// <summary>Takes a snapshot of the current state of the input device</summary>
			public override void TakeSnapshot() { }

			/// <summary>Whether subscribers to the standard button events exist</summary>
			public new bool HaveEventSubscribers
			{
				get { return base.HaveEventSubscribers; }
			}

			/// <summary>Whether subscribers to the extended button events exist</summary>
			public new bool HaveExtendedEventSubscribers
			{
				get { return base.HaveExtendedEventSubscribers; }
			}

			/// <summary>Triggers the ButtonPressed event</summary>
			/// <param name="buttons">Buttons that will be reported</param>
			public void FireButtonPressed(Buttons buttons)
			{
				OnButtonPressed(buttons);
			}

			/// <summary>Triggers the ButtonReleased event</summary>
			/// <param name="buttons">Buttons that will be reported</param>
			public void FireButtonReleased(Buttons buttons)
			{
				OnButtonReleased(buttons);
			}

			/// <summary>Triggers the ExtendedButtonPressed event</summary>
			/// <param name="buttons1">First set of buttons that will be reported</param>
			/// <param name="buttons2">Second set of buttons that will be reported</param>
			public void FireExtendedButtonPressed(ulong buttons1, ulong buttons2)
			{
				OnExtendedButtonPressed(buttons1, buttons2);
			}

			/// <summary>Triggers the ExtendedButtonReleased event</summary>
			/// <param name="buttons1">First set of buttons that will be reported</param>
			/// <param name="buttons2">Second set of buttons that will be reported</param>
			public void FireExtendedButtonReleased(ulong buttons1, ulong buttons2)
			{
				OnExtendedButtonReleased(buttons1, buttons2);
			}

		}

		#endregion // class TestGamePad

		/// <summary>Verifies that the ButtonPressed event is working</summary>
		[Test]
		public void TestButtonPressedEvent()
		{
			var testGamePad = new TestGamePad();
			Mock<IGamePadSubscriber> subscriber = mockSubscriber(testGamePad);

			subscriber.Setup(s => s.ButtonPressed(Buttons.LeftShoulder)).Verifiable();
			testGamePad.FireButtonPressed(Buttons.LeftShoulder);

			subscriber.VerifyAll();
		}

		/// <summary>Verifies that the ButtonReleased event is working</summary>
		[Test]
		public void TestButtonReleasedEvent()
		{
			var testGamePad = new TestGamePad();
			Mock<IGamePadSubscriber> subscriber = mockSubscriber(testGamePad);

			subscriber.Setup(s => s.ButtonReleased(Buttons.RightStick)).Verifiable();
			testGamePad.FireButtonReleased(Buttons.RightStick);

			subscriber.VerifyAll();
		}

		/// <summary>Verifies that the ExtendedButtonPressed event is working</summary>
		[Test]
		public void TestExtendedButtonPressedEvent()
		{
			var testGamePad = new TestGamePad();
			Mock<IGamePadSubscriber> subscriber = mockSubscriber(testGamePad);

			subscriber.Setup(s => s.ExtendedButtonPressed(0x1234567812345678UL, 0x8765432187654321UL)).Verifiable();
			testGamePad.FireExtendedButtonPressed(
			  0x1234567812345678UL, 0x8765432187654321UL
			);

			subscriber.VerifyAll();
		}

		/// <summary>Verifies that the ExtendedButtonReleased event is working</summary>
		[Test]
		public void TestExtendedButtonReleasedEvent()
		{
			var testGamePad = new TestGamePad();
			Mock<IGamePadSubscriber> subscriber = mockSubscriber(testGamePad);

			subscriber.Setup(s => s.ExtendedButtonReleased(0x8765432187654321UL, 0x1234567812345678UL)).Verifiable();
			testGamePad.FireExtendedButtonReleased(
			  0x8765432187654321UL, 0x1234567812345678UL
			);

			subscriber.VerifyAll();
		}

		/// <summary>Verifies that the HaveSubscribers property</summary>
		[Test]
		public void TestHaveSubscribers()
		{
			var testGamePad = new TestGamePad();
			Assert.IsFalse(testGamePad.HaveEventSubscribers);

			Mock<IGamePadSubscriber> subscriber = new Mock<IGamePadSubscriber>();
			testGamePad.ButtonPressed += subscriber.Object.ButtonPressed;

			Assert.IsTrue(testGamePad.HaveEventSubscribers);

			testGamePad.ButtonPressed -= subscriber.Object.ButtonPressed;
			testGamePad.ButtonReleased += subscriber.Object.ButtonReleased;

			Assert.IsTrue(testGamePad.HaveEventSubscribers);

			testGamePad.ButtonReleased -= subscriber.Object.ButtonReleased;

			Assert.IsFalse(testGamePad.HaveEventSubscribers);
		}

		/// <summary>Verifies that the HaveExtendedSubscribers property</summary>
		[Test]
		public void TestHaveExtendedSubscribers()
		{
			var testGamePad = new TestGamePad();
			Assert.IsFalse(testGamePad.HaveExtendedEventSubscribers);

			Mock<IGamePadSubscriber> subscriber = new Mock<IGamePadSubscriber>();
			testGamePad.ExtendedButtonPressed += subscriber.Object.ExtendedButtonPressed;

			Assert.IsTrue(testGamePad.HaveExtendedEventSubscribers);

			testGamePad.ExtendedButtonPressed -= subscriber.Object.ExtendedButtonPressed;
			testGamePad.ExtendedButtonReleased += subscriber.Object.ExtendedButtonReleased;

			Assert.IsTrue(testGamePad.HaveExtendedEventSubscribers);

			testGamePad.ExtendedButtonReleased -= subscriber.Object.ExtendedButtonReleased;

			Assert.IsFalse(testGamePad.HaveExtendedEventSubscribers);
		}

		/// <summary>Mocks a subscriber for the game pad</summary>
		/// <returns>The mocked subscriber</returns>
		private Mock<IGamePadSubscriber> mockSubscriber(TestGamePad testGamePad)
		{
			Mock<IGamePadSubscriber> subscriber = new Mock<IGamePadSubscriber>();
			testGamePad.ButtonPressed += subscriber.Object.ButtonPressed;
			testGamePad.ButtonReleased += subscriber.Object.ButtonReleased;
			testGamePad.ExtendedButtonPressed += subscriber.Object.ExtendedButtonPressed;
			testGamePad.ExtendedButtonReleased += subscriber.Object.ExtendedButtonReleased;
			return subscriber;
		}
	}
} // namespace Nuclex.Input.Devices

#endif // UNITTEST
