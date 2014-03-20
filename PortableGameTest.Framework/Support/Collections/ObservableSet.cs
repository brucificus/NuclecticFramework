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
using System.Linq;
using System.Collections;

#if !NO_SPECIALIZED_COLLECTIONS
using System.Collections.Specialized;
#endif

#if !(WINDOWS_PHONE || XBOX360)

namespace Nuclex.Support.Collections {

  /// <summary>Set which fires events when items are removed or added to it</summary>
  /// <typeparam name="TItem">Type of items to manage in the set</typeparam>
  public class ObservableSet<TItem> :
    ISet<TItem>,
    ICollection<TItem>,
#if !NO_SPECIALIZED_COLLECTIONS
    INotifyCollectionChanged,
#endif
    IObservableCollection<TItem> {

    /// <summary>Raised when an item has been added to the collection</summary>
    public event EventHandler<ItemEventArgs<TItem>> ItemAdded;
    /// <summary>Raised when an item is removed from the collection</summary>
    public event EventHandler<ItemEventArgs<TItem>> ItemRemoved;
    /// <summary>Raised when an item is replaced in the collection</summary>
    public event EventHandler<ItemReplaceEventArgs<TItem>> ItemReplaced {
      add { }
      remove { }
    }
    /// <summary>Raised when the collection is about to be cleared</summary>
    /// <remarks>
    ///   This could be covered by calling ItemRemoved for each item currently
    ///   contained in the collection, but it is often simpler and more efficient
    ///   to process the clearing of the entire collection as a special operation.
    /// </remarks>
    public event EventHandler Clearing;
    /// <summary>Raised when the collection has been cleared</summary>
    public event EventHandler Cleared;

#if !NO_SPECIALIZED_COLLECTIONS
    /// <summary>Called when the collection has changed</summary>
    public event NotifyCollectionChangedEventHandler CollectionChanged;
#endif

    /// <summary>Initializes a new observable set based on a hashed set</summary>
    public ObservableSet() : this(new HashSet<TItem>()) { }

    /// <summary>
    ///   Initializes a new observable set forwarding operations to the specified set
    /// </summary>
    /// <param name="set">Set operations will be forwarded to</param>
    public ObservableSet(ISet<TItem> set) {
      this.set = set;
    }

    /// <summary>Adds an item to the set</summary>
    /// <param name="item">Item that will be added to the set</param>
    /// <returns>
    ///   True if the element was added, false if it was already contained in the set
    /// </returns>
    public bool Add(TItem item) {
      bool wasAdded = this.set.Add(item);
      if(wasAdded) {
        OnAdded(item);
      }
      return wasAdded;
    }

    /// <summary>Removes all elements that are contained in the collection</summary>
    /// <param name="other">Collection whose elements will be removed from this set</param>
    public void ExceptWith(IEnumerable<TItem> other) {
      if(other == this) {
        Clear();
        return;
      }

      foreach(TItem item in other) {
        if(this.set.Remove(item)) {
          OnRemoved(item);
        }
      }
    }

    /// <summary>
    ///   Only keeps those elements in this set that are contained in the collection
    /// </summary>
    /// <param name="other">Other set this set will be filtered by</param>
    public void IntersectWith(IEnumerable<TItem> other) {
      var otherSet = other as ISet<TItem>;
      if(otherSet == null) {
        otherSet = new HashSet<TItem>(other);
      }

      var itemsToRemove = new List<TItem>();
      foreach(TItem item in this.set) {
        if(!otherSet.Contains(item)) {
          itemsToRemove.Add(item);
        }
      }

      for(int index = 0; index < itemsToRemove.Count; ++index) {
        this.set.Remove(itemsToRemove[index]);
        OnRemoved(itemsToRemove[index]);
      }
    }

    /// <summary>
    ///   Determines whether the current set is a proper (strict) subset of a collection
    /// </summary>
    /// <param name="other">Collection against which the set will be tested</param>
    /// <returns>True if the set is a proper subset of the specified collection</returns>
    public bool IsProperSubsetOf(IEnumerable<TItem> other) {
      return this.set.IsProperSubsetOf(other);
    }

    /// <summary>
    ///   Determines whether the current set is a proper (strict) superset of a collection
    /// </summary>
    /// <param name="other">Collection against which the set will be tested</param>
    /// <returns>True if the set is a proper superset of the specified collection</returns>
    public bool IsProperSupersetOf(IEnumerable<TItem> other) {
      return this.set.IsProperSupersetOf(other);
    }

    /// <summary>Determines whether the current set is a subset of a collection</summary>
    /// <param name="other">Collection against which the set will be tested</param>
    /// <returns>True if the set is a subset of the specified collection</returns>
    public bool IsSubsetOf(IEnumerable<TItem> other) {
      return this.set.IsSubsetOf(other);
    }

    /// <summary>Determines whether the current set is a superset of a collection</summary>
    /// <param name="other">Collection against which the set will be tested</param>
    /// <returns>True if the set is a superset of the specified collection</returns>
    public bool IsSupersetOf(IEnumerable<TItem> other) {
      return this.set.IsSupersetOf(other);
    }

    /// <summary>
    ///   Determines if the set shares at least one common element with the collection
    /// </summary>
    /// <param name="other">Collection the set will be tested against</param>
    /// <returns>
    ///   True if the set shares at least one common element with the collection
    /// </returns>
    public bool Overlaps(IEnumerable<TItem> other) {
      return this.set.Overlaps(other);
    }

    /// <summary>
    ///   Determines whether the set contains the same elements as the specified collection
    /// </summary>
    /// <param name="other">Collection the set will be tested against</param>
    /// <returns>True if the set contains the same elements as the collection</returns>
    public bool SetEquals(IEnumerable<TItem> other) {
      return this.set.SetEquals(other);
    }

    /// <summary>
    ///   Modifies the current set so that it contains only elements that are present either
    ///   in the current set or in the specified collection, but not both
    /// </summary>
    /// <param name="other">Collection the set will be excepted with</param>
    public void SymmetricExceptWith(IEnumerable<TItem> other) {
      foreach(TItem item in other) {
        if(this.set.Remove(item)) {
          OnRemoved(item);
        } else {
          this.Add(item);
          OnAdded(item);
        }
      }
    }

    /// <summary>
    ///   Modifies the current set so that it contains all elements that are present in both
    ///   the current set and in the specified collection
    /// </summary>
    /// <param name="other">Collection an union will be built with</param>
    public void UnionWith(IEnumerable<TItem> other) {
      foreach(TItem item in other) {
        if(this.set.Add(item)) {
          OnAdded(item);
        }
      }
    }

    /// <summary>Removes all items from the set</summary>
    public void Clear() {
      OnClearing();
      this.set.Clear();
      OnCleared();
    }

    /// <summary>Determines whether the set contains the specified item</summary>
    /// <param name="item">Item the set will be tested for</param>
    /// <returns>True if the set contains the specified item</returns>
    public bool Contains(TItem item) {
      return this.set.Contains(item);
    }

    /// <summary>Copies the contents of the set into an array</summary>
    /// <param name="array">Array the set's contents will be copied to</param>
    /// <param name="arrayIndex">
    ///   Index in the array the first copied element will be written to
    /// </param>
    public void CopyTo(TItem[] array, int arrayIndex) {
      this.set.CopyTo(array, arrayIndex);
    }

    /// <summary>Counts the number of items contained in the set</summary>
    public int Count {
      get { return this.set.Count; }
    }

    /// <summary>Determines whether the set is readonly</summary>
    public bool IsReadOnly {
      get { return this.set.IsReadOnly; }
    }

    /// <summary>Removes an item from the set</summary>
    /// <param name="item">Item that will be removed from the set</param>
    /// <returns>
    ///   True if the item was contained in the set and is now removed
    /// </returns>
    public bool Remove(TItem item) {
      bool wasRemoved = this.set.Remove(item);
      if(wasRemoved) {
        OnRemoved(item);
      }
      return wasRemoved;
    }

    /// <summary>Creates an enumerator for the set's contents</summary>
    /// <returns>A new enumerator for the sets contents</returns>
    public IEnumerator<TItem> GetEnumerator() {
      return this.set.GetEnumerator();
    }

    /// <summary>Fires the 'ItemAdded' event</summary>
    /// <param name="item">Item that has been added to the collection</param>
    protected virtual void OnAdded(TItem item) {
      if(ItemAdded != null) {
        ItemAdded(this, new ItemEventArgs<TItem>(item));
      }
#if !NO_SPECIALIZED_COLLECTIONS
      if(CollectionChanged != null) {
        CollectionChanged(
          this,
          new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item)
        );
      }
#endif
    }

    /// <summary>Fires the 'ItemRemoved' event</summary>
    /// <param name="item">Item that has been removed from the collection</param>
    protected virtual void OnRemoved(TItem item) {
      if(ItemRemoved != null) {
        ItemRemoved(this, new ItemEventArgs<TItem>(item));
      }
#if !NO_SPECIALIZED_COLLECTIONS
      if(CollectionChanged != null) {
        CollectionChanged(
          this,
          new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item)
        );
      }
#endif
    }

    /// <summary>Fires the 'Clearing' event</summary>
    protected virtual void OnClearing() {
      if(Clearing != null) {
        Clearing(this, EventArgs.Empty);
      }
    }

    /// <summary>Fires the 'Cleared' event</summary>
    protected virtual void OnCleared() {
      if(Cleared != null) {
        Cleared(this, EventArgs.Empty);
      }
#if !NO_SPECIALIZED_COLLECTIONS
      if(CollectionChanged != null) {
        CollectionChanged(this, Constants.NotifyCollectionResetEventArgs);
      }
#endif
    }

    #region ICollection<T> implementation

    /// <summary>Adds an item to the set</summary>
    /// <param name="item">Item that will be added to the set</param>
    void ICollection<TItem>.Add(TItem item) {
      this.set.Add(item);
    }

    #endregion // ICollection<T> implementation

    #region IEnumerable implementation

    /// <summary>Creates an enumerator for the set's contents</summary>
    /// <returns>A new enumerator for the sets contents</returns>
    IEnumerator IEnumerable.GetEnumerator() {
      return this.set.GetEnumerator();
    }

    #endregion // IEnumerable implementation

    /// <summary>The set being wrapped</summary>
    private ISet<TItem> set;

  }

} // namespace Nuclex.Support.Collections

#endif // !(WINDOWS_PHONE || XBOX360)
