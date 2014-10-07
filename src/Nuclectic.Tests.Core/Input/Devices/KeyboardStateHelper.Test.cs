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

  /// <summary>Unit tests for the modifiable keyboard state</summary>
  [TestFixture]
  internal class KeyboardStateHelperTest {

    /// <summary>Tests whether the AddPressedKey() performs as expected</summary>
    [Test]
    public void TestAddPressedKey() {
      var state = new KeyboardState();
      KeyboardStateHelper.AddPressedKey(ref state, (int)Keys.A);
      KeyboardStateHelper.AddPressedKey(ref state, (int)Keys.C);

      Assert.IsTrue(state.IsKeyDown(Keys.A));
      Assert.IsFalse(state.IsKeyDown(Keys.B));
      Assert.IsTrue(state.IsKeyDown(Keys.C));
      Assert.IsFalse(state.IsKeyDown(Keys.D));
    }

    /// <summary>Tests whether the RemovePressedKey() performs as expected</summary>
    [Test]
    public void TestRemovedPressedKey() {
      var state = new KeyboardState(Keys.A, Keys.B, Keys.C, Keys.D);
      KeyboardStateHelper.RemovePressedKey(ref state, (int)Keys.A);
      KeyboardStateHelper.RemovePressedKey(ref state, (int)Keys.C);

      Assert.IsFalse(state.IsKeyDown(Keys.A));
      Assert.IsTrue(state.IsKeyDown(Keys.B));
      Assert.IsFalse(state.IsKeyDown(Keys.C));
      Assert.IsTrue(state.IsKeyDown(Keys.D));
    }

  }

} // namespace Nuclex.Input.Devices

#endif // UNITTEST
