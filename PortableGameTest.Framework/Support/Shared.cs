#region CPL License
/*
Nuclex Framework
Copyright (C) 2002-2013 Nuclex Development Labs

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
using System.Diagnostics;

namespace Nuclex.Support {

  /// <summary>Manages a globally shared instance of the given Type</summary>
  /// <typeparam name="TShared">
  ///   Type of which a globally shared instance will be provided
  /// </typeparam>
  public static class Shared<TShared> where TShared : new() {

    /// <summary>Returns the global instance of the class</summary>
    public static TShared Instance {
      [DebuggerStepThrough]
      get {
        return instance;
      }
    }

    /// <summary>Stored the globally shared instance</summary>
    private static readonly TShared instance = new TShared();

  }

} // namespace Nuclex.Support
