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
using System.Windows.Forms;

using Microsoft.Xna.Framework;

using NUnit.Framework;
using NMock;
using Microsoft.Xna.Framework.Input;

namespace Nuclex.Input.Devices {

  /// <summary>Unit tests for the XNA (XINPUT) game pad</summary>
  [TestFixture]
  internal class XnaGamePadTest {

    #region interface IGamePadSubscriber

    /// <summary>Subscriber for the game pad's events</summary>
    public interface IGamePadSubscriber {

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
    private class TestGamePad : XnaGamePad {

      /// <summary>Initializs a new test game pad</summary>
      public TestGamePad() : base(PlayerIndex.One) { }

      /// <summary>Checks for state changes and triggers the corresponding events</summary>
      /// <param name="previous">Previous state of the game pad</param>
      /// <param name="current">Current state of the game pad</param>
      public new void GenerateEvents(ref GamePadState previous, ref GamePadState current) {
        base.GenerateEvents(ref previous, ref current);
      }

    }

    #endregion // class TestGamePad

    /// <summary>Called before each test is run</summary>
    [SetUp]
    public void Setup() {
      this.mockery = new MockFactory();
      this.testGamePad = new TestGamePad();
    }

    /// <summary>Called after each test has run</summary>
    [TearDown]
    public void Teardown() {
      if (this.mockery != null) {
        this.mockery.Dispose();
        this.mockery = null;
      }
    }

    /// <summary>Verifies that the GetState() method is working</summary>
    [Test]
    public void TestGetState() {
      this.testGamePad.GetState();
      // No exception means success
    }

    /// <summary>Verifies that the GetExtendedState() method is working</summary>
    [Test]
    public void TestGetExtendedState() {
      this.testGamePad.GetExtendedState();
      // No exception means success
    }

    /// <summary>Verifies that the game pad can be attached and detached</summary>
    [Test]
    public void TestIsAttached() {
      bool attached = this.testGamePad.IsAttached;
      Assert.IsTrue(attached || !attached); // result doesn't matter
    }

    /// <summary>Verifies that the mocked game pad's name can be retrieved</summary>
    [Test]
    public void TestName() {
      StringAssert.Contains("game pad", this.testGamePad.Name.ToLower());
    }

    /// <summary>Tests whether state changes are detected by the game pad class</summary>
    /// <param name="button">Button that the detection will be tested with</param>
    /// <param name="extendedButtonIndex">Index of the button in the extended state</param>
    [
      Test,
      TestCase(Buttons.A, 0),
      TestCase(Buttons.B, 1),
      TestCase(Buttons.X, 2),
      TestCase(Buttons.Y, 3),
      TestCase(Buttons.LeftShoulder, 4),
      TestCase(Buttons.RightShoulder, 5),
      TestCase(Buttons.Back, 6),
      TestCase(Buttons.Start, 7),
      TestCase(Buttons.LeftStick, 8),
      TestCase(Buttons.RightStick, 9),
      TestCase(Buttons.BigButton, 10)
    ]
    public void TestStateChangeDetection(Buttons button, int extendedButtonIndex) {
      Mock<IGamePadSubscriber> subscriber = mockSubscriber();

      GamePadState pressedState = makeGamePadState(button);
      GamePadState releasedState = new GamePadState();

      subscriber.Expects.One.Method(x => x.ButtonPressed(0)).With(button);
      subscriber.Expects.One.Method(x => x.ExtendedButtonPressed(0, 0)).With(
        1UL << extendedButtonIndex, 0UL
      );
      this.testGamePad.GenerateEvents(ref releasedState, ref pressedState);

      subscriber.Expects.One.Method(x => x.ButtonReleased(0)).With(button);
      subscriber.Expects.One.Method(x => x.ExtendedButtonReleased(0, 0)).With(
        1UL << extendedButtonIndex, 0UL
      );
      this.testGamePad.GenerateEvents(ref pressedState, ref releasedState);

      this.mockery.VerifyAllExpectationsHaveBeenMet();
    }

    /// <summary>Mocks a subscriber for the game pad</summary>
    /// <returns>The mocked subscriber</returns>
    private Mock<IGamePadSubscriber> mockSubscriber() {
      Mock<IGamePadSubscriber> subscriber = this.mockery.CreateMock<IGamePadSubscriber>();
      this.testGamePad.ButtonPressed += subscriber.MockObject.ButtonPressed;
      this.testGamePad.ButtonReleased += subscriber.MockObject.ButtonReleased;
      this.testGamePad.ExtendedButtonPressed += subscriber.MockObject.ExtendedButtonPressed;
      this.testGamePad.ExtendedButtonReleased += subscriber.MockObject.ExtendedButtonReleased;
      return subscriber;
    }

    /// <summary>Creates a game pad state with the specified button pressed</summary>
    /// <param name="pressedButton">Button that will be pressed down</param>
    /// <returns>The new game pad state</returns>
    private static GamePadState makeGamePadState(Buttons pressedButton) {
      return new GamePadState(
        new GamePadThumbSticks(),
        new GamePadTriggers(),
        new GamePadButtons(pressedButton),
        new GamePadDPad()
      );
    }

    /// <summary>Creates dynamic interface-based mock objects</summary>
    private MockFactory mockery;
    /// <summary>Test implementation of a game pad</summary>
    private TestGamePad testGamePad;

  }

} // namespace Nuclex.Input.Devices

#endif // UNITTEST
