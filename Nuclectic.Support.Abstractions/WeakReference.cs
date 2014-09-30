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
using System.Runtime.Serialization;

namespace Nuclectic.Support {

#if !WINDOWS_PHONE

  /// <summary>
  ///   Type-safe weak reference, referencing an object while still allowing
  ///   that object to be garbage collected.
  /// </summary>
  [DataContract]
  public class WeakReference<ReferencedType> : WeakReference
    where ReferencedType : class {

    /// <summary>
    ///   Initializes a new instance of the WeakReference class, referencing
    ///   the specified object.
    /// </summary>
    /// <param name="target">The object to track or null.</param>
    public WeakReference(ReferencedType target) :
      base(target) { }

    /// <summary>
    ///   Initializes a new instance of the WeakReference class, referencing
    ///   the specified object optionally using resurrection tracking.
    /// </summary>
    /// <param name="target">An object to track.</param>
    /// <param name="trackResurrection">
    ///   Indicates when to stop tracking the object. If true, the object is tracked
    ///   after finalization; if false, the object is only tracked until finalization.
    /// </param>
    public WeakReference(ReferencedType target, bool trackResurrection) :
      base(target, trackResurrection) { }

    /// <summary>
    ///   Gets or sets the object (the target) referenced by the current WeakReference
    ///   object.
    /// </summary>
    /// <remarks>
    ///   Is null if the object referenced by the current System.WeakReference object
    ///   has been garbage collected; otherwise, a reference to the object referenced
    ///   by the current System.WeakReference object.
    /// </remarks>
    /// <exception cref="System.InvalidOperationException">
    ///   The reference to the target object is invalid. This can occur if the current
    ///   System.WeakReference object has been finalized
    /// </exception>
    [DataMember]
    public new ReferencedType Target {
      get { return (base.Target as ReferencedType); }
      set { base.Target = value; }
    }

  }

#endif // !WINDOWS_PHONE

} // namespace Nuclex.Support
