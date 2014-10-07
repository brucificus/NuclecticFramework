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

#if UNITTEST

using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NUnit.Framework;

using Nuclex.Testing.Xna;

using TestVertex = Microsoft.Xna.Framework.Graphics.VertexPositionColor;
using System.Threading;

namespace Nuclex.Graphics.Batching {

  /// <summary>Unit tests for the primitive batcher</summary>
  [TestFixture]
  internal class PrimitiveBatchTest {

    #region class DummyDrawContext

    /// <summary>Drawing context used for the unit test</summary>
    private class DummyDrawContext : DrawContext {

      /// <summary>Number of passes this draw context requires for rendering</summary>
      public override int Passes {
        get { return 0; }
      }

      /// <summary>Prepares the graphics device for drawing</summary>
      /// <param name="pass">Index of the pass to begin rendering</param>
      /// <remarks>
      ///   Should only be called between the normal Begin() and End() methods.
      /// </remarks>
      public override void Apply(int pass) { }

      /// <summary>Tests whether another draw context is identical to this one</summary>
      /// <param name="otherContext">Other context to check for equality</param>
      /// <returns>True if the other context is identical to this one</returns>
      public override bool Equals(DrawContext otherContext) {
        return ReferenceEquals(this, otherContext);
      }

    }

    #endregion // class TestDrawContext

    #region class PrimitiveBatchCreator

    /// <summary>Helper class that automates primitive batcher creation</summary>
    private class PrimitiveBatchCreator : IDisposable {

      /// <summary>Initializes a new primitive batch creator</summary>
      public PrimitiveBatchCreator() {
        this.mockedGraphicsDeviceService = new MockedGraphicsDeviceService();
        this.graphicsDeviceKeeper = this.mockedGraphicsDeviceService.CreateDevice();
        this.vertexDeclaration = TestVertex.VertexDeclaration;
        this.primitiveBatch = new PrimitiveBatch<TestVertex>(
          this.mockedGraphicsDeviceService.GraphicsDevice
        );
      }

      /// <summary>Immediately releases all resources owned by the instance</summary>
      public void Dispose() {
        if(this.primitiveBatch != null) {
          this.primitiveBatch.Dispose();
          this.primitiveBatch = null;
        }
        if(this.graphicsDeviceKeeper != null) {
          this.graphicsDeviceKeeper.Dispose();
          this.graphicsDeviceKeeper = null;
        }

        this.mockedGraphicsDeviceService = null;
      }

      /// <summary>Mocked graphics device service used for rendering</summary>
      public MockedGraphicsDeviceService MockedGraphicsDeviceService {
        get { return this.mockedGraphicsDeviceService; }
      }

      /// <summary>Primitive batch the primitive batch creator has created</summary>
      public PrimitiveBatch<TestVertex> PrimitiveBatch {
        get { return this.primitiveBatch; }
      }

      /// <summary>
      ///   The mocked graphics device sevice providing the graphics device for testing
      /// </summary>
      private MockedGraphicsDeviceService mockedGraphicsDeviceService;
      /// <summary>Keeps the graphics device alive until disposed</summary>
      private IDisposable graphicsDeviceKeeper;
      /// <summary>Declaration for the vertex structure we're using in the tests</summary>
      private VertexDeclaration vertexDeclaration;
      /// <summary>Primitive batcher being tested</summary>
      private PrimitiveBatch<TestVertex> primitiveBatch;

    }

    #endregion // class PrimitiveBatchCreator

    /// <summary>
    ///   Tests whether the constructor of the primitve batcher is working
    /// </summary>
    [Test]
    public void TestConstructor() {
      using(PrimitiveBatchCreator creator = new PrimitiveBatchCreator()) {
        Assert.IsNotNull(creator.PrimitiveBatch);
      }
    }

    /// <summary>
    ///   Tests whether the different queueing strategies can be selected
    /// </summary>
    [Test]
    public void TestStrategySwitching() {
      using(PrimitiveBatchCreator creator = new PrimitiveBatchCreator()) {
        creator.PrimitiveBatch.Begin(QueueingStrategy.Immediate);
        creator.PrimitiveBatch.End();

        creator.PrimitiveBatch.Begin(QueueingStrategy.Deferred);
        creator.PrimitiveBatch.End();

        creator.PrimitiveBatch.Begin(QueueingStrategy.Context);
        creator.PrimitiveBatch.End();

        creator.PrimitiveBatch.Begin(QueueingStrategy.Immediate);
        creator.PrimitiveBatch.End();
      }
    }

    /// <summary>
    ///   Verifies that an exception is thrown if an invalid queueing strategy
    ///   is chosen
    /// </summary>
    [Test]
    public void TestInvalidStrategy() {
      using(PrimitiveBatchCreator creator = new PrimitiveBatchCreator()) {
        Assert.Throws<ArgumentException>(
          delegate() {
            creator.PrimitiveBatch.Begin((QueueingStrategy)(-1));
            creator.PrimitiveBatch.End();
          }
        );
      }
    }

    /// <summary>
    ///   Verifies that the primitive batcher can draw all kinds of primitives
    /// </summary>
    [Test]
    public void TestDrawPrimitives() {
      using(PrimitiveBatchCreator creator = new PrimitiveBatchCreator()) {
        DummyDrawContext dummy = new DummyDrawContext();

        creator.PrimitiveBatch.Begin(QueueingStrategy.Immediate);
        try {
          creator.PrimitiveBatch.Draw(
            new TestVertex[10], PrimitiveType.LineList, dummy
          );
          creator.PrimitiveBatch.Draw(
            new TestVertex[10], PrimitiveType.LineStrip, dummy
          );
          creator.PrimitiveBatch.Draw(
            new TestVertex[9], PrimitiveType.TriangleList, dummy
          );
          creator.PrimitiveBatch.Draw(
            new TestVertex[10], PrimitiveType.TriangleStrip, dummy
          );
        }
        finally {
          creator.PrimitiveBatch.End();
        }
      }
    }

    /// <summary>
    ///   Verifies that the primitive batcher can draw all kinds of primitives
    ///   using an index array
    /// </summary>
    [Test]
    public void TestDrawIndexedPrimitives() {
      using(PrimitiveBatchCreator creator = new PrimitiveBatchCreator()) {
        DummyDrawContext dummy = new DummyDrawContext();

        creator.PrimitiveBatch.Begin(QueueingStrategy.Immediate);
        try {
          creator.PrimitiveBatch.Draw(
            new TestVertex[10], createIndices(10), PrimitiveType.LineList, dummy
          );
          creator.PrimitiveBatch.Draw(
            new TestVertex[10], createIndices(10), PrimitiveType.LineStrip, dummy
          );
          creator.PrimitiveBatch.Draw(
            new TestVertex[9], createIndices(9), PrimitiveType.TriangleList, dummy
          );
          creator.PrimitiveBatch.Draw(
            new TestVertex[10], createIndices(10), PrimitiveType.TriangleStrip, dummy
          );
        }
        finally {
          creator.PrimitiveBatch.End();
        }
      }
    }

    /// <summary>
    ///   Tests whether the primitive batch is able to draw a list of triangles
    /// </summary>
    [Test]
    public void TestDrawTriangles() {
      using(PrimitiveBatchCreator creator = new PrimitiveBatchCreator()) {
        DummyDrawContext dummy = new DummyDrawContext();

        creator.PrimitiveBatch.Begin(QueueingStrategy.Immediate);
        creator.PrimitiveBatch.Draw(new VertexPositionColor[9], dummy);
        creator.PrimitiveBatch.End();
      }
    }

    /// <summary>
    ///   Tests whether the primitive batch is able to draw a list of triangles
    ///   indexed by an index array
    /// </summary>
    [Test]
    public void TestDrawIndexedTriangles() {
      using(PrimitiveBatchCreator creator = new PrimitiveBatchCreator()) {
        DummyDrawContext dummy = new DummyDrawContext();

        creator.PrimitiveBatch.Begin(QueueingStrategy.Immediate);
        creator.PrimitiveBatch.Draw(
          new VertexPositionColor[9], createIndices(9), dummy
        );
        creator.PrimitiveBatch.End();
      }
    }

    /// <summary>
    ///   Verifies that the primitive batcher can survive a graphics device reset
    /// </summary>
    [Test]
    public void TestGraphicsDeviceReset() {
      using(PrimitiveBatchCreator creator = new PrimitiveBatchCreator()) {
        DummyDrawContext dummy = new DummyDrawContext();

        // Simulate a graphics device reset
        creator.MockedGraphicsDeviceService.ResetDevice();

        creator.PrimitiveBatch.Begin(QueueingStrategy.Immediate);
        creator.PrimitiveBatch.Draw(new VertexPositionColor[9], dummy);
        creator.PrimitiveBatch.End();
      }
    }

    /// <summary>Creates a list of sequential indices</summary>
    /// <param name="count">Number of indices to generate</param>
    /// <returns>An array containing the generated indices</returns>
    private static short[] createIndices(int count) {
      short[] indices = new short[9];
      for(short index = 0; index < 9; ++index) {
        indices[index] = index;
      }
      return indices;
    }

  }

} // namespace Nuclex.Graphics

#endif // UNITTEST
