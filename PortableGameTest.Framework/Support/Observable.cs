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
using System.ComponentModel;
using System.Diagnostics;
#if !NO_LINQ_EXPRESSIONS
using System.Linq.Expressions;
#endif
using System.Reflection;

namespace Nuclex.Support {

  /// <summary>Base class for objects that support property change notifications</summary>
#if !NO_SERIALIZATION
  [Serializable]
#endif
  public abstract class Observable : INotifyPropertyChanged {

    /// <summary>Raised when a property of the instance has changed its value</summary>
#if !NO_SERIALIZATION
    [field: NonSerialized]
#endif
    public event PropertyChangedEventHandler PropertyChanged;

#if !NO_LINQ_EXPRESSIONS
    /// <summary>Triggers the PropertyChanged event for the specified property</summary>
    /// <param name="property">
    ///   Lambda expression for the property that will be reported to have changed
    /// </param>
    /// <remarks>
    ///   <para>
    ///     This notification should be fired post-change, i.e. when the property has
    ///     already changed its value.
    ///   </para>
    ///   <example>
    ///     <code>
    ///       public int Limit {
    ///         get { return this.limit; }
    ///         set {
    ///           if(value != this.limit) {
    ///             this.limit = value;
    ///             OnPropertyChanged(() => Limit);
    ///           }
    ///         }
    ///       }
    ///     </code>
    ///   </example>
    /// </remarks>
    protected void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> property) {
      PropertyChangedEventHandler copy = PropertyChanged;
      if(copy != null) {
        copy(this, PropertyChangedEventArgsHelper.GetArgumentsFor(property));
      }
    }
#endif // !NO_LINQ_EXPRESSIONS

    /// <summary>Triggers the PropertyChanged event for the specified property</summary>
    /// <param name="propertyName">Name of the property that has changed its value</param>
    /// <remarks>
    ///   <para>
    ///     This notification should be fired post-change, i.e. when the property has
    ///     already changed its value. If possible, use the other overload of this
    ///     method to ensure the property name will be updated during F2 refactoring.
    ///   </para>
    ///   <example>
    ///     <code>
    ///       public int Limit {
    ///         get { return this.limit; }
    ///         set {
    ///           if(value != this.limit) {
    ///             this.limit = value;
    ///             OnPropertyChanged("Limit"); // Note: prefer lambda exp whenever possible
    ///           }
    ///         }
    ///       }
    ///     </code>
    ///   </example>
    /// </remarks>
    protected virtual void OnPropertyChanged(string propertyName) {
      enforceChangedPropertyExists(propertyName);

      PropertyChangedEventHandler copy = PropertyChanged;
      if(copy != null) {
        copy(this, PropertyChangedEventArgsHelper.GetArgumentsFor(propertyName));
      }
    }

    /// <summary>Ensures that a property with the specified name exists in the type</summary>
    /// <param name="propertyName">Property name that will be checked</param>
    [Conditional("DEBUG")]
    private void enforceChangedPropertyExists(string propertyName) {

      // An empty string or null indicates that all properties have changed
      if(string.IsNullOrEmpty(propertyName)) {
        return;
      }

      // Any other string needs to match a property name
        PropertyInfo property = GetType().GetTypeInfo().GetDeclaredProperty(propertyName);
      if(property == null) {
        throw new ArgumentException(
          string.Format(
            "Type '{0}' tried to raise a change notification for property '{1}', " +
            "but no such property exists!",
            GetType().Name, propertyName
          ),
          "propertyName"
        );
      }

    }

  }

} // namespace Nuclex.Support
