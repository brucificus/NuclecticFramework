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

using StateEventHandler = System.EventHandler<System.EventArgs>;

namespace Nuclex.Game.States {

  /// <summary>Base class for updateable game states</summary>
  public abstract class GameState : IGameState, IUpdateable {

    /// <summary>Never called because the Enabled property cannot change</summary>
    event StateEventHandler IUpdateable.EnabledChanged { add { } remove { } }

    /// <summary>Never called because the UpdateOrder property cannot change</summary>
    event StateEventHandler IUpdateable.UpdateOrderChanged { add { } remove { } }

    /// <summary>Initializes a new game state</summary>
    /// <summary>Called when the game state is being paused</summary>
    public void Pause() {
      if (!this.paused) {
        OnPause();
        this.paused = true;
      }
    }

    /// <summary>Called when the game state is being resumed from pause mode</summary>
    public void Resume() {
      if (this.paused) {
        OnResume();
        this.paused = false;
      }
    }

    /// <summary>Called when the component needs to update its state.</summary>
    /// <param name="gameTime">Provides a snapshot of the Game's timing values</param>
    public abstract void Update(GameTime gameTime);

    /// <summary>Called when the game state has been entered</summary>
    protected virtual void OnEntered() { }

    /// <summary>Called when the game state is being left again</summary>
    protected virtual void OnLeaving() { }

    /// <summary>Called when the game state should enter pause mode</summary>
    protected virtual void OnPause() { }

    /// <summary>Called when the game state should resume from pause mode</summary>
    protected virtual void OnResume() { }

    /// <summary>Whether the game state is currently paused</summary>
    protected bool Paused {
      get { return this.paused; }
    }

    /// <summary>Called when the game state has been entered</summary>
    void IGameState.Enter() {
      OnEntered();
    }

    /// <summary>Called when the game state is being left again</summary>
    void IGameState.Leave() {
      OnLeaving();
    }

    /// <summary>
    ///   Always true to indicate the game state is enabled and should be updated
    /// </summary>
    bool IUpdateable.Enabled {
      get { return true; }
    }

    /// <summary>
    ///   Always 0 because game states have no ordering relative to each other
    /// </summary>
    int IUpdateable.UpdateOrder {
      get { return 0; }
    }

    /// <summary>Used to avoid pausing the game state multiple times</summary>
    private bool paused;

  }

} // namespace Nuclex.Game.States
