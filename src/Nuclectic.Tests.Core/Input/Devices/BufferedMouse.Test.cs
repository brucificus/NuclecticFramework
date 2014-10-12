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

using Moq;
using Nuclectic.Input;
using Nuclectic.Input.Devices;
#if UNITTEST
using NUnit.Framework;

namespace Nuclectic.Tests.Input.Devices
{
	/// <summary>Unit tests for the buffered mouse class</summary>
	[TestFixture]
	public class BufferedMouseTest
	{
		#region class TestBufferedMouse

		/// <summary>Test implementation of a buffered mouse</summary>
		private class TestBufferedMouse : BufferedMouse
		{
			/// <summary>Moves the mouse cursor to the specified location</summary>
			/// <param name="x">New X coordinate of the mouse cursor</param>
			/// <param name="y">New Y coordinate of the mouse cursor</param>
			public override void MoveTo(float x, float y) { base.BufferCursorMovement(x, y); }

			/// <summary>Whether the input device is connected to the system</summary>
			public override bool IsAttached { get { return true; } }

			/// <summary>Human-readable name of the input device</summary>
			public override string Name { get { return "Test mouse"; } }

			/// <summary>Records a mouse button press in the event queue</summary>
			/// <param name="buttons">Buttons that have been pressed</param>
			public new void BufferButtonPress(MouseButtons buttons) { base.BufferButtonPress(buttons); }

			/// <summary>Records a mouse button release in the event queue</summary>
			/// <param name="buttons">Buttons that have been released</param>
			public new void BufferButtonRelease(MouseButtons buttons) { base.BufferButtonRelease(buttons); }

			/// <summary>Records a mouse wheel rotation in the event queue</summary>
			/// <param name="ticks">Ticks the mouse wheel has been rotated</param>
			public new void BufferWheelRotation(float ticks) { base.BufferWheelRotation(ticks); }

			/// <summary>Records a mouse cursor movement in the event queue</summary>
			/// <param name="x">X coordinate the mouse cursor has been moved to</param>
			/// <param name="y">Y coordinate the mouse cursor has been moved to</param>
			public new void BufferCursorMovement(float x, float y) { base.BufferCursorMovement(x, y); }
		}

		#endregion // TestBufferedMouse

		#region interface IMouseSubscriber

		/// <summary>Subscriber to the events of a mouse</summary>
		public interface IMouseSubscriber
		{
			/// <summary>Called when a mouse button has been pressed</summary>
			/// <param name="buttons">Button which has been pressed</param>
			void ButtonPressed(MouseButtons buttons);

			/// <summary>Called when a mouse button has been released</summary>
			/// <param name="buttons">Button which has been released</param>
			void ButtonReleased(MouseButtons buttons);

			/// <summary>Called when the mouse cursor has been moved</summary>
			/// <param name="x">X coordinate of the mouse cursor</param>
			/// <param name="y">Y coordinate of the mouse cursor</param>
			void Moved(float x, float y);

			/// <summary>Called when the mouse wheel has been rotated</summary>
			/// <param name="ticks">Number of ticks the mouse wheel was rotated</param>
			void WheelRotated(float ticks);
		}

		#endregion // interface IMouseSubscriber

		/// <summary>Verifies that button presses can be buffered</summary>
		[Test]
		public void TestBufferButtonPress()
		{
			var mouse = new TestBufferedMouse();
			Mock<IMouseSubscriber> subscriber = mockSubscriber(mouse);

			mouse.BufferButtonPress(MouseButtons.Middle);

			subscriber.Setup(s => s.ButtonPressed(MouseButtons.Middle)).Verifiable();

			mouse.Update();

			subscriber.VerifyAll();
		}

		/// <summary>Verifies that button releases can be buffered</summary>
		[Test]
		public void TestBufferButtonRelease()
		{
			var mouse = new TestBufferedMouse();
			Mock<IMouseSubscriber> subscriber = mockSubscriber(mouse);

			mouse.BufferButtonRelease(MouseButtons.X1);

			subscriber.Setup(s => s.ButtonReleased(MouseButtons.X1)).Verifiable();

			mouse.Update();

			subscriber.VerifyAll();
		}

		/// <summary>Verifies that mouse movements can be buffered</summary>
		[Test]
		public void TestBufferMouseMovement()
		{
			var mouse = new TestBufferedMouse();
			Mock<IMouseSubscriber> subscriber = mockSubscriber(mouse);

			mouse.BufferCursorMovement(12.34f, 56.78f);

			subscriber.Setup(s => s.Moved(12.34f, 56.78f)).Verifiable();

			mouse.Update();

			subscriber.VerifyAll();
		}

		/// <summary>Verifies that mouse wheel rotations can be buffered</summary>
		[Test]
		public void TestBufferWheelRotation()
		{
			var mouse = new TestBufferedMouse();
			Mock<IMouseSubscriber> subscriber = mockSubscriber(mouse);

			mouse.BufferWheelRotation(19.28f);

			subscriber.Setup(s => s.WheelRotated(19.28f)).Verifiable();

			mouse.Update();

			subscriber.VerifyAll();
		}

		/// <summary>Mocks a subscriber for the buffered keyboard</summary>
		/// <returns>A subscriber registered to the events of the keyboard</returns>
		private Mock<IMouseSubscriber> mockSubscriber(TestBufferedMouse mouse)
		{
			Mock<IMouseSubscriber> subscriber = new Mock<IMouseSubscriber>();

			mouse.MouseButtonPressed += subscriber.Object.ButtonPressed;
			mouse.MouseButtonReleased += subscriber.Object.ButtonReleased;
			mouse.MouseMoved += subscriber.Object.Moved;
			mouse.MouseWheelRotated += subscriber.Object.WheelRotated;

			return subscriber;
		}
	}
} // namespace Nuclex.Input.Devices

#endif // UNITTEST