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

namespace Nuclectic.Support.Collections {

  /// <summary>
  ///   Argument container used by collections to notify about changed items
  /// </summary>
  public class ItemEventArgs<TItem> : EventArgs {

    /// <summary>Initializes a new event arguments supplier</summary>
    /// <param name="item">Item to be supplied to the event handler</param>
    public ItemEventArgs(TItem item) {
      this.item = item;
    }

    /// <summary>Obtains the collection item the event arguments are carrying</summary>
    public TItem Item {
      get { return this.item; }
    }

    /// <summary>Item to be passed to the event handler</summary>
    private TItem item;

  }

} // namespace Nuclex.Support.Collections
