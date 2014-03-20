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

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Nuclex.Game.States {

  /// <summary>Manages the game states and updates the active game state</summary>
  public class GameStateManager : DrawableComponent, IGameStateService, IDisposable {

    /// <summary>Initializes a new game state manager</summary>
    public GameStateManager() {
      this.gameStates = new List<KeyValuePair<IGameState, GameStateModality>>();
      this.updateableStates = new List<IUpdateable>();
      this.drawableStates = new List<IDrawable>();
    }

    /// <summary>Initializes a new game state manager</summary>
    /// <param name="gameServices">
    ///   Services container the game state manager will add itself to
    /// </param>
    public GameStateManager(GameServiceContainer gameServices) :
      this() {
      this.gameServices = gameServices;
      gameServices.AddService(typeof(IGameStateService), this);
    }

    /// <summary>Immediately releases all resources used by the component</summary>
    public void Dispose() {
      leaveAllActiveStates();

      // Unregister the service if we have registered it before
      if (this.gameServices != null) {
        object registeredService = this.gameServices.GetService(typeof(IGameStateService));
        if (ReferenceEquals(registeredService, this)) {
          this.gameServices.RemoveService(typeof(IGameStateService));
        }
      }
    }

    /// <summary>
    ///   Whether the game state manager should automatically dispose game states
    ///   that are dropped from its stack
    /// </summary>
    public bool DisposeDroppedStates {
      get { return this.disposeDroppedStates; }
      set { this.disposeDroppedStates = value; }
    }

    /// <summary>Pauses the currently active state</summary>
    public void Pause() {
      if (this.gameStates.Count > 0) {
        this.gameStates[this.gameStates.Count - 1].Key.Pause();
      }
    }

    /// <summary>Resumes the currently active state</summary>
    public void Resume() {
      if (this.gameStates.Count > 0) {
        this.gameStates[this.gameStates.Count - 1].Key.Resume();
      }
    }

    /// <summary>Pushes the specified state onto the state stack</summary>
    /// <param name="state">State that will be pushed onto the stack</param>
    public void Push(IGameState state) {
      Push(state, GameStateModality.Exclusive);
    }

    /// <summary>Pushes the specified state onto the state stack</summary>
    /// <param name="state">State that will be pushed onto the stack</param>
    /// <param name="modality">
    ///   Behavior of the game state in relation to the state(s) below it on the stack
    /// </param>
    public void Push(IGameState state, GameStateModality modality) {
      Pause();

      // If this game state is modal, take all game states that came before it
      // from the draw and update lists
      if (modality == GameStateModality.Exclusive) {
        this.drawableStates.Clear();
        this.updateableStates.Clear();
      }

      // Add the new state to the update and draw lists if it implements
      // the required interfaces
      this.gameStates.Add(new KeyValuePair<IGameState, GameStateModality>(state, modality));
      appendToUpdateableAndDrawableList(state);

      // State is set, now try to enter it
      try {
        state.Enter();
      }
      catch (Exception) {
        Pop();
        throw;
      }
    }

    /// <summary>Takes the currently active game state from the stack</summary>
    /// <returns>The game state that has been popped from the stack</returns>
    public IGameState Pop() {
      int lastStateIndex = this.gameStates.Count - 1;
      if (lastStateIndex < 0) {
        throw new InvalidOperationException("No game states are on the stack");
      }

      KeyValuePair<IGameState, GameStateModality> old = this.gameStates[lastStateIndex];
      IGameState oldState = old.Key;

      // Notify the currently active state that it's being left and take it
      // from the stack of active states
      oldState.Leave();
      this.gameStates.RemoveAt(lastStateIndex);

      // Now we need to remove the popped state from our update and draw lists.
      // If the popped state was exclusive, our lists are empty and we need to
      // rebuild them. Otherwise, we can simply remove the lastmost entry.
      if (old.Value == GameStateModality.Exclusive) {
        this.updateableStates.Clear();
        this.drawableStates.Clear();
        rebuildUpdateableAndDrawableListRecursively(lastStateIndex - 1);
      } else {
        removeFromUpdateableAndDrawableList(old.Key);
      }

      // If the user desires so, dispose the dropped state
      disposeIfSupportedAndDesired(old.Key);

      // Resume the state that has now become the top of the stack
      Resume();

      return oldState;
    }

    /// <summary>Switches the game to the specified state</summary>
    /// <param name="state">State the game will be switched to</param>
    /// <returns>The game state that was replaced on the stack</returns>
    /// <remarks>
    ///   This replaces the running game state on the stack with the specified state.
    /// </remarks>
    public IGameState Switch(IGameState state) {
      return Switch(state, GameStateModality.Exclusive);
    }

    /// <summary>Switches the game to the specified state</summary>
    /// <param name="state">State the game will be switched to</param>
    /// <param name="modality">
    ///   Behavior of the game state in relation to the state(s) below it on the stack
    /// </param>
    /// <returns>The game state that was replaced on the stack</returns>
    /// <remarks>
    ///   This replaces the running game state on the stack with the specified state.
    /// </remarks>
    public IGameState Switch(IGameState state, GameStateModality modality) {
      int stateCount = this.gameStates.Count;
      if (stateCount == 0) {
        Push(state, modality);
        return null;
      }

      int lastStateIndex = stateCount - 1;
      KeyValuePair<IGameState, GameStateModality> old = this.gameStates[lastStateIndex];
      IGameState previousState = old.Key;

      // Notify the previous state that it's being left and kill it if desired
      previousState.Leave();
      disposeIfSupportedAndDesired(previousState);

      // If the switched-to state is exclusive, we need to clear the update
      // and draw lists. If not, depending on whether the previous state was
      // a popup state, we might have to 
      if (old.Value == GameStateModality.Popup) {
        removeFromUpdateableAndDrawableList(previousState);
      } else {
        this.updateableStates.Clear();
        this.drawableStates.Clear();
      }

      // Now swap out the state and put it in the update and draw lists. If we're
      // switching from an exclusive to a pop-up state, the draw and update lists need
      // to be rebuilt.
      var newState = new KeyValuePair<IGameState, GameStateModality>(state, modality);
      this.gameStates[lastStateIndex] = newState;
      if (old.Value == GameStateModality.Exclusive && modality == GameStateModality.Popup) {
        rebuildUpdateableAndDrawableListRecursively(lastStateIndex);
      } else {
        appendToUpdateableAndDrawableList(state);
      }
      
      // Let the state know that it has been entered
      state.Enter();

      return previousState;
    }

    /// <summary>The currently active game state. Can be null.</summary>
    public IGameState ActiveState {
      get {
        int count = this.gameStates.Count;
        if (count == 0) {
          return null;
        } else {
          return this.gameStates[count - 1].Key;
        }
      }
    }

    /// <summary>Updates the active game state</summary>
    /// <param name="gameTime">Snapshot of the game's timing values</param>
    public override void Update(GameTime gameTime) {
      for (int index = 0; index < this.updateableStates.Count; ++index) {
        var updateable = this.updateableStates[index];
        if (updateable.Enabled) {
          updateable.Update(gameTime);
        }
      }
    }

    /// <summary>Draws the active game state</summary>
    /// <param name="gameTime">Snapshot of the game's timing values</param>
    public override void Draw(GameTime gameTime) {
      for (int index = 0; index < this.drawableStates.Count; ++index) {
        var drawable = this.drawableStates[index];
        if (drawable.Visible) {
          this.drawableStates[index].Draw(gameTime);
        }
      }
    }

    /// <summary>
    ///   Disposes the specified state if disposal is enabled and the state implements
    ///   the IDisposable interface
    /// </summary>
    /// <param name="state">State that will be disposed if desired and supported</param>
    private void disposeIfSupportedAndDesired(IGameState state) {
      if (this.disposeDroppedStates) {
        var disposable = state as IDisposable;
        if (disposable != null) {
          disposable.Dispose();
        }
      }
    }

    /// <summary>
    ///   Rebuilds the updateable and drawable lists by recursively going up
    ///   the stacked game states until the top or an exclusive game state
    ///   is reached
    /// </summary>
    /// <param name="index">Index of the game state to start at</param>
    private void rebuildUpdateableAndDrawableListRecursively(int index) {
      if (index < 0) {
        return;
      }

      if (this.gameStates[index].Value != GameStateModality.Exclusive) {
        rebuildUpdateableAndDrawableListRecursively(index - 1);
      }

      appendToUpdateableAndDrawableList(this.gameStates[index].Key);
    }

    /// <summary>
    ///   Removes the specified state from the update and draw lists if it is on
    ///   the top of those lists
    /// </summary>
    /// <param name="state">
    ///   State that will be removed from the update and draw lists
    /// </param>
    private void removeFromUpdateableAndDrawableList(IGameState state) {
      int lastDrawableIndex = this.drawableStates.Count - 1;
      if (lastDrawableIndex > -1) {
        if (ReferenceEquals(this.drawableStates[lastDrawableIndex], state)) {
          this.drawableStates.RemoveAt(lastDrawableIndex);
        }
      }

      int lastUpdateableIndex = this.updateableStates.Count - 1;
      if (lastUpdateableIndex > -1) {
        if (ReferenceEquals(this.updateableStates[lastUpdateableIndex], state)) {
          this.updateableStates.RemoveAt(lastUpdateableIndex);
        }
      }
    }

    /// <summary>Leaves all currently active game states</summary>
    private void leaveAllActiveStates() {
      for (int index = this.gameStates.Count - 1; index >= 0; --index) {
        IGameState state = this.gameStates[index].Key;
        state.Leave();
        disposeIfSupportedAndDesired(state);
        this.gameStates.RemoveAt(index);
      }

      this.drawableStates.Clear();
      this.updateableStates.Clear();
    }

    /// <summary>Appends the specified state to the update and draw lists</summary>
    /// <param name="state">State that will be appended to the lists</param>
    private void appendToUpdateableAndDrawableList(IGameState state) {
      IUpdateable updateable = state as IUpdateable;
      if (updateable != null) {
        this.updateableStates.Add(updateable);
      }

      IDrawable drawable = state as IDrawable;
      if (drawable != null) {
        this.drawableStates.Add(drawable);
      }
    }

    /// <summary>
    ///   Game service container the game state manager has registered itself in
    /// </summary>
    private GameServiceContainer gameServices;

    /// <summary>Whether the game state manager should dispose dropped states</summary>
    private bool disposeDroppedStates;

    /// <summary>Currently active game states</summary>
    /// <remarks>
    ///   The game state manager supports multiple active game states. For example,
    ///   a menu might appear on top of the running game. Only the topmost active
    ///   state receives input through the game 
    /// </remarks>
    private List<KeyValuePair<IGameState, GameStateModality>> gameStates;
    /// <summary>Currently active game states implementing IUpdateable</summary>
    private List<IUpdateable> updateableStates;
    /// <summary>Currently active game states implementing IDrawable</summary>
    private List<IDrawable> drawableStates;

  }

} // namespace Nuclex.Game.States
