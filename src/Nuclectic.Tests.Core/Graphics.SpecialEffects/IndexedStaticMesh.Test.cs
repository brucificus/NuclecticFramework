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

namespace Nuclex.Graphics.SpecialEffects {

  /// <summary>Unit test for the indexed mesh graphics resource keeper</summary>
  [TestFixture]
  internal class IndexedStaticMeshTest {

    #region struct TestVertex

    /// <summary>
    ///   Vertex used to unit-test the static mesh graphics resource keepr
    /// </summary>
    private struct TestVertex : IVertexType {

      /// <summary>A vertex element of type Vector2</summary>
      [VertexElement(VertexElementUsage.Position)]
      public Vector3 Position;
      /// <summary>A vertex element of type Color</summary>
      [VertexElement(VertexElementUsage.Color)]
      public Color Color;

      /// <summary>Provides a declaration for this vertex type</summary>
      VertexDeclaration IVertexType.VertexDeclaration {
        get { return TestVertex.VertexDeclaration; }
      }

      /// <summary>Vertex declaration for this vertex structure</summary>
      public static readonly VertexDeclaration VertexDeclaration =
        new VertexDeclaration(VertexDeclarationHelper.BuildElementList<TestVertex>());

    }

    #endregion // struct TestVertex

    #region class TestStaticMesh

    /// <summary>Dummy static mesh class used for unit testing</summary>
    private class TestIndexedStaticMesh : IndexedStaticMesh<TestVertex> {

      /// <summary>
      ///   Initializes a new static mesh that automatically determines its vertex format
      /// </summary>
      /// <param name="graphicsDevice">Graphics device the static mesh lives on</param>
      /// <param name="vertexCount">Number of vertices used by the static mesh</param>
      /// <param name="indexCount">Number of indices in the static mesh</param>
      public TestIndexedStaticMesh(
        GraphicsDevice graphicsDevice, int vertexCount, int indexCount
      ) :
        base(graphicsDevice, vertexCount, indexCount) { }

      /// <summary>Selects the static meshes' vertex buffer</summary>
      public new void Select() {
        base.Select();
      }

      /// <summary>Index buffer containing the test meshes' indices</summary>
      public new IndexBuffer IndexBuffer {
        get { return base.IndexBuffer; }
      }

    }

    #endregion // class TestStaticMesh

    /// <summary>
    ///   Verifies that the simple constructor of the static mesh class is working
    /// </summary>
    [Test]
    public void TestSimpleConstructor() {
      MockedGraphicsDeviceService mockGraphicsDeviceService =
        new MockedGraphicsDeviceService();

      using(IDisposable keeper = mockGraphicsDeviceService.CreateDevice()) {
        using(
          TestIndexedStaticMesh test = new TestIndexedStaticMesh(
            mockGraphicsDeviceService.GraphicsDevice, 4, 4
          )
        ) { }
      }
    }

    /// <summary>
    ///   Verifies that the simple constructor rolls back when an exception occurs in it
    /// </summary>
    [Test]
    public void TestThrowInSimpleConstructorRollback() {
      MockedGraphicsDeviceService mockGraphicsDeviceService =
        new MockedGraphicsDeviceService();

      using(IDisposable keeper = mockGraphicsDeviceService.CreateDevice()) {
        Assert.Throws<ArgumentOutOfRangeException>(
          delegate() {
            using(
              TestIndexedStaticMesh test = new TestIndexedStaticMesh(
                mockGraphicsDeviceService.GraphicsDevice, 4, -1
              )
            ) { }
          }
        );
      }
    }

    /// <summary>
    ///   Tests whether the static meshes' Select() method is implemented correctly
    /// </summary>
    [Test]
    public void TestSelect() {
      MockedGraphicsDeviceService mockGraphicsDeviceService =
        new MockedGraphicsDeviceService();

      using(IDisposable keeper = mockGraphicsDeviceService.CreateDevice()) {
        using(
          TestIndexedStaticMesh test = new TestIndexedStaticMesh(
            mockGraphicsDeviceService.GraphicsDevice, 4, 4
          )
        ) {
          test.Select();
          Assert.AreSame(
            test.IndexBuffer,
            mockGraphicsDeviceService.GraphicsDevice.Indices
          );
        }
      }
    }

    /// <summary>
    ///   Only exists to prevent the compiler from complaining about unused fields
    /// </summary>
    protected void AvoidCompilerWarnings() {
      TestVertex v;
      v.Color = Color.Red;
      v.Position = Vector3.Zero;
    }

  }

} // namespace Nuclex.Graphics.SpecialEffects

#endif // UNITTEST
