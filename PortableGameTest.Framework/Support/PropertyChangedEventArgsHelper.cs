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
#if NO_CONCURRENT_COLLECTIONS
using System.Collections.Generic;
#else
using System.Collections.Concurrent;
#endif
using System.ComponentModel;
#if !NO_LINQ_EXPRESSIONS
using System.Linq.Expressions;
#endif

namespace Nuclex.Support {

  /// <summary>Contains helper methods for property change notifications</summary>
  public static class PropertyChangedEventArgsHelper {

    /// <summary>
    ///   A property change event argument container that indicates that all
    ///   properties have changed their value.
    /// </summary>
    public static readonly PropertyChangedEventArgs Wildcard =
      new PropertyChangedEventArgs(null);

    /// <summary>Initializes a new property changed argument helper</summary>
    static PropertyChangedEventArgsHelper() {
#if NO_CONCURRENT_COLLECTIONS
      cache = new Dictionary<string, PropertyChangedEventArgs>();
#else
      cache = new ConcurrentDictionary<string, PropertyChangedEventArgs>();
#endif
    }

#if !NO_LINQ_EXPRESSIONS
    /// <summary>
    ///   Provides a property change argument container for the specified property
    /// </summary>
    /// <param name="property">
    ///   Property for which an event argument container will be provided
    /// </param>
    /// <returns>The event argument container for a property of the specified name</returns>
    /// <remarks>
    ///   <para>
    ///     This method transparently caches instances of the argument containers
    ///     to avoid feeding the garbage collector. A typical application only has
    ///     in the order of tens to hundreds of different properties for which changes
    ///     will be reported, making a cache to avoid garbage collections viable.
    ///   </para>
    ///   <example>
    ///     <code>
    ///       PropertyChangedEventArgs arguments =
    ///         PropertyChangedEventArgsHelper.GetArgumentsFor(() => SomeProperty);
    ///     </code>
    ///   </example>
    /// </remarks>
    public static PropertyChangedEventArgs GetArgumentsFor<TValue>(
      Expression<Func<TValue>> property
    ) {
      return GetArgumentsFor(ObservableHelper.GetPropertyName(property));
    }
#endif

    /// <summary>
    ///   Provides a property change argument container for the specified property
    /// </summary>
    /// <param name="propertyName">
    ///   Property for which an event argument container will be provided
    /// </param>
    /// <returns>The event argument container for a property of the specified name</returns>
    /// <remarks>
    ///   <para>
    ///     This method transparently caches instances of the argument containers
    ///     to avoid feeding the garbage collector. A typical application only has
    ///     in the order of tens to hundreds of different properties for which changes
    ///     will be reported, making a cache to avoid garbage collections viable.
    ///   </para>
    ///   <example>
    ///     <code>
    ///       PropertyChangedEventArgs arguments =
    ///         PropertyChangedEventArgsHelper.GetArgumentsFor("SomeProperty");
    ///     </code>
    ///   </example>
    /// </remarks>
    public static PropertyChangedEventArgs GetArgumentsFor(string propertyName) {
      if(string.IsNullOrEmpty(propertyName)) {
        return Wildcard;
      }

#if NO_CONCURRENT_COLLECTIONS
      lock(cache) {
        // Try to reuse the change notification if an instance already exists
        PropertyChangedEventArgs arguments;
        if(!cache.TryGetValue(propertyName, out arguments)) {
          arguments = new PropertyChangedEventArgs(propertyName);
          cache.Add(propertyName, arguments);
        }

        return arguments;
      }
#else
      // If an instance for this property already exists, just return it
      PropertyChangedEventArgs arguments;
      if(cache.TryGetValue(propertyName, out arguments)) {
        return arguments;
      }

      // No instance existed (at least a short moment ago), so create a new one
      return cache.GetOrAdd(propertyName, new PropertyChangedEventArgs(propertyName));
#endif
    }

#if !NO_LINQ_EXPRESSIONS
    /// <summary>
    ///   Determines whether the property change affects the specified property
    /// </summary>
    /// <typeparam name="TValue">
    ///   Type of the property that will be tested for being affected
    /// </typeparam>
    /// <param name="arguments">
    ///   Property change that has been reported by the observed object
    /// </param>
    /// <param name="property">Property that will be tested for being affected</param>
    /// <returns>Whether the specified property is affected by the property change</returns>
    /// <remarks>
    ///   <para>
    ///     By using this method, you can shorten the code needed to test whether
    ///     a property change notification affects a specific property. You also
    ///     avoid hardcoding the property name, which would have the adverse effect
    ///     of not updating the textual property names during F2 refactoring.
    ///   </para>
    ///   <example>
    ///     <code>
    ///       private void propertyChanged(object sender, PropertyChangedEventArgs arguments) {
    ///         if(arguments.AreAffecting(() => ViewModel.DisplayedValue)) {
    ///           updateDisplayedValueFromViewModel();
    ///         } // Do not use else if here or wildcards will not work
    ///         if(arguments.AreAffecting(() => ViewModel.OtherValue)) {
    ///           updateOtherValueFromViewModel();
    ///         }
    ///       }
    ///     </code>
    ///   </example>
    /// </remarks>
    public static bool AreAffecting<TValue>(
      this PropertyChangedEventArgs arguments, Expression<Func<TValue>> property
    ) {
      if(arguments.AffectAllProperties()) {
        return true;
      }

      string propertyName = ObservableHelper.GetPropertyName(property);
      return (arguments.PropertyName == propertyName);
    }
#endif

    /// <summary>
    ///   Determines whether the property change affects the specified property
    /// </summary>
    /// <param name="arguments">
    ///   Property change that has been reported by the observed object
    /// </param>
    /// <param name="propertyName">Property that will be tested for being affected</param>
    /// <returns>Whether the specified property is affected by the property change</returns>
    /// <remarks>
    ///   <para>
    ///     By using this method, you can shorten the code needed to test whether
    ///     a property change notification affects a specific property.
    ///   </para>
    ///   <example>
    ///     <code>
    ///       private void propertyChanged(object sender, PropertyChangedEventArgs arguments) {
    ///         if(arguments.AreAffecting("DisplayedValue")) {
    ///           updateDisplayedValueFromViewModel();
    ///         } // Do not use else if here or wildcards will not work
    ///         if(arguments.AreAffecting("OtherValue")) {
    ///           updateOtherValueFromViewModel();
    ///         }
    ///       }
    ///     </code>
    ///   </example>
    /// </remarks>
    public static bool AreAffecting(
      this PropertyChangedEventArgs arguments, string propertyName
    ) {
      if(arguments.AffectAllProperties()) {
        return true;
      }

      return (arguments.PropertyName == propertyName);
    }

    /// <summary>Determines whether a property change notification is a wildcard</summary>
    /// <param name="arguments">
    ///		Property change notification that will be checked on being a wildcard
    /// </param>
    /// <returns>
    ///   Whether the property change is a wildcard, indicating that all properties
    ///   have changed.
    /// </returns>
    /// <remarks>
    ///   <para>
    ///     As stated on MSDN: "The PropertyChanged event can indicate all properties
    ///     on the object have changed by using either Nothing or String.Empty as
    ///     the property name in the PropertyChangedEventArgs."
    ///   </para>
    ///   <para>
    ///     This method offers an expressive way of checking for that eventuality.
    ///   </para>
    ///   <example>
    ///     <code>
    ///       private void propertyChanged(object sender, PropertyChangedEventArgs arguments) {
    ///         if(arguments.AffectAllProperties()) {
    ///           // Do something
    ///         }
    ///       }
    ///     </code>
    ///   </example>
    /// </remarks>
    public static bool AffectAllProperties(this PropertyChangedEventArgs arguments) {
      return string.IsNullOrEmpty(arguments.PropertyName);
    }

    /// <summary>
    ///   Caches PropertyChangedEventArgs instances to avoid feeding the garbage collector
    /// </summary>
#if NO_CONCURRENT_COLLECTIONS
    private static readonly Dictionary<string, PropertyChangedEventArgs> cache;
#else
    private static readonly ConcurrentDictionary<string, PropertyChangedEventArgs> cache;
#endif

  }

} // namespace Nuclex.Support
