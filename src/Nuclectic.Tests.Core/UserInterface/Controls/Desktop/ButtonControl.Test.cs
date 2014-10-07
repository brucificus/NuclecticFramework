#region CPL License
/*
Nuclex Framework
Copyright (C) 2002-2010 Nuclex Development Labs

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

using Nuclex.Support;

using NUnit.Framework;
using NMock;

using Nuclex.Input;
using Nuclex.UserInterface.Input;

namespace Nuclex.UserInterface.Controls.Desktop {

  /// <summary>Unit Test for the button control class</summary>
  [TestFixture]
  internal class ButtonControlTest {

    #region interface IButtonSubscriber

    /// <summary>Interface for a subscriber to the button control's events</summary>
    public interface IButtonSubscriber {

      /// <summary>Called when the button control is pressed</summary>
      /// <param name="sender">Button control that has been pressed</param>
      /// <param name="arguments">Not used</param>
      void Pressed(object sender, EventArgs arguments);

    }

    #endregion // interface IButtonSubscriber

    /// <summary>Called before each test is run</summary>
    [SetUp]
    public void Setup() {
      this.mockery = new MockFactory();
    }

    /// <summary>Called after each test has run</summary>
    [TearDown]
    public void Teardown() {
      if(this.mockery != null) {
        this.mockery.Dispose();
        this.mockery = null;
      }
    }

    /// <summary>Verifies whether the button can be pressed using the mouse</summary>
    [Test]
    public void TestButtonPressByMouse() {
      ButtonControl button = new ButtonControl();
      button.Bounds = new UniRectangle(10, 10, 100, 100);

      Mock<IButtonSubscriber> mockedSubscriber = mockSubscriber(button);
      mockedSubscriber.Expects.One.Method(m => m.Pressed(null, null)).WithAnyArguments();

      // Move the mouse over the button and do a left-click
      button.ProcessMouseMove(0, 0, 50, 50);
      button.ProcessMousePress(MouseButtons.Left);
      button.ProcessMouseRelease(MouseButtons.Left);

      this.mockery.VerifyAllExpectationsHaveBeenMet();
    }

    /// <summary>
    ///   Tests whether a button press can be aborted at the last second by moving
    ///   the mouse away from the button before the mouse button is released
    /// </summary>
    [Test]
    public void TestLastSecondAbortByMouse() {
      ButtonControl button = new ButtonControl();
      button.Bounds = new UniRectangle(10, 10, 100, 100);

      Mock<IButtonSubscriber> mockedSubscriber = mockSubscriber(button);
      mockedSubscriber.Expects.No.Method(m => m.Pressed(null, null)).WithAnyArguments();

      // Move the mouse over the button and do a left-click
      button.ProcessMouseMove(0, 0, 50, 50);
      button.ProcessMousePress(MouseButtons.Left);
      button.ProcessMouseMove(0, 0, 5, 5); // outside of the button
      button.ProcessMouseRelease(MouseButtons.Left);

      this.mockery.VerifyAllExpectationsHaveBeenMet();
    }

    /// <summary>Mocks a subscriber for the events of a button</summary>
    /// <param name="button">Button to mock an event subscriber for</param>
    /// <returns>The mocked event subscriber</returns>
    private Mock<IButtonSubscriber> mockSubscriber(ButtonControl button) {
      Mock<IButtonSubscriber> mockedSubscriber = this.mockery.CreateMock<IButtonSubscriber>();

      button.Pressed += new EventHandler(mockedSubscriber.MockObject.Pressed);

      return mockedSubscriber;
    }

    /// <summary>Manages mocked interfaces and verifies expectations</summary>
    private MockFactory mockery;

  }

} // namespace Nuclex.UserInterface.Controls.Desktop

#endif // UNITTEST
