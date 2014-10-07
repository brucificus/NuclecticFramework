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
using Microsoft.Xna.Framework.Input.Touch;

using NUnit.Framework;
using NMock;

namespace Nuclex.Input.Devices {

  /// <summary>Unit tests for the touch collection helper</summary>
  [TestFixture]
  internal class TouchCollectionHelperTest {

    /// <summary>
    ///   Tests whether the number of touch locations in the collection can be set
    /// </summary>
    [Test]
    public void TestSetLocationCount() {
      var touches = new TouchCollection();

      Assert.AreEqual(0, touches.Count);
      TouchCollectionHelper.SetLocationCount(ref touches, 2);
      Assert.AreEqual(2, touches.Count);
    }

    /// <summary>Verifies that a touch collection can be cleared</summary>
    [Test]
    public void TestClear() {
      var touches = new TouchCollection();

      TouchCollectionHelper.SetLocationCount(ref touches, 3);
      TouchCollectionHelper.Clear(ref touches);

      Assert.AreEqual(0, touches.Count);
    }

    /// <summary>
    ///   Verifies that touch location can be added to a touch collection
    /// </summary>
    [Test]
    public void TestAddTouchLocation() {
      var touches = new TouchCollection();

      TouchCollectionHelper.AddTouchLocation(
        ref touches, 1,
        TouchLocationState.Pressed, 12, 34,
        TouchLocationState.Released, 56, 78
      );

      Assert.AreEqual(1, touches.Count);

      TouchLocation location = touches[0];

      Assert.AreEqual(12, location.Position.X);
      Assert.AreEqual(34, location.Position.Y);
      Assert.AreEqual(TouchLocationState.Pressed, location.State);
      Assert.AreEqual(1, location.Id);
    }

  }

} // namespace Nuclex.Input.Devices

#endif // UNITTEST
