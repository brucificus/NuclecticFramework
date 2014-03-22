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
using System.Collections.Generic;

namespace Nuclex.Support.Collections {

  /// <summary>
  ///   Argument container used by collections to notify about replaced items
  /// </summary>
  public class ItemReplaceEventArgs<TItem> : EventArgs {

    /// <summary>Initializes a new event arguments supplier</summary>
    /// <param name="oldItem">Item that has been replaced by another item</param>
    /// <param name="newItem">Replacement item that is now part of the collection</param>
    public ItemReplaceEventArgs(TItem oldItem, TItem newItem) {
      this.oldItem = oldItem;
      this.newItem = newItem;
    }

    /// <summary>Item that has been replaced by another item</summary>
    public TItem OldItem {
      get { return this.oldItem; }
    }

    /// <summary>Replacement item that is now part of the collection</summary>
    public TItem NewItem {
      get { return this.newItem; }
    }

    /// <summary>Item that was removed from the collection</summary>
    private TItem oldItem;
    /// <summary>Item that was added to the collection</summary>
    private TItem newItem;

  }

} // namespace Nuclex.Support.Collections
