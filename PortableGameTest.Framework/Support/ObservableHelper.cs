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
#if !NO_LINQ_EXPRESSIONS
using System.Linq.Expressions;
#endif

namespace Nuclex.Support {

  /// <summary>Contains helper methods for observing property changed</summary>
  public static class ObservableHelper {

#if !NO_LINQ_EXPRESSIONS
    /// <summary>Obtains the name of a property from a lambda expression</summary>
    /// <param name="property">
    ///   Lambda expression for the property whose name will be returned
    /// </param>
    /// <returns>The name of the property contained in the lamba expression</returns>
    /// <remarks>
    ///   <para>
    ///     This method obtains the textual name of a property specified in a lambda
    ///     expression. By going through a lambda expression, the property will be
    ///     stated as actual code, allowing F2 refactoring to correctly update any
    ///     references to the property when it is renamed.
    ///   </para>
    ///   <example>
    ///     <code>
    ///       string propertyName = ObservableHelper.GetPropertyName(() => SomeValue);
    ///       Assert.AreEqual("SomeValue", propertyName);
    ///     </code>
    ///   </example>
    /// </remarks>
    public static string GetPropertyName<TValue>(Expression<Func<TValue>> property) {
      var lambda = (LambdaExpression)property;

      MemberExpression memberExpression;
      {
        var unaryExpression = lambda.Body as UnaryExpression;
        if(unaryExpression != null) {
          memberExpression = (MemberExpression)unaryExpression.Operand;
        } else {
          memberExpression = (MemberExpression)lambda.Body;
        }
      }

      return memberExpression.Member.Name;
    }
#endif // !NO_LINQ_EXPRESSIONS

  }

} // namespace Nuclex.Support
