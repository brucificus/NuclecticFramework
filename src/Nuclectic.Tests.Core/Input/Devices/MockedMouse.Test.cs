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
using Nuclex.Input.Devices;
#if UNITTEST
using NUnit.Framework;

namespace Nuclectic.Tests.Input.Devices
{
	/// <summary>Unit tests for the mocked mouse</summary>
	[TestFixture]
	public class MockedMouseTest
	{
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

		/// <summary>Verifies that the GetState() method is working</summary>
		[Test]
		public void TestGetState()
		{
			var mouse = new MockedMouse();

			mouse.GetState();
			// No exception means success
		}

		/// <summary>Verifies that the mouse can be attached and detached</summary>
		[Test]
		public void TestAttachAndDetach()
		{
			var mouse = new MockedMouse();

			Assert.IsFalse(mouse.IsAttached);
			mouse.Attach();
			Assert.IsTrue(mouse.IsAttached);
			mouse.Detach();
			Assert.IsFalse(mouse.IsAttached);
		}

		/// <summary>Verifies that the mocked mouse's name can be retrieved</summary>
		[Test]
		public void TestName()
		{
			var mouse = new MockedMouse();

			StringAssert.Contains("mock", mouse.Name.ToLower());
		}

		/// <summary>Verifies that button presses can be simulated</summary>
		[Test]
		public void TestPressButton()
		{
			var mouse = new MockedMouse();
			Mock<IMouseSubscriber> subscriber = mockSubscriber(mouse);

			mouse.Press(MouseButtons.Right);

			subscriber.Setup(x => x.ButtonPressed(MouseButtons.Right));

			mouse.Update();

			subscriber.VerifyAll();
		}

		/// <summary>Verifies that button releases can be simulated</summary>
		[Test]
		public void TestReleaseButton()
		{
			var mouse = new MockedMouse();
			Mock<IMouseSubscriber> subscriber = mockSubscriber(mouse);

			mouse.Release(MouseButtons.X2);

			subscriber.Setup(x => x.ButtonReleased(MouseButtons.X2)).Verifiable();

			mouse.Update();

			subscriber.VerifyAll();
		}

		/// <summary>Verifies that mouse movement can be simulated</summary>
		[Test]
		public void TestMoveTo()
		{
			var mouse = new MockedMouse();
			Mock<IMouseSubscriber> subscriber = mockSubscriber(mouse);

			mouse.MoveTo(43.21f, 87.65f);

			subscriber.Setup(x => x.Moved(43.21f, 87.65f)).Verifiable();

			mouse.Update();

			subscriber.VerifyAll();
		}

		/// <summary>Verifies that mouse wheel rotations can be simulated</summary>
		[Test]
		public void TestRotateWheel()
		{
			var mouse = new MockedMouse();
			Mock<IMouseSubscriber> subscriber = mockSubscriber(mouse);

			mouse.RotateWheel(1.2f);

			subscriber.Setup(x => x.WheelRotated(1.2f)).Verifiable();

			mouse.Update();

			subscriber.VerifyAll();
		}

		/// <summary>Mocks a subscriber for the buffered keyboard</summary>
		/// <returns>A subscriber registered to the events of the keyboard</returns>
		private Mock<IMouseSubscriber> mockSubscriber(MockedMouse mouse)
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