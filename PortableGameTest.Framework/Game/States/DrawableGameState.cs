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

  /// <summary>Base class for updateable and drawable game states</summary>
  public abstract class DrawableGameState : GameState, IDrawable {

    /// <summary>Never called because the Visible property cannot change</summary>
    event StateEventHandler IDrawable.VisibleChanged { add { } remove { } }

    /// <summary>Never called because the DrawOrder property cannot change</summary>
    event StateEventHandler IDrawable.DrawOrderChanged { add { } remove { } }

    /// <summary>Called when the drawable component needs to draw itself</summary>
    /// <param name="gameTime">Provides a snapshot of the game's timing values</param>
    public abstract void Draw(GameTime gameTime);

    /// <summary>
    ///   Always 0 because game states have no ordering relative to each other
    /// </summary>
    int IDrawable.DrawOrder {
      get { return 0; }
    }

    /// <summary>
    ///   Always true to indicate the game state is visible and should be drawn
    /// </summary>
    bool IDrawable.Visible {
      get { return true; }
    }

  }

} // namespace Nuclex.Game.States
