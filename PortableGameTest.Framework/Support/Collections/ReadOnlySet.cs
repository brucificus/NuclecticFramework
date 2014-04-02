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
using System.Collections;
using System.Collections.Generic;

namespace Nuclex.Support.Collections {

  /// <summary>Wraps a set and prevents it from being modified</summary>
  /// <typeparam name="TItem">Type of items to manage in the set</typeparam>
  public class ReadOnlySet<TItem> : ISet<TItem>, ICollection<TItem> {

    /// <summary>
    ///   Initializes a new observable set forwarding operations to the specified set
    /// </summary>
    /// <param name="set">Set operations will be forwarded to</param>
    public ReadOnlySet(ISet<TItem> set) {
      this.set = set;
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
      get { return true; }
    }


    /// <summary>Creates an enumerator for the set's contents</summary>
    /// <returns>A new enumerator for the sets contents</returns>
    public IEnumerator<TItem> GetEnumerator() {
      return this.set.GetEnumerator();
    }

    /// <summary>Creates an enumerator for the set's contents</summary>
    /// <returns>A new enumerator for the sets contents</returns>
    IEnumerator IEnumerable.GetEnumerator() {
      return this.set.GetEnumerator();
    }

    /// <summary>
    ///   Modifies the current set so that it contains only elements that are present either
    ///   in the current set or in the specified collection, but not both
    /// </summary>
    /// <param name="other">Collection the set will be excepted with</param>
    void ISet<TItem>.SymmetricExceptWith(IEnumerable<TItem> other) {
      throw new NotSupportedException(
        "Excepting is not supported by the read-only set"
      );
    }

    /// <summary>
    ///   Modifies the current set so that it contains all elements that are present in both
    ///   the current set and in the specified collection
    /// </summary>
    /// <param name="other">Collection an union will be built with</param>
    void ISet<TItem>.UnionWith(IEnumerable<TItem> other) {
      throw new NotSupportedException(
        "Unioning is not supported by the read-only set"
      );
    }

    /// <summary>Removes all items from the set</summary>
    public void Clear() {
      throw new NotSupportedException(
        "Clearing is not supported by the read-only set"
      );
    }

    /// <summary>Removes an item from the set</summary>
    /// <param name="item">Item that will be removed from the set</param>
    /// <returns>
    ///   True if the item was contained in the set and is now removed
    /// </returns>
    bool ICollection<TItem>.Remove(TItem item) {
      throw new NotSupportedException(
        "Removing items is not supported by the read-only set"
      );
    }

    /// <summary>Adds an item to the set</summary>
    /// <param name="item">Item that will be added to the set</param>
    /// <returns>
    ///   True if the element was added, false if it was already contained in the set
    /// </returns>
    bool ISet<TItem>.Add(TItem item) {
      throw new NotSupportedException(
        "Adding items is not supported by the read-only set"
      );
    }

    /// <summary>Removes all elements that are contained in the collection</summary>
    /// <param name="other">Collection whose elements will be removed from this set</param>
    void ISet<TItem>.ExceptWith(IEnumerable<TItem> other) {
      throw new NotSupportedException(
        "Excepting items is not supported by the read-only set"
      );
    }

    /// <summary>
    ///   Only keeps those elements in this set that are contained in the collection
    /// </summary>
    /// <param name="other">Other set this set will be filtered by</param>
    void ISet<TItem>.IntersectWith(IEnumerable<TItem> other) {
      throw new NotSupportedException(
        "Intersecting items is not supported by the read-only set"
      );
    }

    /// <summary>Adds an item to the set</summary>
    /// <param name="item">Item that will be added to the set</param>
    void ICollection<TItem>.Add(TItem item) {
      throw new NotSupportedException(
        "Adding is not supported by the read-only set"
      );
    }

    /// <summary>The set being wrapped</summary>
    private ISet<TItem> set;

  }

} // namespace Nuclex.Support.Collections
