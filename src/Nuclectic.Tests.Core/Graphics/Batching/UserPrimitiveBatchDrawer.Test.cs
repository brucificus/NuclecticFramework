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

using Microsoft.Xna.Framework.Graphics;
using Nuclectic.Graphics.TriD.Batching;
using Nuclectic.Tests.Mocks;
using SlimDX.Direct3D9;
using PrimitiveType = Microsoft.Xna.Framework.Graphics.PrimitiveType;
using VertexDeclaration = SlimDX.Direct3D9.VertexDeclaration;
#if UNITTEST
using System;
using NUnit.Framework;

namespace Nuclectic.Tests.Graphics.Batching
{
	/// <summary>Unit tests for the DrawUserPrimitive()-based batch drawer</summary>
	[TestFixture(IgnoreReason = "Unstable, may freeze test runner.")]
	[RequiresSTA]
	internal class UserPrimitiveBatchDrawerTest
		: TestFixtureBase
	{
		#region class Creator

		/// <summary>Manages the lifetime of a batch drawer instance</summary>
		private class Creator : IDisposable
		{
			/// <summary>Initializes a new batch drawer creator</summary>
			public Creator()
			{
				this.mockedService = PrepareGlobalExclusiveMockedGraphicsDeviceService();
				try
				{
					this.batchDrawer = new UserPrimitiveBatchDrawer<VertexDeclarationHelperTest.TestVertex>(
						this.mockedService.GraphicsDevice
						);
				}
				catch (Exception)
				{
					Dispose();
					throw;
				}
			}

			/// <summary>Immediately releases all resources owned by the instance</summary>
			public void Dispose()
			{
				if (this.batchDrawer != null)
				{
					this.batchDrawer.Dispose();
					this.batchDrawer = null;
				}
				if (this.vertexDeclaration != null)
				{
					this.vertexDeclaration.Dispose();
					this.vertexDeclaration = null;
				}
				if (this.graphicsDeviceKeeper != null)
				{
					this.graphicsDeviceKeeper.Dispose();
					this.graphicsDeviceKeeper = null;
				}
				this.mockedService = null;
			}

			/// <summary>Graphics device the batch drawer is using</summary>
			public GraphicsDevice GraphicsDevice { get { return this.mockedService.GraphicsDevice; } }

			/// <summary>The batch drawer being tested</summary>
			public UserPrimitiveBatchDrawer<VertexDeclarationHelperTest.TestVertex> BatchDrawer { get { return this.batchDrawer; } }

			/// <summary>Mocked graphics device service the drawer operates on</summary>
			private IMockedGraphicsDeviceService mockedService;

			/// <summary>Keeps the graphics device alive</summary>
			private IDisposable graphicsDeviceKeeper;

			/// <summary>Vertex declaration for the vertices we use for testing</summary>
			private VertexDeclaration vertexDeclaration;

			/// <summary>Batch drawer being tested</summary>
			private UserPrimitiveBatchDrawer<VertexDeclarationHelperTest.TestVertex> batchDrawer;
		}

		#endregion // class Creator

		/// <summary>
		///   Verifies that the constructor of the user primitive batch drawer works
		/// </summary>
		[Test]
		public void TestConstructor()
		{
			using (Creator creator = new Creator())
			{
				Assert.IsNotNull(creator.BatchDrawer);
			}
		}

		/// <summary>
		///   Tests whethe the MaximumBatchSize property of the batcher can be accessed
		/// </summary>
		[Test]
		public void TestMaximumBatchSize()
		{
			using (Creator creator = new Creator())
			{
				Assert.Greater(creator.BatchDrawer.MaximumBatchSize, 4);
			}
		}

		/// <summary>
		///   Tests whether the Select() method without vertex indices is working
		/// </summary>
		[Test]
		public void TestSelectWithoutIndices()
		{
			using (Creator creator = new Creator())
			{
				VertexDeclarationHelperTest.TestVertex[] vertices = new VertexDeclarationHelperTest.TestVertex[9];

				creator.BatchDrawer.Select(vertices, 9);
			}
		}

		/// <summary>
		///   Tests whether the Select() method with vertex indices is working
		/// </summary>
		[Test]
		public void TestSelectWithIndices()
		{
			using (Creator creator = new Creator())
			{
				VertexDeclarationHelperTest.TestVertex[] vertices = new VertexDeclarationHelperTest.TestVertex[9];
				short[] indices = new short[9];

				creator.BatchDrawer.Select(vertices, 9, indices, 9);
			}
		}

		/// <summary>
		///   Tests whether the Draw() method without vertex indices is working
		/// </summary>
		[Test]
		public void TestDrawWithoutIndices()
		{
			using (Creator creator = new Creator())
			{
				VertexDeclarationHelperTest.TestVertex[] vertices = new VertexDeclarationHelperTest.TestVertex[9];

				creator.BatchDrawer.Select(vertices, 9);
				creator.BatchDrawer.Draw(
										 0, 9,
										 PrimitiveType.TriangleList,
										 new BasicEffectDrawContext(creator.GraphicsDevice)
					);
			}
		}

		/// <summary>
		///   Tests whether the Draw() method with vertex indices is working
		/// </summary>
		[Test]
		public void TestDrawWithIndices()
		{
			using (Creator creator = new Creator())
			{
				VertexDeclarationHelperTest.TestVertex[] vertices = new VertexDeclarationHelperTest.TestVertex[9];
				short[] indices = new short[9];

				creator.BatchDrawer.Select(vertices, 9, indices, 9);
				creator.BatchDrawer.Draw(
										 0, 9,
										 0, 9,
										 PrimitiveType.TriangleList,
										 new BasicEffectDrawContext(creator.GraphicsDevice)
					);
			}
		}
	}
} // namespace Nuclex.Graphics

#endif // UNITTEST