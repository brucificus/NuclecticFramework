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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using NUnit.Framework;
using NMock;

namespace Nuclex.Input.Devices {

  /// <summary>Unit tests for the touch state</summary>
  [TestFixture]
  internal class TouchStateTest {

    /// <summary>Tests whether the attached property is working</summary>
    [Test]
    public void TestAttachedProperty() {
      TouchState state = new TouchState(true, new TouchCollection());
      Assert.IsTrue(state.IsAttached);

      state = new TouchState(false, new TouchCollection());
      Assert.IsFalse(state.IsAttached);
    }

    /// <summary>Tests whether the touches property is working</summary>
    [Test]
    public void TestTouchesProperty() {
      TouchCollection touches = new TouchCollection(
        new TouchLocation[] {
          new TouchLocation(1, TouchLocationState.Pressed, new Vector2(12.34f, 56.78f))
        }
      );
      TouchState state = new TouchState(true, touches);

      Assert.AreEqual(touches, state.Touches);
    }

  }

} // namespace Nuclex.Input.Devices

#endif // UNITTEST
