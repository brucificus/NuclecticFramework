#region CPL License

///*
//Nuclex Framework
//Copyright (C) 2002-2009 Nuclex Development Labs

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

#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclectic.Fonts;
using Nuclectic.Graphics.Helpers;
using Nuclectic.Tests.Mocks;
using Nuclectic.Tests.Resources;
using SlimDX.Direct3D9;
using Effect = Microsoft.Xna.Framework.Graphics.Effect;
using Text = Nuclectic.Fonts.Text;
#if UNITTEST
using NUnit.Framework;

namespace Nuclectic.Tests.Fonts
{

	/// <summary>Unit tests for the text batcher</summary>
	[TestFixture(IgnoreReason = "Unstable, may freeze test runner.")]
	public class TextBatchTest
		: TestFixtureBase
	{

		#region class TestText

		/// <summary>Test implemented of a text mesh for the unit test</summary>
		private class TestText : Text
		{

			/// <summary>Initializes a new test text mesh</summary>
			public TestText()
			{
				this.vertices = new VertexPositionNormalTexture[12];
				this.indices = new short[12];
				this.primitiveType = Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleList;
				this.width = 12.34f;
				this.height = 56.78f;
			}

		}

		#endregion // class TestText

		/// <summary>Verifies that instances of the effect can be created</summary>
		[Test]
		[RequiresSTA]
		public void TestConstructor()
		{
			using (var mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			{
				var servicesProvider = GraphicsDeviceServiceHelper.MakePrivateServiceProvider(mockedGraphicsDeviceService);
				
				using (var contentManager = new ResourceContentManager(servicesProvider, TextBatchResources.ResourceManager))
				using (var effect = contentManager.Load<Effect>("DefaultTextEffect"))
				using (var textBatch = new TextBatch(mockedGraphicsDeviceService.GraphicsDevice, effect))
				{
					Assert.IsNotNull(textBatch);
				}
			}
		}


		/// <summary>
		///   Verifies that the view/projection matrix is saved by the text batcher
		/// </summary>
		[Test]
		[RequiresSTA]
		public void TestViewProjectionMatrix()
		{
			using (var mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			{
				var servicesProvider = GraphicsDeviceServiceHelper.MakePrivateServiceProvider(mockedGraphicsDeviceService);

				using (var contentManager = new ResourceContentManager(servicesProvider, TextBatchResources.ResourceManager))
				using (var effect = contentManager.Load<Effect>("DefaultTextEffect"))
				using (var textBatch = new TextBatch(mockedGraphicsDeviceService.GraphicsDevice, effect))
				{
					Matrix testMatrix = new Matrix(
					  1.1f, 1.2f, 1.3f, 1.4f,
					  2.1f, 2.2f, 2.3f, 2.4f,
					  3.1f, 3.2f, 3.3f, 3.4f,
					  4.1f, 4.2f, 4.3f, 4.4f
					);

					textBatch.ViewProjection = testMatrix;
					Assert.AreEqual(testMatrix, textBatch.ViewProjection);
				}
			}
		}

		/// <summary>
		///   Tests whether the Begin() and End() methods can be called without any
		///   drawing commands inbetween
		/// </summary>
		[Test]
		[RequiresSTA]
		public void TestBeginEnd()
		{
			using (var mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			{
				var servicesProvider = GraphicsDeviceServiceHelper.MakePrivateServiceProvider(mockedGraphicsDeviceService);

				using (var contentManager = new ResourceContentManager(servicesProvider, TextBatchResources.ResourceManager))
				using (var effect = contentManager.Load<Effect>("DefaultTextEffect"))
				using (var textBatch = new TextBatch(mockedGraphicsDeviceService.GraphicsDevice, effect))
				{
					textBatch.Begin();
					textBatch.End();
				}
			}
		}

		/// <summary>
		///   Tests the text drawing method using the default transformation matrix
		/// </summary>
		[Test]
		[RequiresSTA]
		public void TestDrawTextWithDefaultTransform()
		{
			using (var mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			{
				var servicesProvider = GraphicsDeviceServiceHelper.MakePrivateServiceProvider(mockedGraphicsDeviceService);

				using (var contentManager = new ResourceContentManager(servicesProvider, TextBatchResources.ResourceManager))
				using (var effect = contentManager.Load<Effect>("DefaultTextEffect"))
				using (var textBatch = new TextBatch(mockedGraphicsDeviceService.GraphicsDevice, effect))
				{
					TestText test = new TestText();

					textBatch.Begin();
					try
					{
						textBatch.DrawText(test, Color.White);
					}
					finally
					{
						textBatch.End();
					}
				}
			}
		}

		/// <summary>
		///   Tests the text drawing method using a custom transformation matrix
		/// </summary>
		[Test]
		[RequiresSTA]
		public void TestDrawTextWithCustomTransform()
		{
			using (var mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			{
				var servicesProvider = GraphicsDeviceServiceHelper.MakePrivateServiceProvider(mockedGraphicsDeviceService);

				using (var contentManager = new ResourceContentManager(servicesProvider, TextBatchResources.ResourceManager))
				using (var effect = contentManager.Load<Effect>("DefaultTextEffect"))
				using (var textBatch = new TextBatch(mockedGraphicsDeviceService.GraphicsDevice, effect))
				{
					TestText test = new TestText();
					Matrix testMatrix = new Matrix(
					  1.1f, 1.2f, 1.3f, 1.4f,
					  2.1f, 2.2f, 2.3f, 2.4f,
					  3.1f, 3.2f, 3.3f, 3.4f,
					  4.1f, 4.2f, 4.3f, 4.4f
					);

					textBatch.Begin();
					try
					{
						textBatch.DrawText(test, testMatrix, Color.White);
					}
					finally
					{
						textBatch.End();
					}
				}
			}
		}

		/// <summary>
		///   Tests the text drawing method using a custom effect
		/// </summary>
		[Test]
		[RequiresSTA]
		public void TestDrawTextWithCustomEffect()
		{
			using (var mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			{
				var servicesProvider = GraphicsDeviceServiceHelper.MakePrivateServiceProvider(mockedGraphicsDeviceService);

				using (var contentManager = new ResourceContentManager(servicesProvider, TextBatchResources.ResourceManager))
				using (var effect = contentManager.Load<Effect>("DefaultTextEffect"))
				using (var textBatch = new TextBatch(mockedGraphicsDeviceService.GraphicsDevice, effect))
				{
					TestText test = new TestText();
					try
					{
						textBatch.Begin();
						try
						{
							textBatch.DrawText(test, effect);
						}
						finally
						{
							textBatch.End();
						}
					}
					finally
					{
						effect.Dispose();
					}
				}
			}

		}
	}

} // namespace Nuclex.Fonts

#endif // UNITTEST
