#region CPL License
/*
Nuclex Framework
Copyright (C) 2002-2009 Nuclex Development Labs

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
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Nuclectic.Support.Helpers;
using Nuclectic.Support.Helpers.InteropServices.Marshal;

namespace Nuclectic.Graphics.Helpers {

  /// <summary>
  ///   Builds vertex declarations from vertex structures
  /// </summary>
  /// <remarks>
  ///   Based on ideas from Michael Popoloski's article on gamedev.net:
  ///   http://www.gamedev.net/reference/programming/features/xnaVertexElement/
  /// </remarks>
  public static class VertexDeclarationHelper {

    /// <summary>Combines two vertex element list into one single list</summary>
    /// <param name="left">First vertex element list that will be merged</param>
    /// <param name="right">Second vertex element list that will be merged</param>
    /// <returns>The combined vertex element list from both inputs</returns>
    /// <remarks>
    ///   <para>
    ///     No intelligence is applied to avoid duplicates or to adjust the usage index
    ///     of individual vertex elements. This method simply serves as a helper to merge
    ///     two vertex element lists from two structures that are used in seperate
    ///     vertex streams (but require a single vertex declaration containing the elements
    ///     of both streams).
    ///   </para>
    ///   <para>
    ///     <example>
    ///       This example shows how two vertex structures, each used in a different
    ///       vertex buffer, can be merged into a single vertex declaration that fetches
    ///       vertices from both vertex buffers, the positions from stream 0 and
    ///       the texture coordinates from stream 1
    ///       <code>
    ///         struct PositionVertex {
    ///           [VertexElement(VertexElementUsage.Position)]
    ///           public Vector3 Position;
    ///         }
    ///         struct TextureCoordinateVertex {
    ///           [VertexElement(VertexElementUsage.TextureCoordinate)]
    ///           public Vector2 TextureCoordinate;
    ///         }
    ///         
    ///         private VertexDeclaration buildVertexDeclaration() {
    ///           VertexDeclaration declaration = new VertexDeclaration(
    ///             graphicsDevice,
    ///             VertexDeclarationHelper.Combine(
    ///               VertexDeclarationHelper.BuildElementList&lt;PositionVertex&gt;(0),
    ///               VertexDeclarationHelper.BuildElementList&lt;TextureCoordinateVertex&gt;(1)
    ///             )
    ///           );
    ///         }
    ///       </code>
    ///     </example>
    ///   </para>
    /// </remarks>
    public static VertexElement[] Combine(VertexElement[] left, VertexElement[] right) {

      // Determine the total length the resulting array will have. If one of the arguments
      // is null, this line will intentionally trigger the NullReferenceException
      int totalLength = left.Length + right.Length;

      // Merge the two arrays
      VertexElement[] combined = new VertexElement[totalLength];
      Array.Copy(left, combined, left.Length);
      Array.Copy(right, 0, combined, left.Length, right.Length);

      // Done, no further processing required
      return combined;

    }
   
		/// <summary>
		///   Builds a vertex element list that can be used to construct a vertex declaration
		///   from a vertex structure that has the vertex element attributes applied to it
		/// </summary>
		/// <typeparam name="VertexType">
		///   Vertex structure with vertex element attributes applied to it
		/// </typeparam>
		/// <returns>
		///   A vertex element list that can be used to create a new vertex declaration matching
		///   the provided vertex structure
		/// </returns>
		[Obsolete("Use VertexDefinitionBuilder")]
		public static VertexElement[] BuildElementList<VertexType>() where VertexType : struct
		{
			// NOTE: This is complicated because Marshal.OffsetOf is supported in few places
			var lateBoundMarshalOffsetOf = (new LateBoundMarshalFactory()).CreateMarshalOffsetOf();
			VertexDefinitionBuilder builder;
			var marshalOffsetOfByFieldOffsetAttribute = new MarshalOffsetOfByFieldOffsetAttribute();
			if (lateBoundMarshalOffsetOf != null)
			{
				builder = new VertexDefinitionBuilder(new MarshalOffsetOfByStrategicOr(lateBoundMarshalOffsetOf, marshalOffsetOfByFieldOffsetAttribute));
			}
			else
			{
				builder = new VertexDefinitionBuilder(marshalOffsetOfByFieldOffsetAttribute);
			}
			return builder.BuildElementList<VertexType>();
		}

		/// <summary>Obtains the stride value for a vertex</summary>
		/// <typeparam name="VertexType">
		///   Vertex structure the stride value will be obtained for
		/// </typeparam>
		/// <returns>The stride value for the specified vertex structure</returns>
		internal static int GetStride<VertexType>(IMarshalSizeOf marshalSizeOf) where VertexType : struct
		{
			var fields = typeof(VertexType).GetTypeInfo().DeclaredFields.ToArray();

			int fieldOffset = 0;
			for (int index = 0; index < fields.Length; ++index)
			{
				fieldOffset += marshalSizeOf.SizeOf(fields[index].FieldType).Value;
			}

			return fieldOffset;
		}




  }
} // namespace Nuclex.Graphics
