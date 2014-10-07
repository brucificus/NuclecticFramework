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

#if UNITTEST

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;

using NUnit.Framework;
using NMock;

using WinForm = System.Windows.Forms.Form;
using Application = System.Windows.Forms.Application;

namespace Nuclex.Input {

  /// <summary>Unit tests for the window message interceptor</summary>
  [TestFixture]
  internal class WindowMessageFilterTest {

    #region interface IKeyboardMessageSubscriber

    /// <summary>Subscriber to keyboard messages arriving at the window</summary>
    public interface IKeyboardMessageSubscriber {

      /// <summary>Called when a key is pressed on the keyboard</summary>
      /// <param name="key">Key that has been pressed</param>
      void KeyPressed(Keys key);

      /// <summary>Called when a key on the keyboard has been released</summary>
      /// <param name="key">Key that has been released</param>
      void KeyReleased(Keys key);

      /// <summary>Called when the user has entered a character</summary>
      /// <param name="character">Character the user has entered</param>
      void CharacterEntered(char character);

    }

    #endregion // interface IKeyboardMessageSubscriber

    #region interface IMouseMessageSubscriber

    /// <summary>Subscriber to mouse messages arriving at the window</summary>
    public interface IMouseMessageSubscriber {

      /// <summary>Called when a mouse button has been pressed</summary>
      /// <param name="buttons">Buttons that have been pressed</param>
      void MouseButtonPressed(MouseButtons buttons);

      /// <summary>Called when a button on the mouse has been released</summary>
      /// <param name="buttons">Button that has been released</param>
      void MouseButtonReleased(MouseButtons buttons);

      /// <summary>Called when the mouse cursor has been moved</summary>
      /// <param name="x">New X coordinate of the mouse cursor</param>
      /// <param name="y">New Y coordinate of the mouse cursor</param>
      void MouseMoved(float x, float y);

      /// <summary>Called when the mouse wheel has been rotated</summary>
      /// <param name="ticks">Number of ticks the mouse wheel was rotated</param>
      void MouseWheelRotated(float ticks);

    }

    #endregion // interface IMouseMessageSubscriber

    /// <summary>Called once before each test is run</summary>
    [SetUp]
    public void Setup() {
      this.mockery = new MockFactory();
      this.form = new WinForm();
      this.filter = new WindowMessageFilter(this.form.Handle);
    }

    /// <summary>Called once after each test has run</summary>
    [TearDown]
    public void Teardown() {
      if (this.filter != null) {
        this.filter.Dispose();
        this.filter = null;
      }
      if (this.form != null) {
        this.form.Dispose();
        this.form = null;
      }
      if (this.mockery != null) {
        this.mockery.Dispose();
        this.mockery = null;
      }
    }

    /// <summary>Verifies that the constructor of the interceptor is working</summary>
    [Test]
    public void TestConstructor() {
      Assert.IsNotNull(this.filter);
    }
    /*
        /// <summary>Checks whether the dialog code is adjusted by the interceptor</summary>
        [Test]
        public void TestGetDialogCode() {
          int dlgCode = sendMessage(UnsafeNativeMethods.WindowMessages.WM_GETDLGCODE, 0, 0);

          int expected = UnsafeNativeMethods.DLGC_WANTALLKEYS;
          Assert.AreEqual(expected, dlgCode & expected);

          expected = UnsafeNativeMethods.DLGC_WANTCHARS;
          Assert.AreEqual(expected, dlgCode & expected);
        }
    */
    /// <summary>Verifies that the WM_KEYDOWN message is intercepted</summary>
    [Test]
    public void TestKeyPressMessage() {
      Mock<IKeyboardMessageSubscriber> subscriber = mockKeyboardSubscriber();
      
      subscriber.Expects.One.Method(x => x.KeyPressed(0)).With(Keys.X);
      sendThreadMessage(UnsafeNativeMethods.WindowMessages.WM_KEYDOWN, (int)Keys.X, 0);

      this.mockery.VerifyAllExpectationsHaveBeenMet();
    }

    /// <summary>Verifies that the WM_KEYDOWN message is intercepted</summary>
    [Test]
    public void TestKeyReleaseMessage() {
      Mock<IKeyboardMessageSubscriber> subscriber = mockKeyboardSubscriber();

      subscriber.Expects.One.Method(x => x.KeyReleased(0)).With(Keys.V);
      sendThreadMessage(UnsafeNativeMethods.WindowMessages.WM_KEYUP, (int)Keys.V, 0);

      this.mockery.VerifyAllExpectationsHaveBeenMet();
    }

    /// <summary>Verifies that the WM_CHAR message is intercepted</summary>
    [Test]
    public void TestCharacterEnteredMessage() {
      Mock<IKeyboardMessageSubscriber> subscriber = mockKeyboardSubscriber();

      subscriber.Expects.One.Method(x => x.CharacterEntered(' ')).With('C');
      sendThreadMessage(UnsafeNativeMethods.WindowMessages.WM_CHAR, (int)'C', 0);

      this.mockery.VerifyAllExpectationsHaveBeenMet();
    }

    /// <summary>Verifies that the WM_MOUSEMOVE message is intercepted</summary>
    [Test]
    public void TestMouseMoveMessage() {
      Mock<IMouseMessageSubscriber> subscriber = mockMouseSubscriber();

      subscriber.Expects.One.Method(x => x.MouseMoved(0, 0)).With(123.0f, 456.0f);

      // When the window is destroyed, a WM_MOUSELEAVE can be generated, which
      // leads to another mouse movement notification with coordinates -1, -1.
      // For some crappy reason that happens in the DoEvents() call already.
      subscriber.Expects.AtMostOnce.Method(x => x.MouseMoved(0, 0)).With(-1.0f, -1.0f);

      sendThreadMessage(UnsafeNativeMethods.WindowMessages.WM_MOUSEMOVE, 0, 123 | (456 << 16));

      this.mockery.VerifyAllExpectationsHaveBeenMet();
    }

    /// <summary>Verifies that the WM_*BUTTONDOWN messages are intercepted</summary>
    [
      Test,
      TestCase(UnsafeNativeMethods.WindowMessages.WM_LBUTTONDOWN, 0, MouseButtons.Left),
      TestCase(UnsafeNativeMethods.WindowMessages.WM_MBUTTONDOWN, 0, MouseButtons.Middle),
      TestCase(UnsafeNativeMethods.WindowMessages.WM_RBUTTONDOWN, 0, MouseButtons.Right),
      TestCase(UnsafeNativeMethods.WindowMessages.WM_XBUTTONDOWN, 1, MouseButtons.X1),
      TestCase(UnsafeNativeMethods.WindowMessages.WM_XBUTTONDOWN, 2, MouseButtons.X2)
    ]
    public void TestMouseButtonPressedMessage(
      UnsafeNativeMethods.WindowMessages message, int wParam, MouseButtons button
    ) {
      Mock<IMouseMessageSubscriber> subscriber = mockMouseSubscriber();

      subscriber.Expects.One.Method(x => x.MouseButtonPressed(0)).With(button);
      sendThreadMessage(message, wParam << 16, 0);

      this.mockery.VerifyAllExpectationsHaveBeenMet();
    }

    /// <summary>Verifies that the WM_*BUTTONUP messages are intercepted</summary>
    [
      Test,
      TestCase(UnsafeNativeMethods.WindowMessages.WM_LBUTTONUP, 0, MouseButtons.Left),
      TestCase(UnsafeNativeMethods.WindowMessages.WM_MBUTTONUP, 0, MouseButtons.Middle),
      TestCase(UnsafeNativeMethods.WindowMessages.WM_RBUTTONUP, 0, MouseButtons.Right),
      TestCase(UnsafeNativeMethods.WindowMessages.WM_XBUTTONUP, 1, MouseButtons.X1),
      TestCase(UnsafeNativeMethods.WindowMessages.WM_XBUTTONUP, 2, MouseButtons.X2)
    ]
    public void TestMouseButtonReleasedMessage(
      UnsafeNativeMethods.WindowMessages message, int wParam, MouseButtons button
    ) {
      Mock<IMouseMessageSubscriber> subscriber = mockMouseSubscriber();

      subscriber.Expects.One.Method(x => x.MouseButtonReleased(0)).With(button);
      sendThreadMessage(message, wParam << 16, 0);

      this.mockery.VerifyAllExpectationsHaveBeenMet();
    }

    /// <summary>Verifies that WM_MOUSEWHEEL messages are intercepted</summary>
    [Test]
    public void TestMouseWheelMessage() {
      Mock<IMouseMessageSubscriber> subscriber = mockMouseSubscriber();

      subscriber.Expects.One.Method(x => x.MouseWheelRotated(0)).With(1.0f);
      sendThreadMessage(UnsafeNativeMethods.WindowMessages.WM_MOUSEHWHEEL, 120 << 16, 0);

      this.mockery.VerifyAllExpectationsHaveBeenMet();
    }

    /// <summary>Verifies that WM_MOUSELEAVE messages are intercepted</summary>
    [Test]
    public void TestMouseLeaveMessage() {
      Mock<IMouseMessageSubscriber> subscriber = mockMouseSubscriber();

      subscriber.Expects.One.Method(x => x.MouseMoved(0, 0)).With(-1.0f, -1.0f);
      sendThreadMessage(UnsafeNativeMethods.WindowMessages.WM_MOUSELEAVE, 0, 0);

      this.mockery.VerifyAllExpectationsHaveBeenMet();
    }

    /// <summary>Sends a message to the game's window</summary>
    /// <param name="message">Message that will be sent</param>
    /// <param name="wParam">Value for the message's wParam</param>
    /// <param name="lParam">Value for the message's lParam</param>
    private void sendThreadMessage(
      UnsafeNativeMethods.WindowMessages message, int wParam, int lParam
    ) {

      // Post the requested message to the thread's message queue
      // (and not the window's message queue, thus the window procedure will
      // not be invoked at this point)
#pragma warning disable 0618 // Unmanaged thread ID can change for managed threads
      bool posted = UnsafeNativeMethods.PostThreadMessage(
        AppDomain.GetCurrentThreadId(),
        (uint)message,
        new IntPtr(wParam),
        new IntPtr(lParam)
      );
#pragma warning restore 0618

      if (!posted) {
        throw new Exception("Failed posting a message to the current thread");
      }

      // Now have our thread run the message pump once to process the new message
      Application.DoEvents();

    }

    /// <summary>
    ///   Mocks a mouse message subscriber and registers it to the interceptor's events
    /// </summary>
    /// <returns>The new mouse message subscriber</returns>
    private Mock<IMouseMessageSubscriber> mockMouseSubscriber() {
      Mock<IMouseMessageSubscriber> subscriber = this.mockery.CreateMock<
        IMouseMessageSubscriber
      >();
      this.filter.MouseButtonPressed += subscriber.MockObject.MouseButtonPressed;
      this.filter.MouseButtonReleased += subscriber.MockObject.MouseButtonReleased;
      this.filter.MouseMoved += subscriber.MockObject.MouseMoved;
      this.filter.MouseWheelRotated += subscriber.MockObject.MouseWheelRotated;
      return subscriber;
    }

    /// <summary>
    ///   Mocks a keyboard message subscriber and registers it to the interceptor's events
    /// </summary>
    /// <returns>The new keyboard message subscriber</returns>
    private Mock<IKeyboardMessageSubscriber> mockKeyboardSubscriber() {
      Mock<IKeyboardMessageSubscriber> subscriber = this.mockery.CreateMock<
        IKeyboardMessageSubscriber
      >();
      this.filter.KeyPressed += subscriber.MockObject.KeyPressed;
      this.filter.KeyReleased += subscriber.MockObject.KeyReleased;
      this.filter.CharacterEntered += subscriber.MockObject.CharacterEntered;
      return subscriber;
    }

    /// <summary>Called when a mouse button has been pressed</summary>
    private static readonly int[] MouseButtonDownMessage = new int[] {
      (int)UnsafeNativeMethods.WindowMessages.WM_LBUTTONDOWN,
      (int)UnsafeNativeMethods.WindowMessages.WM_MBUTTONDOWN,
      (int)UnsafeNativeMethods.WindowMessages.WM_RBUTTONDOWN,
      (int)UnsafeNativeMethods.WindowMessages.WM_XBUTTONDOWN,
      (int)UnsafeNativeMethods.WindowMessages.WM_XBUTTONDOWN
    };

    /// <summary>Called when a mouse button has been released</summary>
    private static readonly int[] MouseButtonUpMessage = new int[] {
      (int)UnsafeNativeMethods.WindowMessages.WM_LBUTTONUP,
      (int)UnsafeNativeMethods.WindowMessages.WM_MBUTTONUP,
      (int)UnsafeNativeMethods.WindowMessages.WM_RBUTTONUP,
      (int)UnsafeNativeMethods.WindowMessages.WM_XBUTTONUP,
      (int)UnsafeNativeMethods.WindowMessages.WM_XBUTTONUP
    };

    /// <summary>Form used to test interception of window messages</summary>
    private WinForm form;
    /// <summary>Message interceptor for the form</summary>
    private WindowMessageFilter filter;
    /// <summary>Creates dynamic mock objects based on interfaces</summary>
    private MockFactory mockery;

  }

} // namespace Nuclex.Input

#endif // UNITTEST
