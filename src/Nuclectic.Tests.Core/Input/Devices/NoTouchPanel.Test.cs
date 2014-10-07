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

using NUnit.Framework;
using NMock;
using Microsoft.Xna.Framework;

namespace Nuclex.Input.Devices {

  /// <summary>Unit tests for the dummy touch panel</summary>
  [TestFixture]
  internal class NoTouchPanelTest {

    /// <summary>Verifies that the constructor is working</summary>
    [Test]
    public void TestConstructor() {
      var touchPanel = new NoTouchPanel();
      Assert.IsNotNull(touchPanel);
    }

    /// <summary>Verifies that the GetState() method is working</summary>
    [Test]
    public void TestGetState() {
      var touchPanel = new NoTouchPanel();
      touchPanel.GetState();
      // No exception means success
    }

    /// <summary>Verifies that the keyboard dummy is not attached</summary>
    [Test]
    public void TestIsAttached() {
      var touchPanel = new NoTouchPanel();
      Assert.IsFalse(touchPanel.IsAttached);
    }

    /// <summary>Verifies that the keyboard dummy's name can be retrieved</summary>
    [Test]
    public void TestName() {
      var touchPanel = new NoTouchPanel();
      StringAssert.Contains("no", touchPanel.Name.ToLower());
    }

    /// <summary>Verifies that the TakeSnapshot() method works</summary>
    [Test]
    public void TestTakeSnapshot() {
      var touchPanel = new NoTouchPanel();
      touchPanel.TakeSnapshot();
      // No exception means success
    }

    /// <summary>Verifies that the Update() method works</summary>
    [Test]
    public void TestUpdate() {
      var touchPanel = new NoTouchPanel();
      touchPanel.Update();
      // No exception means success
    }

    /// <summary>Verifies that the MaximumTouchCount property returns 0</summary>
    [Test]
    public void TestMaximumTouchCount() {
      var touchPanel = new NoTouchPanel();
      Assert.AreEqual(0, touchPanel.MaximumTouchCount);
    }

    /// <summary>Tests whether the no mouse class' events can be subscribed</summary>
    [Test]
    public void TestEvents() {
      var touchPanel = new NoTouchPanel();

      touchPanel.Pressed += touchPanelPressed;
      touchPanel.Pressed -= touchPanelPressed;

      touchPanel.Moved += touchPanelMoved;
      touchPanel.Moved -= touchPanelMoved;

      touchPanel.Released += touchPanelReleased;
      touchPanel.Released -= touchPanelReleased;
    }

    /// <summary>Dummy subscriber for the touch panel Pressed event</summary>
    /// <param name="id">ID of the touch</param>
    /// <param name="position">Position the user is touching the screen at</param>
    private static void touchPanelPressed(int id, Vector2 position) { }

    /// <summary>Dummy subscriber for the touch panel Moved event</summary>
    /// <param name="id">ID of the touch</param>
    /// <param name="position">Position the user has moved his touch to</param>
    private static void touchPanelMoved(int id, Vector2 position) { }

    /// <summary>Dummy subscriber for the touch panel Released event</summary>
    /// <param name="id">ID of the touch</param>
    /// <param name="position">Position at which the user has released the screen</param>
    private static void touchPanelReleased(int id, Vector2 position) { }

  }

} // namespace Nuclex.Input.Devices

#endif // UNITTEST
