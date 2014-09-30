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

#if !NO_SPECIALIZED_COLLECTIONS

#endif
using System.Collections.Specialized;

namespace Nuclectic.Support.Collections {

  /// <summary>Contains fixed constants used by some collections</summary>
  public static class Constants {

#if !NO_SPECIALIZED_COLLECTIONS
    /// <summary>Fixed event args used to notify that the collection has reset</summary>
    public static readonly NotifyCollectionChangedEventArgs NotifyCollectionResetEventArgs =
      new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
#endif

  }

} // namespace Nuclex.Support.Collections
