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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclectic.Graphics.Helpers;
using Nuclectic.Graphics.TriD;
using Nuclectic.Graphics.TriD.SpecialEffects.Water;
using Nuclectic.Tests.Mocks;
#if UNITTEST
using NUnit.Framework;

namespace Nuclectic.Tests.Graphics.SpecialEffects.Water
{

	/// <summary>Unit tests for the grid class</summary>
	[TestFixture(IgnoreReason = "Unstable, may freeze test runner.")]
	[RequiresSTA]
	internal class WaterSurfaceTest
		: TestFixtureBase
	{
		/// <summary>
		///   Verifies that the simple constructor of the Grid class is working
		/// </summary>
		[Test]
		public void TestSimpleConstructor()
		{
			using (var mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			using (
			  WaterSurface surface = new WaterSurface(
				mockedGraphicsDeviceService.GraphicsDevice,
				new Vector2(-10.0f, -10.0f), new Vector2(10.0f, 10.0f)
			  )
			)
			{
				Assert.IsNotNull(surface); // Nonsense; avoids compiler warning
			}
		}

		/// <summary>
		///   Verifies that the complete constructor of the Grid class is working
		/// </summary>
		[Test]
		public void TestFullConstructor()
		{
			using (var mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			using (
			  WaterSurface surface = new WaterSurface(
				mockedGraphicsDeviceService.GraphicsDevice,
				new Vector2(-10.0f, -10.0f), new Vector2(10.0f, 10.0f),
				4, 4
			  )
			)
			{
				Assert.IsNotNull(surface); // Nonsense; avoids compiler warning
			}
		}

		/// <summary>
		///   Tests whether the water surface can select its index and vertex buffers
		/// </summary>
		[Test]
		public void TestSelectIndexAndVertexBuffer()
		{
			using (var mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			using (
			  WaterSurface surface = new WaterSurface(
				mockedGraphicsDeviceService.GraphicsDevice,
				new Vector2(-10.0f, -10.0f), new Vector2(10.0f, 10.0f),
				4, 4
			  )
			)
			{
				GraphicsDevice graphicsDevice = mockedGraphicsDeviceService.GraphicsDevice;
				graphicsDevice.Indices = null;
				graphicsDevice.SetVertexBuffer(null);

				Assert.IsNull(graphicsDevice.Indices);
				Assert.Inconclusive("MonoGame GraphicsDevice.GetVertexBuffers() is missing");
				//Assert.AreEqual(0, graphicsDevice.GetVertexBuffers().Length);

				surface.SelectVertexAndIndexBuffer();

				Assert.IsNotNull(graphicsDevice.Indices);
				Assert.Inconclusive("MonoGame GraphicsDevice.GetVertexBuffers() is missing");
				//Assert.IsNotNull(graphicsDevice.GetVertexBuffers()[0].VertexBuffer);
			}
		}

		/// <summary>
		///   Tests whether the water surface can draw its water plane
		/// </summary>
		[Test]
		public void TestDrawWaterPlane()
		{
			using (var mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			using (var contentManager = mockedGraphicsDeviceService.CreateResourceContentManager(Resources.UnitTestResources.ResourceManager))
			{
				Effect waterSurfaceEffect = contentManager.Load<Effect>(
																			 "WaterSurfaceEffect"
					);

				using (
					WaterSurface surface = new WaterSurface(
						mockedGraphicsDeviceService.GraphicsDevice,
						new Vector2(-10.0f, -10.0f), new Vector2(10.0f, 10.0f),
						4, 4
						)
					)
				{
					surface.SelectVertexAndIndexBuffer();

					EffectTechnique technique = waterSurfaceEffect.CurrentTechnique;
					for (int pass = 0; pass < technique.Passes.Count; ++pass)
					{
						technique.Passes[pass].Apply();

						surface.DrawWaterPlane(new GameTime(), Camera.CreateDefaultOrthographic());
					}
				}
			}
		}

		/// <summary>
		///   Tests whether the water surface can update its reflection texture
		/// </summary>
		[Test]
		public void TestUpdateReflection()
		{
			using (var mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			using (
			  WaterSurface surface = new WaterSurface(
				mockedGraphicsDeviceService.GraphicsDevice,
				new Vector2(-10.0f, -10.0f), new Vector2(10.0f, 10.0f),
				4, 4
			  )
			)
			{
				surface.UpdateReflection(
				  new GameTime(), Camera.CreateDefaultOrthographic(),
				  new WaterSurface.SceneDrawDelegate(drawReflection)
				);

				Assert.IsNotNull(surface.ReflectionCamera);
				Assert.IsNotNull(surface.ReflectionTexture);
			}
		}

		/// <summary>
		///   Verifies that the water surface can survive a graphics device reset
		/// </summary>
		[Test]
		public void TestGraphicsDeviceReset()
		{
			using (var mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			using (
			  WaterSurface surface = new WaterSurface(
				mockedGraphicsDeviceService.GraphicsDevice,
				new Vector2(-10.0f, -10.0f), new Vector2(10.0f, 10.0f),
				4, 4
			  )
			)
			{
				mockedGraphicsDeviceService.ResetDevice();

				surface.UpdateReflection(
				  new GameTime(), Camera.CreateDefaultOrthographic(),
				  new WaterSurface.SceneDrawDelegate(drawReflection)
				);

				Assert.IsNotNull(surface.ReflectionCamera);
				Assert.IsNotNull(surface.ReflectionTexture);
			}
		}

		/// <summary>Dummy that's supposed to draw the water's reflection</summary>
		/// <param name="gameTime">Snapshot of the game's timing values</param>
		/// <param name="camera">Camera containing the viewer's position</param>
		private void drawReflection(GameTime gameTime, ICamera camera) { }
	}

} // namespace Nuclex.Graphics.SpecialEffects.Water

#endif // UNITTEST
