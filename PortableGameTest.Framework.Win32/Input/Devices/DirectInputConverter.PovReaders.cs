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

using PortableGameTest.Framework.Input.Devices;
#if !NO_DIRECTINPUT
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using SlimDX.DirectInput;

using XnaGamePadButtons = Microsoft.Xna.Framework.Input.GamePadButtons;

namespace Nuclex.Input.Devices {

  partial class DirectInputConverter {

    #region interface IPovReader

      #endregion

    #region class PovReader

    /// <summary>Reads the state of a specified point-of-view controller</summary>
    private class PovReader : IPovReader {

      /// <summary>Initializes a new PoV reader</summary>
      /// <param name="povIndex">Index of the PoV controller that will be read</param>
      public PovReader(int povIndex) {
        this.povIndex = povIndex;
      }

      /// <summary>Retrieves the current direction of the PoV controller</summary>
      /// <param name="povs">PoV states the direction will be read from</param>
      /// <returns>The direction of the PoV controller</returns>
      public int GetDirection(int[] povs) {
        return povs[this.povIndex];
      }

      /// <summary>
      ///   Reports whether the state of the point-of-view controller has changed
      /// </summary>
      /// <param name="previous">Previous states of the PoV controllers</param>
      /// <param name="current">Current states of the PoV controllers</param>
      /// <returns>True if the state of the PoV controller has changed</returns>
      public bool HasChanged(int[] previous, int[] current) {
        return previous[this.povIndex] != current[this.povIndex];
      }

      /// <summary>Index of the PoV controller that will be read</summary>
      private int povIndex;

    }

    #endregion // class PovReader

  }
} // namespace Nuclex.Input.Devices

#endif // !NO_DIRECTINPUT
