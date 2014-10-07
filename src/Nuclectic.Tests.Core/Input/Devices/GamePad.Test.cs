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

namespace Nuclex.Input.Devices {

  /// <summary>Unit tests for the game pad base class</summary>
  [TestFixture]
  internal class GamePadTest {

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
    private class TestGamePad : GamePad {

      /// <summary>Retrieves the current state of the game pad</summary>
      /// <returns>The current state of the game pad</returns>
      public override GamePadState GetState() {
        return new GamePadState();
      }

      /// <summary>Retrieves the current DirectInput joystick state</summary>
      /// <returns>The current state of the DirectInput joystick</returns>
      public override ExtendedGamePadState GetExtendedState() {
        return new ExtendedGamePadState();
      }

      /// <summary>Whether the input device is connected to the system</summary>
      public override bool IsAttached {
        get { return true; }
      }

      /// <summary>Human-readable name of the input device</summary>
      public override string Name {
        get { return "Test dummy"; }
      }

      /// <summary>Update the state of all input devices</summary>
      public override void Update() { }

      /// <summary>Takes a snapshot of the current state of the input device</summary>
      public override void TakeSnapshot() { }

      /// <summary>Whether subscribers to the standard button events exist</summary>
      public new bool HaveEventSubscribers {
        get { return base.HaveEventSubscribers; }
      }

      /// <summary>Whether subscribers to the extended button events exist</summary>
      public new bool HaveExtendedEventSubscribers {
        get { return base.HaveExtendedEventSubscribers; }
      }

      /// <summary>Triggers the ButtonPressed event</summary>
      /// <param name="buttons">Buttons that will be reported</param>
      public void FireButtonPressed(Buttons buttons) {
        OnButtonPressed(buttons);
      }

      /// <summary>Triggers the ButtonReleased event</summary>
      /// <param name="buttons">Buttons that will be reported</param>
      public void FireButtonReleased(Buttons buttons) {
        OnButtonReleased(buttons);
      }

      /// <summary>Triggers the ExtendedButtonPressed event</summary>
      /// <param name="buttons1">First set of buttons that will be reported</param>
      /// <param name="buttons2">Second set of buttons that will be reported</param>
      public void FireExtendedButtonPressed(ulong buttons1, ulong buttons2) {
        OnExtendedButtonPressed(buttons1, buttons2);
      }

      /// <summary>Triggers the ExtendedButtonReleased event</summary>
      /// <param name="buttons1">First set of buttons that will be reported</param>
      /// <param name="buttons2">Second set of buttons that will be reported</param>
      public void FireExtendedButtonReleased(ulong buttons1, ulong buttons2) {
        OnExtendedButtonReleased(buttons1, buttons2);
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

    /// <summary>Verifies that the ButtonPressed event is working</summary>
    [Test]
    public void TestButtonPressedEvent() {
      Mock<IGamePadSubscriber> subscriber = mockSubscriber();

      subscriber.Expects.One.Method(x => x.ButtonPressed(0)).With(Buttons.LeftShoulder);
      this.testGamePad.FireButtonPressed(Buttons.LeftShoulder);

      this.mockery.VerifyAllExpectationsHaveBeenMet();
    }

    /// <summary>Verifies that the ButtonReleased event is working</summary>
    [Test]
    public void TestButtonReleasedEvent() {
      Mock<IGamePadSubscriber> subscriber = mockSubscriber();

      subscriber.Expects.One.Method(x => x.ButtonReleased(0)).With(Buttons.RightStick);
      this.testGamePad.FireButtonReleased(Buttons.RightStick);

      this.mockery.VerifyAllExpectationsHaveBeenMet();
    }

    /// <summary>Verifies that the ExtendedButtonPressed event is working</summary>
    [Test]
    public void TestExtendedButtonPressedEvent() {
      Mock<IGamePadSubscriber> subscriber = mockSubscriber();

      subscriber.Expects.One.Method(x => x.ExtendedButtonPressed(0, 0)).With(
        0x1234567812345678UL, 0x8765432187654321UL
      );
      this.testGamePad.FireExtendedButtonPressed(
        0x1234567812345678UL, 0x8765432187654321UL
      );

      this.mockery.VerifyAllExpectationsHaveBeenMet();
    }

    /// <summary>Verifies that the ExtendedButtonReleased event is working</summary>
    [Test]
    public void TestExtendedButtonReleasedEvent() {
      Mock<IGamePadSubscriber> subscriber = mockSubscriber();

      subscriber.Expects.One.Method(x => x.ExtendedButtonReleased(0, 0)).With(
        0x8765432187654321UL, 0x1234567812345678UL
      );
      this.testGamePad.FireExtendedButtonReleased(
        0x8765432187654321UL, 0x1234567812345678UL
      );

      this.mockery.VerifyAllExpectationsHaveBeenMet();
    }

    /// <summary>Verifies that the HaveSubscribers property</summary>
    [Test]
    public void TestHaveSubscribers() {
      Assert.IsFalse(this.testGamePad.HaveEventSubscribers);

      Mock<IGamePadSubscriber> subscriber = this.mockery.CreateMock<IGamePadSubscriber>();
      this.testGamePad.ButtonPressed += subscriber.MockObject.ButtonPressed;

      Assert.IsTrue(this.testGamePad.HaveEventSubscribers);

      this.testGamePad.ButtonPressed -= subscriber.MockObject.ButtonPressed;
      this.testGamePad.ButtonReleased += subscriber.MockObject.ButtonReleased;

      Assert.IsTrue(this.testGamePad.HaveEventSubscribers);

      this.testGamePad.ButtonReleased -= subscriber.MockObject.ButtonReleased;

      Assert.IsFalse(this.testGamePad.HaveEventSubscribers);
    }

    /// <summary>Verifies that the HaveExtendedSubscribers property</summary>
    [Test]
    public void TestHaveExtendedSubscribers() {
      Assert.IsFalse(this.testGamePad.HaveExtendedEventSubscribers);

      Mock<IGamePadSubscriber> subscriber = this.mockery.CreateMock<IGamePadSubscriber>();
      this.testGamePad.ExtendedButtonPressed += subscriber.MockObject.ExtendedButtonPressed;

      Assert.IsTrue(this.testGamePad.HaveExtendedEventSubscribers);

      this.testGamePad.ExtendedButtonPressed -= subscriber.MockObject.ExtendedButtonPressed;
      this.testGamePad.ExtendedButtonReleased += subscriber.MockObject.ExtendedButtonReleased;

      Assert.IsTrue(this.testGamePad.HaveExtendedEventSubscribers);

      this.testGamePad.ExtendedButtonReleased -= subscriber.MockObject.ExtendedButtonReleased;

      Assert.IsFalse(this.testGamePad.HaveExtendedEventSubscribers);
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
