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
using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

namespace Nuclex.Graphics.Batching {

  /// <summary>Draws batched vertices using DrawUserPrimitive() calls</summary>
  internal class UserPrimitiveBatchDrawer<VertexType> :
    IBatchDrawer<VertexType>, IDisposable
    where VertexType : struct, IVertexType {

    /// <summary>Initializes a new DrawUserPrimitive()-based batch drawer</summary>
    /// <param name="graphicsDevice">
    ///   Graphics device that will be used for rendering
    /// </param>
    public UserPrimitiveBatchDrawer(GraphicsDevice graphicsDevice) {
      this.graphicsDevice = graphicsDevice;
    }

    /// <summary>Immediately releases all resources owned by the instance</summary>
    public void Dispose() { }

    /// <summary>
    ///   Maximum number of vertices or indices a single batch is allowed to have
    /// </summary>
    public int MaximumBatchSize {
      get { return PrimitiveBatch<VertexType>.BatchSize; }
    }

    /// <summary>Selects the vertices that will be used for drawing</summary>
    /// <param name="vertices">Primitive vertices</param>
    /// <param name="vertexCount">Number of vertices to draw</param>
    /// <param name="indices">Indices of the vertices to draw</param>
    /// <param name="indexCount">Number of vertex indices to draw</param>
    public void Select(
      VertexType[] vertices, int vertexCount,
      short[] indices, int indexCount
    ) {
      this.vertices = vertices;
      this.indices = indices;
    }

    /// <summary>Selects the vertices that will be used for drawing</summary>
    /// <param name="vertices">Primitive vertices</param>
    /// <param name="vertexCount">Number of vertices to draw</param>
    public void Select(
      VertexType[] vertices, int vertexCount
    ) {
      this.vertices = vertices;
    }

    /// <summary>Draws a batch of indexed primitives</summary>
    /// <param name="startVertex">
    ///   Index of the first vertex in the vertex array. This vertex will become
    ///   the new index 0 for the index buffer.
    /// </param>
    /// <param name="vertexCount">Number of vertices used in the call</param>
    /// <param name="startIndex">
    ///   Position at which to begin processing the index array
    /// </param>
    /// <param name="indexCount">Number of indices that will be processed</param>
    /// <param name="type">Type of primitives to draw</param>
    /// <param name="context">Desired graphics device settings for the primitives</param>
    public void Draw(
      int startVertex, int vertexCount,
      int startIndex, int indexCount,
      PrimitiveType type, DrawContext context
    ) {
      int primitiveCount = VertexHelper.GetPrimitiveCount(indexCount, type);

      // If the DrawContext requires more than one pass, we have to draw the
      // primitives multiple times
      for (int pass = 0; pass < context.Passes; ++pass) {

        // Enter the effect pass and send all primitives to the graphics card
        context.Apply(pass);

        this.graphicsDevice.DrawUserIndexedPrimitives<VertexType>(
          type, // type of primitives to render
          this.vertices, // vertex array
          startVertex, // offset that will be added to all vertex indices
          vertexCount, // number of vertices used in the call
          this.indices, // index array
          startIndex, // where in the index array to begin processing
          primitiveCount // number of primitives
        );

      } // for
    }

    /// <summary>Draws a batch of primitives</summary>
    /// <param name="startVertex">Index of vertex to begin drawing with</param>
    /// <param name="vertexCount">Number of vertices used</param>
    /// <param name="type">Type of primitives that will be drawn</param>
    /// <param name="context">Desired graphics device settings for the primitives</param>
    public void Draw(
      int startVertex, int vertexCount,
      PrimitiveType type, DrawContext context
    ) {
      int primitiveCount = VertexHelper.GetPrimitiveCount(vertexCount, type);

      // If the DrawContext requires more than one pass, we have to draw the
      // primitives multiple times
      for (int pass = 0; pass < context.Passes; ++pass) {

        // Enter the effect pass and send all primitives to the graphics card
        context.Apply(pass);

        this.graphicsDevice.DrawUserPrimitives<VertexType>(
          type, // type of primitives to render
          this.vertices, // vertex array
          startVertex, // where in the vertex array to begin processing
          primitiveCount // number of primitives
        );

      } // for
    }

    /// <summary>Graphics device being used for rendering</summary>
    private GraphicsDevice graphicsDevice;
    /// <summary>Vertices being used for drawing</summary>
    private VertexType[] vertices;
    /// <summary>Indices being used for drawing</summary>
    private short[] indices;

  }

} // namespace Nuclex.Graphics.Batching
