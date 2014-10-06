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
using System.Reflection;

namespace Nuclex.Support {

  /// <summary>Helper methods for the reflection Type class</summary>
  public static class TypeHelper {

    #region class FieldInfoComparer

    /// <summary>Determines whether member informations relate to the same member</summary>
    private class FieldInfoComparer : IEqualityComparer<FieldInfo> {

      /// <summary>Default instance of the comparer</summary>
      public static readonly FieldInfoComparer Default = new FieldInfoComparer();

      /// <summary>Checks whether two member informations are equal</summary>
      /// <param name="left">Informations about the left member in the comaprison</param>
      /// <param name="right">Informations about the right member in the comparison</param>
      /// <returns>True if the two member informations relate to the same member</returns>
      public bool Equals(FieldInfo left, FieldInfo right) {
        return
          (left.DeclaringType == right.DeclaringType) &&
          (left.Name == right.Name);
      }

      /// <summary>Determines the hash code of the specified member informations</summary>
      /// <param name="FieldInfo">
      ///   Member informations whose hash code will be determined
      /// </param>
      /// <returns>The hash code of the specified member informations</returns>
      public int GetHashCode(FieldInfo FieldInfo) {
        return (FieldInfo.DeclaringType.GetHashCode() ^ FieldInfo.Name.GetHashCode());
      }

    }

    #endregion // class MemberInfoComparer

#if WINRT

    /// <summary>
    ///   Returns all the fields of a type, including those defined in the type's base classes
    /// </summary>
    /// <param name="type">Type whose fields will be returned</param>
    /// <param name="bindingFlags">Binding flags to use when querying the fields</param>
    /// <returns>All of the type's fields, including its base types</returns>
    public static FieldInfo[] GetFieldInfosIncludingBaseClasses(this Type type) {
      var fieldInfoSet = new HashSet<FieldInfo>(fieldInfos, FieldInfoComparer.Default);

      while(type != typeof(object)) {
        TypeInfo typeInfo = type.GetTypeInfo();

        foreach(FieldInfo fieldInfo in typeInfo.DeclaredFields) {
          fieldInfoSet.Add(fieldInfo);
        }

        type = typeInfo.BaseType;
      }

      FieldInfo[] fieldInfos = new FieldInfo[fieldInfoSet.Count];
      fieldInfoSet.CopyTo(fieldInfos, 0);
      return fieldInfos;
    }

#elif !(XBOX360 || WINDOWS_PHONE)

    /// <summary>
    ///   Returns all the fields of a type, including those defined in the type's base classes
    /// </summary>
    /// <param name="type">Type whose fields will be returned</param>
    /// <param name="bindingFlags">Binding flags to use when querying the fields</param>
    /// <returns>All of the type's fields, including its base types</returns>
    public static FieldInfo[] GetFieldInfosIncludingBaseClasses(
      this Type type, BindingFlags bindingFlags
    ) {
      FieldInfo[] fieldInfos = type.GetFields(bindingFlags);

      // If this class doesn't have a base, don't waste any time
      if(type.BaseType != typeof(object)) {
        var fieldInfoSet = new HashSet<FieldInfo>(fieldInfos, FieldInfoComparer.Default);
        while(type.BaseType != typeof(object)) {
          type = type.BaseType;
          fieldInfos = type.GetFields(bindingFlags);
          for(int index = 0; index < fieldInfos.Length; ++index) {
            fieldInfoSet.Add(fieldInfos[index]);
          }
        }

        fieldInfos = new FieldInfo[fieldInfoSet.Count];
        fieldInfoSet.CopyTo(fieldInfos);
      }

      return fieldInfos;
    }

#else // !(XBOX360 || WINDOWS_PHONE)

    /// <summary>
    ///   Returns all the fields of a type, including those defined in the type's base classes
    /// </summary>
    /// <param name="type">Type whose fields will be returned</param>
    /// <param name="bindingFlags">Binding flags to use when querying the fields</param>
    /// <returns>All of the type's fields, including its base types</returns>
    public static FieldInfo[] GetFieldInfosIncludingBaseClasses(
      this Type type, BindingFlags bindingFlags
    ) {
      FieldInfo[] fieldInfos = type.GetFields(bindingFlags);

      // If this class doesn't have a base, don't waste any time
      if(type.BaseType != typeof(object)) {
        var fieldInfoSet = new Dictionary<FieldInfo, object>(FieldInfoComparer.Default);
        for(int index = 0; index < fieldInfos.Length; ++index) {
          fieldInfoSet.Add(fieldInfos[index], null);
        }

        while(type.BaseType != typeof(object)) {
          type = type.BaseType;
          fieldInfos = type.GetFields(bindingFlags);

          for(int index = 0; index < fieldInfos.Length; ++index) {
            addIfNotExists(fieldInfoSet, fieldInfos[index]);
          }
        }

        fieldInfos = new FieldInfo[fieldInfoSet.Count];
        fieldInfoSet.Keys.CopyTo(fieldInfos, 0);
      }

      return fieldInfos;
    }

    /// <summary>
    ///   Adds field informations to a list if they're not already contained in it
    /// </summary>
    /// <param name="fieldInfos">List the field informations will be added to</param>
    /// <param name="fieldInfo">Field informations that will be added to the list</param>
    private static void addIfNotExists(
      IDictionary<FieldInfo, object> fieldInfos, FieldInfo fieldInfo
    ) {
      if(!fieldInfos.ContainsKey(fieldInfo)) {
        fieldInfos.Add(fieldInfo, null);
      }
    }

#endif // !(XBOX360 || WINDOWS_PHONE)

#if WINRT

    /// <summary>Determines whether the given type has a default constructor</summary>
    /// <param name="type">Type which is to be checked</param>
    /// <returns>True if the type has a default constructor</returns>
    public static bool HasDefaultConstructor(this Type type) {
      foreach(ConstructorInfo constructorInfo in type.GetTypeInfo().DeclaredConstructors) {
        if(constructorInfo.IsPublic && (constructorInfo.GetParameters().Length == 0)) {
          return true;
        }
      }

      return false;
    }

#else

    /// <summary>Determines whether the given type has a default constructor</summary>
    /// <param name="type">Type which is to be checked</param>
    /// <returns>True if the type has a default constructor</returns>
    public static bool HasDefaultConstructor(this Type type) {
      ConstructorInfo[] constructors = type.GetConstructors();

      for(int index = 0; index < constructors.Length; ++index) {
        ConstructorInfo constructor = constructors[index];
        if(constructor.IsPublic && (constructor.GetParameters().Length == 0)) {
          return true;
        }
      }

      return false;
    }

#endif

    /// <summary>Determines whether the type has the specified attribute</summary>
    /// <typeparam name="TAttribute">Attribute the type will be checked for</typeparam>
    /// <param name="type">
    ///   Type that will be checked for presence of the specified attribute
    /// </param>
    /// <returns>True if the type has the specified attribute, otherwise false</returns>
    public static bool HasAttribute<TAttribute>(this Type type) {
      return type.HasAttribute(typeof(TAttribute));
    }

#if WINRT

    /// <summary>Determines whether the type has the specified attribute</summary>
    /// <param name="type">
    ///   Type that will be checked for presence of the specified attribute
    /// </param>
    /// <param name="attributeType">Attribute the type will be checked for</typeparam>
    /// <returns>True if the type has the specified attribute, otherwise false</returns>
    public static bool HasAttribute(this Type type, Type attributeType) {
      return (type.GetTypeInfo().GetCustomAttribute(attributeType) != null);
    }

#else

    /// <summary>Determines whether the type has the specified attribute</summary>
    /// <param name="type">
    ///   Type that will be checked for presence of the specified attribute
    /// </param>
    /// <param name="attributeType">Attribute the type will be checked for</param>
    /// <returns>True if the type has the specified attribute, otherwise false</returns>
    public static bool HasAttribute(this Type type, Type attributeType) {
      object[] attributes = type.GetCustomAttributes(attributeType, true);
      return (attributes != null) && (attributes.Length > 0);
    }

#endif

  }

} // namespace Nuclex.Support
