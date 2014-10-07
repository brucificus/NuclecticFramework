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
  internal class GameStateTest {

    #region class TestGameState

    /// <summary>Game state used for unit testing</summary>
    private class TestGameState : GameState {

      /// <summary>Initializes a new test game state</summary>
      public TestGameState() { }

      /// <summary>
      ///   Allows the game state to run logic such as updating the world,
      ///   checking for collisions, gathering input and playing audio.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values</param>
      public override void Update(GameTime gameTime) {
        ++this.UpdateCallCount;
      }

      /// <summary>Called when the game state has been entered</summary>
      protected override void OnEntered() {
        ++this.OnEnteredCallCount;
        base.OnEntered(); // for test coverage...
      }

      /// <summary>Called when the game state is being left again</summary>
      protected override void OnLeaving() {
        ++this.OnLeavingCallCount;
        base.OnLeaving(); // for test coverage...
      }

      /// <summary>Called when the game state should enter pause mode</summary>
      protected override void OnPause() {
        ++this.OnPauseCallCount;
        base.OnPause(); // for test coverage...
      }

      /// <summary>Called when the game state should resume from pause mode</summary>
      protected override void OnResume() {
        ++this.OnResumeCallCount;
        base.OnResume(); // for test coverage...
      }

      /// <summary>Number of calls to the Update() method</summary>
      public int UpdateCallCount;
      /// <summary>Number of calls to the OnEntered() method</summary>
      public int OnEnteredCallCount;
      /// <summary>Number of calls to the OnLeaving() method</summary>
      public int OnLeavingCallCount;
      /// <summary>Number of calls to the OnPause() method</summary>
      public int OnPauseCallCount;
      /// <summary>Number of calls to the OnResume() method</summary>
      public int OnResumeCallCount;

    }

    #endregion // class TestGameState

    /// <summary>
    ///   Verifies that the constructor of the game state class is working
    /// </summary>
    [Test]
    public void TestConstructor() {
      GameState gameState = new TestGameState();
      Assert.IsNotNull(gameState); // nonsense, avoids compiler warning
    }

    /// <summary>
    ///   Verifies that the Update() call can be used on the game state
    /// </summary>
    [Test]
    public void TestUpdate() {
      var testGameState = new TestGameState();

      testGameState.Update(new GameTime());
      Assert.AreEqual(1, testGameState.UpdateCallCount);
    }

    /// <summary>
    ///   Tests whether the Enter() and Leave() notifications of a game state are
    ///   invoked at the appropriate times
    /// </summary>
    [Test]
    public void TestEnterLeave() {
      var testGameState = new TestGameState();

      Assert.AreEqual(0, testGameState.OnEnteredCallCount);
      ((IGameState)testGameState).Enter();
      Assert.AreEqual(1, testGameState.OnEnteredCallCount);

      Assert.AreEqual(0, testGameState.OnLeavingCallCount);
      ((IGameState)testGameState).Leave();
      Assert.AreEqual(1, testGameState.OnLeavingCallCount);
    }

    /// <summary>
    ///   Tests whether the Pause() and Resume() notifications of a game state are
    ///   invoked at the appropriate times
    /// </summary>
    [Test]
    public void TestPauseResume() {
      var testGameState = new TestGameState();

      Assert.AreEqual(0, testGameState.OnPauseCallCount);
      ((IGameState)testGameState).Pause();
      Assert.AreEqual(1, testGameState.OnPauseCallCount);

      Assert.AreEqual(0, testGameState.OnResumeCallCount);
      ((IGameState)testGameState).Resume();
      Assert.AreEqual(1, testGameState.OnResumeCallCount);
    }

    /// <summary>
    ///   Verifies that redundant calls to the Pause() method will not cause
    ///   multiple calls of the OnPaused() notification
    /// </summary>
    [Test]
    public void TestRedundantPauseIsIgnored() {
      var testGameState = new TestGameState();

      Assert.AreEqual(0, testGameState.OnPauseCallCount);
      ((IGameState)testGameState).Pause();
      Assert.AreEqual(1, testGameState.OnPauseCallCount);

      ((IGameState)testGameState).Pause();
      Assert.AreEqual(1, testGameState.OnPauseCallCount);
    }

    /// <summary>
    ///   Verifies that redundant calls to the Resume() method will not cause
    ///   multiple calls of the OnResume() notification
    /// </summary>
    [Test]
    public void TestRedundantResumeIsIgnored() {
      var testGameState = new TestGameState();

      Assert.AreEqual(0, testGameState.OnResumeCallCount);
      ((IGameState)testGameState).Resume();
      Assert.AreEqual(0, testGameState.OnResumeCallCount);

      ((IGameState)testGameState).Pause();
      ((IGameState)testGameState).Resume();
      Assert.AreEqual(1, testGameState.OnResumeCallCount);

      ((IGameState)testGameState).Resume();
      Assert.AreEqual(1, testGameState.OnResumeCallCount);
    }

    /// <summary>
    ///   Tests whether the Enabled property provided via the IUpdateable
    ///   interface is returned correctly
    /// </summary>
    [Test]
    public void TestEnabledProperty() {
      IUpdateable updateableState = new TestGameState();
      Assert.IsTrue(updateableState.Enabled);
    }

    /// <summary>
    ///   Tests whether the UpdateOrder property provided via the IUpdateable
    ///   interface is returned correctly
    /// </summary>
    [Test]
    public void TestUpdateOrderProperty() {
      IUpdateable updateableState = new TestGameState();
      Assert.AreEqual(0, updateableState.UpdateOrder);
    }

    /// <summary>
    ///   Tests whether the UpdateOrderChanged event can be used
    /// </summary>
    [Test]
    public void TestUpdateOrderChangedEvent() {
      IUpdateable updateableState = new TestGameState();

      // There's no point in doing any testing here because the events are
      // never triggered, thus no exception on this means the part that
      // needs to be implemented is working right (ie. doing nothing)
      updateableState.UpdateOrderChanged += delegate(object sender, EventArgs arguments) { };
      updateableState.UpdateOrderChanged -= delegate(object sender, EventArgs arguments) { };
    }

    /// <summary>
    ///   Tests whether the EnabledChanged event can be used
    /// </summary>
    [Test]
    public void TestEnabledChangedEvent() {
      IUpdateable updateableState = new TestGameState();

      // There's no point in doing any testing here because the events are
      // never triggered, thus no exception on this means the part that
      // needs to be implemented is working right (ie. doing nothing)
      updateableState.EnabledChanged += delegate(object sender, EventArgs arguments) { };
      updateableState.EnabledChanged -= delegate(object sender, EventArgs arguments) { };
    }

  }

} // namespace Nuclex.Game.States

#endif // UNITTEST
