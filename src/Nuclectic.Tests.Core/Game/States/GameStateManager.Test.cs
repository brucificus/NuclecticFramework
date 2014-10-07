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

  /// <summary>Unit test for the game state manager</summary>
  [TestFixture]
  public class GameStateManagerTest {

    #region class TestGameState

    /// <summary>Game state used for unit testing</summary>
    private class TestGameState : DrawableGameState, IDisposable {

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

      /// <summary>This is called when the game state should draw itself</summary>
      /// <param name="gameTime">Provides a snapshot of timing values</param>
      public override void Draw(GameTime gameTime) {
        ++this.DrawCallCount;
      }

      /// <summary>Immediately releases all resources owned by the instance</summary>
      public void Dispose() {
        ++this.DisposeCallCount;
      }

      /// <summary>Called when the game state has been entered</summary>
      protected override void OnEntered() {
        ++this.OnEnteredCallCount;
        base.OnEntered();
      }

      /// <summary>Called when the game state is being left again</summary>
      protected override void OnLeaving() {
        ++this.OnLeavingCallCount;
        base.OnLeaving();
      }

      /// <summary>Called when the game state should enter pause mode</summary>
      protected override void OnPause() {
        ++this.OnPauseCallCount;
        base.OnPause();
      }

      /// <summary>Called when the game state should resume from pause mode</summary>
      protected override void OnResume() {
        ++this.OnResumeCallCount;
        base.OnResume();
      }

      /// <summary>Number of calls to the Dispose() method</summary>
      public int DisposeCallCount;
      /// <summary>Number of calls to the Update() method</summary>
      public int UpdateCallCount;
      /// <summary>Number of calls to the DraW() method</summary>
      public int DrawCallCount;
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

    #region class UnenterableGameState

    /// <summary>Game state the canont be entered</summary>
    private class UnenterableGameState : GameState {

      /// <summary>Initializes a new unenterable game state</summary>
      public UnenterableGameState() { }

      /// <summary>
      ///   Allows the game state to run logic such as updating the world,
      ///   checking for collisions, gathering input and playing audio.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values</param>
      public override void Update(GameTime gameTime) { }

      /// <summary>Called when the game state has been entered</summary>
      protected override void OnEntered() {
        throw new InvalidOperationException("Simulated error for unit testing");
      }

    }

    #endregion // class UnenterableGameState

    #region class ReentrantGameState

    /// <summary>Game state that nests another game state upon being entered</summary>
    private class ReentrantGameState : GameState {

      /// <summary>Initializes a new unresumable game state</summary>
      /// <param name="gameStateService">
      ///   Game state manager the unresumable game state belongs to
      /// </param>
      public ReentrantGameState(IGameStateService gameStateService) {
        this.gameStateService = gameStateService;
      }

      /// <summary>
      ///   Allows the game state to run logic such as updating the world,
      ///   checking for collisions, gathering input and playing audio.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values</param>
      public override void Update(GameTime gameTime) { }

      /// <summary>Called when the game state has been entered</summary>
      protected override void OnEntered() {
        this.gameStateService.Push(new TestGameState());
      }

      /// <summary>Game state service the reentrant state will use</summary>
      private IGameStateService gameStateService;

    }

    #endregion // class ReentrantGameState

    /// <summary>
    ///   Verifies that the constructor of the game state manager is working
    /// </summary>
    [Test]
    public void TestDefaultConstructor() {
      var manager = new GameStateManager();
      Assert.IsNotNull(manager); // nonsense, avoids compiler warning
    }

    /// <summary>
    ///   Verifies that the constructor of the game state manager accepting
    ///   a reference to the GameServiceCollection is working
    /// </summary>
    [Test]
    public void TestGameServiceConstructor() {
      var services = new GameServiceContainer();

      Assert.IsNull(services.GetService(typeof(IGameStateService)));
      using (var manager = new GameStateManager(services)) {
        Assert.IsNotNull(services.GetService(typeof(IGameStateService)));
      }
      Assert.IsNull(services.GetService(typeof(IGameStateService)));
    }

    /// <summary>
    ///   Tests whether disposing the game state manager causes it to leave
    ///   the active game state
    /// </summary>
    [Test]
    public void TestLeaveOnDisposal() {
      var test = new TestGameState();
      Assert.AreEqual(0, test.OnLeavingCallCount);

      using (var manager = new GameStateManager()) {
        manager.Push(test);
      }

      Assert.AreEqual(1, test.OnLeavingCallCount);
    }

    /// <summary>
    ///   Tests whether disposing the game state manager disposes the currently
    ///   active game states when it is disposed itself.
    /// </summary>
    /// <param name="disposalEnabled">Whether to run the test with enabled disposal</param>
    [Test, TestCase(true), TestCase(false)]
    public void TestDisposeActiveStatesOnDisposal(bool disposalEnabled) {
      var test = new TestGameState();

      Assert.AreEqual(0, test.DisposeCallCount);
      using (var manager = new GameStateManager()) {
        manager.DisposeDroppedStates = disposalEnabled;
        manager.Push(test);
      }

      // The manager should only dispose the state if disposal was enabled
      if (disposalEnabled) {
        Assert.AreEqual(1, test.DisposeCallCount);
      } else {
        Assert.AreEqual(0, test.DisposeCallCount);
      }
    }

    /// <summary>
    ///   Verifies that the Pause() and Resume() methods are propagated to
    ///   the topmost game state on the stack
    /// </summary>
    [Test]
    public void TestPauseAndResume() {
      var obscured = new TestGameState();
      var active = new TestGameState();

      using (var manager = new GameStateManager()) {
        manager.Push(obscured);

        Assert.AreEqual(0, obscured.OnPauseCallCount);
        manager.Push(active);
        Assert.AreEqual(1, obscured.OnPauseCallCount);

        Assert.AreEqual(0, active.OnPauseCallCount);
        manager.Pause();
        Assert.AreEqual(1, active.OnPauseCallCount);

        Assert.AreEqual(0, active.OnResumeCallCount);
        manager.Resume();
        Assert.AreEqual(1, active.OnResumeCallCount);

        Assert.AreEqual(0, obscured.OnResumeCallCount);
        manager.Pop();
        Assert.AreEqual(1, obscured.OnResumeCallCount);
      }
    }

    /// <summary>
    ///   Verifies that the Push() method respects the modality parameter
    /// </summary>
    /// <param name="modality">Modality that will be tested</param>
    [Test, TestCase(GameStateModality.Exclusive), TestCase(GameStateModality.Popup)]
    public void TestPushModality(GameStateModality modality) {
      var alwaysObscured = new TestGameState();
      var potentiallyObscured = new TestGameState();
      var active = new TestGameState();

      using (var manager = new GameStateManager()) {
        manager.Push(alwaysObscured);
        manager.Push(potentiallyObscured);
        manager.Push(active, modality);

        Assert.AreEqual(0, alwaysObscured.UpdateCallCount);
        Assert.AreEqual(0, alwaysObscured.DrawCallCount);
        Assert.AreEqual(0, potentiallyObscured.UpdateCallCount);
        Assert.AreEqual(0, potentiallyObscured.DrawCallCount);
        Assert.AreEqual(0, active.UpdateCallCount);
        Assert.AreEqual(0, active.DrawCallCount);

        manager.Update(new GameTime());
        manager.Draw(new GameTime());

        Assert.AreEqual(0, alwaysObscured.UpdateCallCount);
        Assert.AreEqual(0, alwaysObscured.DrawCallCount);
        if (modality == GameStateModality.Exclusive) {
          Assert.AreEqual(0, potentiallyObscured.UpdateCallCount);
          Assert.AreEqual(0, potentiallyObscured.DrawCallCount);
        } else {
          Assert.AreEqual(1, potentiallyObscured.UpdateCallCount);
          Assert.AreEqual(1, potentiallyObscured.DrawCallCount);
        }
        Assert.AreEqual(1, active.UpdateCallCount);
        Assert.AreEqual(1, active.DrawCallCount);
      }
    }

    /// <summary>
    ///   Verifies that an exception whilst pushing a state on the stack leaves the
    ///   game state manager unchanged
    /// </summary>
    [Test]
    public void TestPushUnenterableState() {
      var test = new TestGameState();

      using (var manager = new GameStateManager()) {
        manager.Push(test);
        Assert.AreSame(test, manager.ActiveState);

        Assert.Throws<InvalidOperationException>(
          delegate() { manager.Push(new UnenterableGameState()); }
        );

        Assert.AreSame(test, manager.ActiveState);

        // Make sure the test state is still running. Whether pause was
        // called zero times or more, we only care that it's running after
        // the push has failed
        Assert.AreEqual(test.OnResumeCallCount, test.OnPauseCallCount);
      }
    }

    /// <summary>
    ///   Tests whether the game state manager correctly handles a reentrant call
    ///   to Push() from a pushed game state
    /// </summary>
    [Test]
    public void TestReeantrantPush() {
      using (var manager = new GameStateManager()) {
        var reentrant = new ReentrantGameState(manager);
        manager.Push(reentrant);

        // The reentrant game state pushes another game state onto the stack in its
        // OnEntered() notification. If this causes the stack to be built in the wrong
        // order, the ReentrantGameState would become the new active game state instead
        // of the sub-game-state it pushed onto the stack.
        Assert.AreNotSame(reentrant, manager.ActiveState);
      }
    }

    /// <summary>Verifies that the disposal of dropped states in Pop() works</summary>
    /// <param name="disposalEnabled">Whether to run the test with enabled disposal</param>
    [Test, TestCase(true), TestCase(false)]
    public void TestDisposalInPop(bool disposalEnabled) {
      var test = new TestGameState();

      using (var manager = new GameStateManager()) {
        manager.DisposeDroppedStates = disposalEnabled;
        manager.Push(test);

        Assert.AreEqual(0, test.DisposeCallCount);
        Assert.AreSame(test, manager.Pop());
        if (disposalEnabled) {
          Assert.AreEqual(1, test.DisposeCallCount);
        } else {
          Assert.AreEqual(0, test.DisposeCallCount);
        }
      }
    }

    /// <summary>
    ///   Verifies that the game state manager correctly rolls back its update
    ///   and draw lists when an exclusive state is popped from the stack
    /// </summary>
    [Test]
    public void TestUpdateAndDrawListRollbackInPop() {
      var obscured = new TestGameState();
      var blocker1 = new TestGameState();
      var popup = new TestGameState();
      var blocker2 = new TestGameState();

      using (var manager = new GameStateManager()) {
        manager.Push(obscured);
        manager.Push(blocker1);
        manager.Push(popup, GameStateModality.Popup);

        manager.Update(new GameTime());
        Assert.AreEqual(0, obscured.UpdateCallCount);
        Assert.AreEqual(1, blocker1.UpdateCallCount);
        Assert.AreEqual(1, popup.UpdateCallCount);
        Assert.AreEqual(0, blocker2.UpdateCallCount);

        manager.Push(blocker2);
        manager.Update(new GameTime());
        Assert.AreEqual(0, obscured.UpdateCallCount);
        Assert.AreEqual(1, blocker1.UpdateCallCount);
        Assert.AreEqual(1, popup.UpdateCallCount);
        Assert.AreEqual(1, blocker2.UpdateCallCount);

        manager.Pop();
        manager.Update(new GameTime());
        Assert.AreEqual(0, obscured.UpdateCallCount);
        Assert.AreEqual(2, blocker1.UpdateCallCount);
        Assert.AreEqual(2, popup.UpdateCallCount);
        Assert.AreEqual(1, blocker2.UpdateCallCount);
      }
    }

    /// <summary>
    ///   Verifies that the game state manager correctly rolls back its update
    ///   and draw lists when an exclusive state is popped from the stack
    /// </summary>
    [Test]
    public void TestUpdateAndDrawListRollbackInSwitch() {
      var obscured = new TestGameState();
      var blocker1 = new TestGameState();
      var popup = new TestGameState();
      var blocker2 = new TestGameState();

      using (var manager = new GameStateManager()) {
        manager.Push(obscured);
        manager.Push(blocker1);
        manager.Push(popup, GameStateModality.Popup);

        manager.Update(new GameTime());
        Assert.AreEqual(0, obscured.UpdateCallCount);
        Assert.AreEqual(1, blocker1.UpdateCallCount);
        Assert.AreEqual(1, popup.UpdateCallCount);
        Assert.AreEqual(0, blocker2.UpdateCallCount);

        manager.Push(blocker2);
        manager.Update(new GameTime());
        Assert.AreEqual(0, obscured.UpdateCallCount);
        Assert.AreEqual(1, blocker1.UpdateCallCount);
        Assert.AreEqual(1, popup.UpdateCallCount);
        Assert.AreEqual(1, blocker2.UpdateCallCount);

        manager.Switch(blocker2, GameStateModality.Popup);
        manager.Update(new GameTime());
        Assert.AreEqual(0, obscured.UpdateCallCount);
        Assert.AreEqual(2, blocker1.UpdateCallCount);
        Assert.AreEqual(2, popup.UpdateCallCount);
        Assert.AreEqual(2, blocker2.UpdateCallCount);
      }
    }

    /// <summary>Verifies that the disposal of switched out states in Switch() works</summary>
    /// <param name="disposalEnabled">Whether to run the test with enabled disposal</param>
    [Test, TestCase(true), TestCase(false)]
    public void TestDisposalInSwitch(bool disposalEnabled) {
      var test = new TestGameState();

      using (var manager = new GameStateManager()) {
        manager.DisposeDroppedStates = disposalEnabled;
        manager.Push(test);

        Assert.AreEqual(0, test.DisposeCallCount);
        Assert.AreSame(test, manager.Switch(new TestGameState()));
        if (disposalEnabled) {
          Assert.AreEqual(1, test.DisposeCallCount);
        } else {
          Assert.AreEqual(0, test.DisposeCallCount);
        }
      }
    }

    /// <summary>
    ///   Verifies that switch only replaces the active game state,
    ///   not the whole stack
    /// </summary>
    [Test]
    public void TestSwitchOnlyChangesActiveState() {
      var obscured = new TestGameState();
      var active = new TestGameState();

      using (var manager = new GameStateManager()) {
        manager.Push(obscured);
        manager.Push(active);

        Assert.AreEqual(0, obscured.OnLeavingCallCount);
        Assert.AreEqual(0, active.OnLeavingCallCount);

        manager.Switch(new TestGameState());

        Assert.AreEqual(0, obscured.OnLeavingCallCount);
        Assert.AreEqual(1, active.OnLeavingCallCount);
      }
    }

    /// <summary>
    ///   Verifies that the active game state can be queried
    /// </summary>
    [Test]
    public void TestActiveState() {
      var test = new TestGameState();

      using (var manager = new GameStateManager()) {
        Assert.IsNull(manager.ActiveState);

        manager.Push(test);

        Assert.AreSame(test, manager.ActiveState);
      }
    }

  }

} // namespace Nuclex.Game.States

#endif // UNITTEST
