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

  /// <summary>
  ///   Possible behaviors of a game state in respect to the states its stacked on
  /// </summary>
  public enum GameStateModality {
  
    /// <summary>
    ///   The game state takes exclusive of the screen does not require the state
    ///   below it in the stack to be updated as long as it's active.
    /// </summary>
    Exclusive,
    
    /// <summary>
    ///   The game state sits on top of the state below it in the stack, but does
    ///   not completely obscure it or requires it to continue being updated.
    /// </summary>
    Popup
  
  }

} // namespace Nuclex.Game.States
