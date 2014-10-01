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
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Nuclectic.Graphics.Abstractions.Batching;
using Nuclectic.Graphics.Helpers;

namespace Nuclectic.Graphics.Batching {

  /// <summary>Draws batched vertices using a dynamic vertex buffer</summary>
  public class DynamicBufferBatchDrawer<VertexType> :
    IBatchDrawer<VertexType>, IDisposable
    where VertexType : struct, IVertexType {

    /// <summary>Number of regions the vertex buffer is divided into</summary>
    public const int Divisions = 4;

    /// <summary>
    ///   Equivalent to SetDataOptions.Discard if supported by the target platform
    /// </summary>
    private const SetDataOptions DiscardIfPossible = SetDataOptions.None;

    /// <summary>Initializes a new dynamic vertex buffer based batch drawer</summary>
    /// <param name="graphicsDevice">
    ///   Graphics device that will be used for rendering
    /// </param>
    public DynamicBufferBatchDrawer(GraphicsDevice graphicsDevice) {
      const int BatchSize = PrimitiveBatch<VertexType>.BatchSize;
      const int BufferSize = BatchSize * Divisions;

      Debug.Assert(
        (BufferSize - 1) <= short.MaxValue,
        "Selected batch size results in a vertex buffer that's too large"
      );

      this.graphicsDevice = graphicsDevice;

      // Create a new vertex buffer
      this.vertexBuffer = new DynamicVertexBuffer(
        graphicsDevice, typeof(VertexType), BufferSize, BufferUsage.WriteOnly
      );
      this.stride = this.vertexBuffer.VertexDeclaration.VertexStride;

      // Create a new index buffer
      this.indexBuffer = new DynamicIndexBuffer(
        graphicsDevice,
        typeof(short), 
        BufferSize, 
        BufferUsage.WriteOnly
      );
    }

    /// <summary>Immediately releases all resources owned by the instance</summary>
    public void Dispose() {
      if(this.vertexBuffer != null) {
        this.vertexBuffer.Dispose();
        this.vertexBuffer = null;
      }
      if(this.indexBuffer != null) {
        this.indexBuffer.Dispose();
        this.indexBuffer = null;
      }
    }

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
      unselectBuffers();

      this.currentDivisionIndex = (this.currentDivisionIndex + 1) % Divisions;
      if(this.currentDivisionIndex == 0) {
        this.vertexBuffer.SetData<VertexType>(
          0, vertices, 0, vertexCount, this.stride, DiscardIfPossible
        );    
        this.indexBuffer.SetData<short>(
          0, indices, 0, indexCount, DiscardIfPossible
        );
      } else {
        int offset = PrimitiveBatch<VertexType>.BatchSize * this.currentDivisionIndex;
        this.vertexBuffer.SetData<VertexType>(
          offset * this.stride,
          vertices, 0, vertexCount, this.stride, SetDataOptions.NoOverwrite
        );
        this.indexBuffer.SetData<short>(
          offset * 2,
          indices, 0, indexCount, SetDataOptions.NoOverwrite
        );
      }

      selectBuffers(this.currentDivisionIndex);
    }

    /// <summary>Selects the vertices that will be used for drawing</summary>
    /// <param name="vertices">Primitive vertices</param>
    /// <param name="vertexCount">Number of vertices to draw</param>
    public void Select(
      VertexType[] vertices, int vertexCount
    ) {
      unselectBuffers();

      this.currentDivisionIndex = (this.currentDivisionIndex + 1) % Divisions;
      if(this.currentDivisionIndex == 0) {
        this.vertexBuffer.SetData<VertexType>(
          0, vertices, 0, vertexCount, this.stride, DiscardIfPossible
        );
                                         } else {
        int offset = PrimitiveBatch<VertexType>.BatchSize * this.currentDivisionIndex;
        this.vertexBuffer.SetData<VertexType>(
          offset * this.stride,
          vertices, 0, vertexCount, this.stride, SetDataOptions.NoOverwrite
        );                                         }

      selectBuffers(this.currentDivisionIndex);
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
      startIndex += PrimitiveBatch<VertexType>.BatchSize * this.currentDivisionIndex;

      int primitiveCount = VertexHelper.GetPrimitiveCount(indexCount, type);

      // If the DrawContext requires more than one pass, we have to draw the
      // primitives multiple times
      for (int pass = 0; pass < context.Passes; ++pass) {

        // Enter the pass and send all primitives to the graphics card
        context.Apply(pass);

        this.graphicsDevice.DrawIndexedPrimitives(
          type, // type of primitives to render
          startVertex, // offset that will be added to all vertex indices
          0, // lowest vertex used in the call (relative to previous parameter)
          vertexCount, // number of vertices used in the call
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

        // Enter the pass and send all primitives to the graphics card
        context.Apply(pass);

        this.graphicsDevice.DrawPrimitives(
          type, // type of primitives to render
          startVertex, // offset that will be added to all vertex indices
          primitiveCount // number of primitives being drawn
        );

      } // for
    }

    /// <summary>Deselects the index and vertex buffers</summary>
    private void unselectBuffers() {

      // Unselect the buffer to allow them to be modified
      this.graphicsDevice.Indices = null;
      this.graphicsDevice.SetVertexBuffer(null);

    }

    /// <summary>Selects the index and vertex buffers</summary>
    /// <param name="divisionIndex">Index of the division to select</param>
    private void selectBuffers(int divisionIndex) {
      const int BatchSize = PrimitiveBatch<VertexType>.BatchSize;

      // Assign the index and vertex buffers for the DrawPrimitive call
      this.graphicsDevice.Indices = this.indexBuffer;
#if !MONOGAME
      this.graphicsDevice.SetVertexBuffer(
        this.vertexBuffer,
        BatchSize * divisionIndex
      );
#else
    if ((BatchSize*divisionIndex) > 0)
        throw new NotSupportedException("MonoGame immaturity");
      this.graphicsDevice.SetVertexBuffer(
        this.vertexBuffer
      );
#endif
    }

    /// <summary>Graphics device being used for rendering</summary>
    private GraphicsDevice graphicsDevice;
    /// <summary>Dynamic VertexBuffer used to render batches</summary>
    private DynamicVertexBuffer vertexBuffer;
    /// <summary>Dynamic IndexBuffer used to render batches</summary>
    private DynamicIndexBuffer indexBuffer;

    /// <summary>Division that will be filled next</summary>
    private int currentDivisionIndex;
    /// <summary>Size, in bytes, of a single vertex</summary>
    private int stride;
  }

} // namespace Nuclex.Graphics.Batching
