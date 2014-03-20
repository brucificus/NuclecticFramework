//#region CPL License
///*
//Nuclex Framework
//Copyright (C) 2002-2011 Nuclex Development Labs

//This library is free software; you can redistribute it and/or
//modify it under the terms of the IBM Common Public License as
//published by the IBM Corporation; either version 1.0 of the
//License, or (at your option) any later version.

//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//IBM Common Public License for more details.

//You should have received a copy of the IBM Common Public
//License along with this library
//*/
//#endregion

//using System;
//using System.Collections.Generic;
//using System.IO;

//using Microsoft.Xna.Framework;

//using Nuclex.Support;

//namespace Nuclex.Game.Serialization {

//  /// <summary>Contains utility methods for serializating objects into binary data</summary>
//  public static class BinarySerializer {

//    /// <summary>Initializes the static members of the class</summary>
//    static BinarySerializer() {
//      simpleNameToTypeDictionary = new Dictionary<string, Type>();
//      typeToSimpleNameDictionary = new Dictionary<Type, string>();

//      registerSimpleNamesForSerializableTypes();
//      registerSimpleNamesForAssetTypes();
//    }

//    /// <summary>Registers a simple name for the specified type</summary>
//    /// <typeparam name="T">Type a simple name will be registered for</typeparam>
//    /// <param name="simpleName">Simple name of the specified type</param>
//    /// <remarks>
//    ///   Using simple names can reduce the size of the serialized data (useful if
//    ///   data is being sent over network connections) and is a means to avoid
//    ///   versioning trouble if your assembly versions change, affecting the assembly
//    ///   qualified names of any types you serialize.
//    /// </remarks>
//    public static void RegisterSimpleName<T>(string simpleName) {
//      lock(typeToSimpleNameDictionary) {
//        Type type = typeof(T);

//        if(checkExistingRegistration(type, simpleName)) {
//          return;
//        }

//        typeToSimpleNameDictionary.Add(type, simpleName);
//        simpleNameToTypeDictionary.Add(simpleName, type);
//      }
//    }

//    #region Any

//    /// <summary>Loads an object from its serialization representation</summary>
//    /// <typeparam name="T">Type of value that will be loaded</typeparam>
//    /// <param name="reader">Reader to use for reading the object</param>
//    /// <param name="value">Receives the deserialized object</param>
//    public static void Load<T>(BinaryReader reader, out T value)
//      where T : IBinarySerializable {
//      Type type;
//      Load(reader, out type); // Get type

//      value = (T)Activator.CreateInstance(type);
//      value.Load(reader); // Deserialize contents
//    }

//    /// <summary>Serializes an object into a binary data stream</summary>
//    /// <typeparam name="T">Type of value that will be saved</typeparam>
//    /// <param name="writer">BinaryWriter to serialize the object into</param>
//    /// <param name="value">Object that will be serialized</param>
//    public static void Save<T>(BinaryWriter writer, T value)
//      where T : IBinarySerializable {

//      Type valueType = value.GetType();
//      if(!valueType.HasDefaultConstructor()) {
//        throw new InvalidOperationException(
//          "Attempted to serialize type '" + valueType.Name + "' without default constructor"
//        );
//      }

//      Save(writer, valueType); // Save type
//      value.Save(writer); // Serialize contents
//    }

//    #endregion // Any

//    #region System.Type

//    /// <summary>Loads a type specification from its serialization representation</summary>
//    /// <param name="reader">Reader to use for reading the type specification</param>
//    /// <param name="type">Receives the deserialized type specification</param>
//    public static void Load(BinaryReader reader, out Type type) {
//      string typeName = reader.ReadString();

//      if(simpleNameToTypeDictionary.TryGetValue(typeName, out type)) {
//        return;
//      }

//      type = Type.GetType(typeName);
//    }

//    /// <summary>Serializes a type specification into a binary data stream</summary>
//    /// <param name="writer">BinaryWriter to serialize the type specification into</param>
//    /// <param name="type">Type specification that will be serialized</param>
//    public static void Save(BinaryWriter writer, Type type) {
//      string typeName;

//      if(!typeToSimpleNameDictionary.TryGetValue(type, out typeName)) {
//        typeName = type.AssemblyQualifiedName;
//      }

//      writer.Write(typeName);
//    }

//    #endregion // System.Type

//    #region System.Collections.Generic.ICollection

//    /// <summary>Loads a collection from its serialized representation</summary>
//    /// <param name="reader">Reader to use for reading the collection</param>
//    /// <param name="collection">Collection that will receive the deserialized items</param>
//    /// <remarks>
//    ///   This method loads right into the collection and is not transactional.
//    ///   If an error occurs during loading, the collection is left in
//    ///   an intermediate state and no assumptions should be made as to its
//    ///   contents. If you need transactional safety, create a temporary collection,
//    ///   load into the temporary collection and then replace your actual
//    ///   collection with it.
//    /// </remarks>
//    public static void LoadCollection<T>(BinaryReader reader, ICollection<T> collection)
//      where T : IBinarySerializable {
//      collection.Clear();

//      int count = reader.ReadInt32();
//      for(int index = 0; index < count; ++index) {
//        T item;
//        Load(reader, out item);
//        collection.Add(item);
//      }
//    }

//    /// <summary>Serializes a collection of binary serializable objects</summary>
//    /// <param name="writer">BinaryWriter to serialize the collection into</param>
//    /// <param name="collection">Collection that will be serialized</param>
//    public static void SaveCollection<T>(BinaryWriter writer, ICollection<T> collection)
//      where T : IBinarySerializable {
//      writer.Write((int)collection.Count);
//      foreach(T item in collection) {
//        Save(writer, item);
//      }
//    }

//    #endregion // System.Collections.Generic.ICollection

//    #region Microsoft.Xna.Framework.Matrix

//    /// <summary>Loads a matrix from its serialized representation</summary>
//    /// <param name="reader">Reader to use for reading the matrix</param>
//    /// <param name="matrix">Received the deserialized matrix</param>
//    public static void Load(BinaryReader reader, out Matrix matrix) {
//      matrix = new Matrix(
//        reader.ReadSingle(), // m11
//        reader.ReadSingle(), // m12
//        reader.ReadSingle(), // m13
//        reader.ReadSingle(), // m14

//        reader.ReadSingle(), // m21
//        reader.ReadSingle(), // m22
//        reader.ReadSingle(), // m23
//        reader.ReadSingle(), // m24

//        reader.ReadSingle(), // m31
//        reader.ReadSingle(), // m32
//        reader.ReadSingle(), // m33
//        reader.ReadSingle(), // m34

//        reader.ReadSingle(), // m41
//        reader.ReadSingle(), // m42
//        reader.ReadSingle(), // m43
//        reader.ReadSingle()  // m44
//      );
//    }

//    /// <summary>Serializes a matrix into a binary data stream</summary>
//    /// <param name="writer">BinaryWriter to serialize the matrix into</param>
//    /// <param name="matrix">Matrix that will be serialized</param>
//    public static void Save(BinaryWriter writer, ref Matrix matrix) {
//      writer.Write(matrix.M11);
//      writer.Write(matrix.M12);
//      writer.Write(matrix.M13);
//      writer.Write(matrix.M14);

//      writer.Write(matrix.M21);
//      writer.Write(matrix.M22);
//      writer.Write(matrix.M23);
//      writer.Write(matrix.M24);

//      writer.Write(matrix.M31);
//      writer.Write(matrix.M32);
//      writer.Write(matrix.M33);
//      writer.Write(matrix.M34);

//      writer.Write(matrix.M41);
//      writer.Write(matrix.M42);
//      writer.Write(matrix.M43);
//      writer.Write(matrix.M44);
//    }

//    #endregion // Microsoft.Xna.Framework.Matrix

//    #region Microsoft.Xna.Framework.Vector2

//    /// <summary>Loads a vector from its serialized representation</summary>
//    /// <param name="reader">Reader to use for reading the vector</param>
//    /// <param name="vector">Receives the deserialized vector</param>
//    public static void Load(BinaryReader reader, out Vector2 vector) {
//      vector = new Vector2(
//        reader.ReadSingle(),
//        reader.ReadSingle()
//      );
//    }

//    /// <summary>Serializes a vector into a binary data stream</summary>
//    /// <param name="writer">BinaryWriter to serialize the vector into</param>
//    /// <param name="vector">Vector that will be serialized</param>
//    public static void Save(BinaryWriter writer, ref Vector2 vector) {
//      writer.Write(vector.X);
//      writer.Write(vector.Y);
//    }

//    #endregion // Microsoft.Xna.Framework.Vector2

//    #region Microsoft.Xna.Framework.Vector3

//    /// <summary>Loads a vector from its serialized representation</summary>
//    /// <param name="reader">Reader to use for reading the vector</param>
//    /// <param name="vector">Received the deserialized vector</param>
//    public static void Load(BinaryReader reader, out Vector3 vector) {
//      vector = new Vector3(
//        reader.ReadSingle(),
//        reader.ReadSingle(),
//        reader.ReadSingle()
//      );
//    }

//    /// <summary>Serializes a vector into a binary data stream</summary>
//    /// <param name="writer">BinaryWriter to serialize the vector into</param>
//    /// <param name="vector">Vector that will be serialized</param>
//    public static void Save(BinaryWriter writer, ref Vector3 vector) {
//      writer.Write(vector.X);
//      writer.Write(vector.Y);
//      writer.Write(vector.Z);
//    }

//    #endregion // Microsoft.Xna.Framework.Vector3

//    #region Microsoft.Xna.Framework.Vector4

//    /// <summary>Loads a vector from its serialized representation</summary>
//    /// <param name="reader">Reader to use for reading the vector</param>
//    /// <param name="vector">Receives the deserialized vector</param>
//    public static void Load(BinaryReader reader, out Vector4 vector) {
//      // This is valid in C# (but order of evaluation would be undefined in C++)
//      vector = new Vector4(
//        reader.ReadSingle(),
//        reader.ReadSingle(),
//        reader.ReadSingle(),
//        reader.ReadSingle()
//      );
//    }

//    /// <summary>Serializes a vector into a binary data stream</summary>
//    /// <param name="writer">BinaryWriter to serialize the vector into</param>
//    /// <param name="vector">Vector that will be serialized</param>
//    public static void Save(BinaryWriter writer, ref Vector4 vector) {
//      writer.Write(vector.X);
//      writer.Write(vector.Y);
//      writer.Write(vector.Z);
//      writer.Write(vector.W);
//    }

//    #endregion // Microsoft.Xna.Framework.Vector4

//    #region Microsoft.Xna.Framework.Quaternion

//    /// <summary>Loads a quaternion from its serialized representation</summary>
//    /// <param name="reader">Reader to use for reading the quaternion</param>
//    /// <param name="quaternion">Receives the deserialized quaternion</param>
//    public static void Load(BinaryReader reader, out Quaternion quaternion) {
//      // This is valid in C# (but order of evaluation would be undefined in C++)
//      quaternion = new Quaternion(
//        reader.ReadSingle(),
//        reader.ReadSingle(),
//        reader.ReadSingle(),
//        reader.ReadSingle()
//      );
//    }

//    /// <summary>Serializes a quaternion into a binary data stream</summary>
//    /// <param name="writer">BinaryWriter to serialize the quaternion into</param>
//    /// <param name="quaternion">Quaternion that will be serialized</param>
//    public static void Save(BinaryWriter writer, ref Quaternion quaternion) {
//      writer.Write(quaternion.X);
//      writer.Write(quaternion.Y);
//      writer.Write(quaternion.Z);
//      writer.Write(quaternion.W);
//    }

//    #endregion // Microsoft.Xna.Framework.Quaternion

//    #region Microsoft.Xna.Framework.Curve

//    /// <summary>Loads a curve from its serialized representation</summary>
//    /// <param name="reader">Reader to use for reading the curve</param>
//    /// <param name="curve">Curve to be deserialized</param>
//    /// <remarks>
//    ///   This method loads right into the curve and is not transactional.
//    ///   If an error occurs during loading, the curve is left in
//    ///   an intermediate state and no assumptions should be made as to its
//    ///   contents. If you need transactional safety, create a temporary curve,
//    ///   load into the temporary curve and then replace your actual
//    ///   curve with it.
//    /// </remarks>
//    public static void Load(BinaryReader reader, Curve curve) {
//      curve.Keys.Clear();

//      // Load the curve's loop settings
//      curve.PreLoop = (CurveLoopType)reader.ReadByte();
//      curve.PostLoop = (CurveLoopType)reader.ReadByte();

//      // Load the key frames defined for the curve
//      int keyCount = reader.ReadInt32();
//      for(int keyIndex = 0; keyIndex < keyCount; ++keyIndex) {
//        float position = reader.ReadSingle();
//        float value = reader.ReadSingle();
//        float tangentIn = reader.ReadSingle();
//        float tangentOut = reader.ReadSingle();
//        CurveContinuity continuity = (CurveContinuity)reader.ReadByte();

//        curve.Keys.Add(new CurveKey(position, value, tangentIn, tangentOut, continuity));
//      } // for
//    }

//    /// <summary>Serializes a curve into a binary data stream</summary>
//    /// <param name="writer">BinaryWriter to serialize the curve into</param>
//    /// <param name="curve">Curve to be serialized</param>
//    public static void Save(BinaryWriter writer, Curve curve) {

//      // Save the curve's loop settings
//      writer.Write((byte)curve.PreLoop);
//      writer.Write((byte)curve.PostLoop);

//      // Save the key frames contained in the curve
//      writer.Write(curve.Keys.Count);
//      for(int keyIndex = 0; keyIndex < curve.Keys.Count; ++keyIndex) {
//        CurveKey key = curve.Keys[keyIndex];

//        writer.Write(key.Position);
//        writer.Write(key.Value);
//        writer.Write(key.TangentIn);
//        writer.Write(key.TangentOut);
//        writer.Write((byte)key.Continuity);
//      } // for

//    }

//    #endregion // Microsoft.Xna.Framework.Curve

//    /// <summary>Checks whether the registration is valid</summary>
//    /// <param name="type">Type that will be checked</param>
//    /// <param name="simpleName">Simple name the type would be registered under</param>
//    /// <returns>True if the type is already registered, false otherwise</returns>
//    /// <exception cref="InvalidOperationException">
//    ///   If the type has already been registered under another name of the specified
//    ///   simple name has already been used for a different type.
//    /// </exception>
//    private static bool checkExistingRegistration(Type type, string simpleName) {
//      string existingSimpleName;
//      if(typeToSimpleNameDictionary.TryGetValue(type, out existingSimpleName)) {
//        if(simpleName == existingSimpleName) {
//          return true; // This registration already exists
//        }

//        throw new InvalidOperationException(
//          string.Format(
//            "Type '{0}' has already been given the simple name '{1}'",
//            type.Name, existingSimpleName
//          )
//        );
//      }

//      Type existingType;
//      if(simpleNameToTypeDictionary.TryGetValue(simpleName, out existingType)) {
//        throw new InvalidOperationException(
//          string.Format(
//            "Simple name '{0}' has already been used for type '{1}'",
//            simpleName, existingType.Name
//          )
//        );
//      }

//      return false;
//    }

//    /// <summary>Registers simple names for types the user might persist</summary>
//    private static void registerSimpleNamesForSerializableTypes() {
//      RegisterSimpleName<Microsoft.Xna.Framework.Vector2>("Xna.Vector2");
//      RegisterSimpleName<Microsoft.Xna.Framework.Vector3>("Xna.Vector3");
//      RegisterSimpleName<Microsoft.Xna.Framework.Vector4>("Xna.Vector4");
//      RegisterSimpleName<Microsoft.Xna.Framework.Matrix>("Xna.Matrix");
//      RegisterSimpleName<Microsoft.Xna.Framework.Quaternion>("Xna.Quaternion");
//      RegisterSimpleName<Microsoft.Xna.Framework.Curve>("Xna.Curve");
//    }

//    /// <summary>
//    ///   Registers simple names for types the user might load from a content manager
//    /// </summary>
//    private static void registerSimpleNamesForAssetTypes() {
//      RegisterSimpleName<Microsoft.Xna.Framework.Graphics.SpriteFont>("Xna.SpriteFont");
//      RegisterSimpleName<Microsoft.Xna.Framework.Graphics.Texture2D>("Xna.Texture2D");
//#if !MONOGAME
//      RegisterSimpleName<Microsoft.Xna.Framework.Graphics.Texture3D>("Xna.Texture3D");
//#endif
//      RegisterSimpleName<Microsoft.Xna.Framework.Graphics.Model>("Xna.Model");
//      RegisterSimpleName<Microsoft.Xna.Framework.Audio.SoundEffect>("Xna.SoundEffect");
//      RegisterSimpleName<Microsoft.Xna.Framework.Media.Song>("Xna.Song");
//#if !MONOGAME && !NO_VIDEO
//      RegisterSimpleName<Microsoft.Xna.Framework.Media.Video>("Xna.Video");
//#endif
//    }

//    /// <summary>Dictionary that converts simple names into types</summary>
//    private static IDictionary<string, Type> simpleNameToTypeDictionary;
//    /// <summary>Dictionary that converts types into simple names</summary>
//    private static IDictionary<Type, string> typeToSimpleNameDictionary;

//  } // class BinarySerializer

//} // namespace Nuclex.Game.Serialization
