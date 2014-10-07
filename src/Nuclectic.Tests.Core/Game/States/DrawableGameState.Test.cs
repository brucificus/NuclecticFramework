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
using Microsoft.Xna.Framework.Graphics;

using NUnit.Framework;

namespace Nuclex.Game.States {

  /// <summary>Unit test for the game state class</summary>
  [TestFixture]
  internal class DrawableGameStateTest {

    #region class TestGameState

    /// <summary>Game state used for unit testing</summary>
    private class TestGameState : DrawableGameState {

      /// <summary>Initializes a new test game state</summary>
      public TestGameState() { }

      /// <summary>
      ///   Allows the game state to run logic such as updating the world,
      ///   checking for collisions, gathering input and playing audio.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values</param>
      public override void Update(GameTime gameTime) { }

      /// <summary>This is called when the game state should draw itself</summary>
      /// <param name="gameTime">Provides a snapshot of timing values</param>
      public override void Draw(GameTime gameTime) {
        ++this.DrawCallCount;
      }

      /// <summary>Number of calls to the DraW() method</summary>
      public int DrawCallCount;

    }

    #endregion // class TestGameState

    /// <summary>
    ///   Verifies that the constructor of the drawable game state is working
    /// </summary>
    [Test]
    public void TestConstructor() {
      GameState gameState = new TestGameState();
    }

    /// <summary>
    ///   Verifies that the Draw() call can be used on the game state
    /// </summary>
    [Test]
    public void TestDraw() {
      var gameState = new TestGameState();

      gameState.Draw(new GameTime());
      Assert.AreEqual(1, gameState.DrawCallCount);
    }

    /// <summary>
    ///   Tests whether the Visible property provided via the IDrawable
    ///   interface is returned correctly
    /// </summary>
    [Test]
    public void TestVisibleProperty() {
      IDrawable drawableState = new TestGameState();
      Assert.IsTrue(drawableState.Visible);
    }

    /// <summary>
    ///   Tests whether the DrawOrder property provided via the IDrawable
    ///   interface is returned correctly
    /// </summary>
    [Test]
    public void TestDrawOrderProperty() {
      IDrawable drawableState = new TestGameState();
      Assert.AreEqual(0, drawableState.DrawOrder);
    }

    /// <summary>
    ///   Tests whether the DrawOrderChanged event can be used
    /// </summary>
    [Test]
    public void TestDrawOrderChangedEvent() {
      IDrawable drawableState = new TestGameState();

      // There's no point in doing any testing here because the events are
      // never triggered, thus no exception on this means the part that
      // needs to be implemented is working right (ie. doing nothing)
      drawableState.DrawOrderChanged += delegate(object sender, EventArgs arguments) { };
      drawableState.DrawOrderChanged -= delegate(object sender, EventArgs arguments) { };
    }

    /// <summary>
    ///   Tests whether the VisibleChanged event can be used
    /// </summary>
    [Test]
    public void TestVisibleChangedEvent() {
      IDrawable drawableState = new TestGameState();

      // There's no point in doing any testing here because the events are
      // never triggered, thus no exception on this means the part that
      // needs to be implemented is working right (ie. doing nothing)
      drawableState.VisibleChanged += delegate(object sender, EventArgs arguments) { };
      drawableState.VisibleChanged -= delegate(object sender, EventArgs arguments) { };
    }

  }

} // namespace Nuclex.Game.States

#endif // UNITTEST
