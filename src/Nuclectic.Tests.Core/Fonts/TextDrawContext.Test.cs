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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclectic.Fonts;
using Nuclectic.Graphics.Helpers;
using Nuclectic.Graphics.TriD.Batching;
using Nuclectic.Tests.Mocks;
using Nuclectic.Tests.Resources;
#if UNITTEST
using NUnit.Framework;
using Text = Nuclectic.Fonts.Text;

namespace Nuclectic.Tests.Fonts
{
	/// <summary>Unit tests for the text drawing context</summary>
	[TestFixture(IgnoreReason = "Unreliable, sometimes fails to load requisite content.")]
	[RequiresSTA]
	public class TextDrawContextTest
		: TestFixtureBase
	{
		/// <summary>
		///     Verifies that two text contexts which should produce the exact same result
		///     compare as equal
		/// </summary>
		[Test]
		public void TestIdenticalEffectParameters()
		{
			using (var mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			{
				using (var contentManager = mockedGraphicsDeviceService.CreateResourceContentManager(TextBatchResources.ResourceManager))
				{
					using (var effect = contentManager.Load<Effect>("DefaultTextEffect"))
					{
						Matrix matrix = Matrix.Identity;

						TextDrawContext context1 = new TextDrawContext(effect, matrix, Color.White);
						TextDrawContext context2 = new TextDrawContext(effect, matrix, Color.White);

						Assert.IsTrue(context1.Equals(context2));
					}
				}
			}
		}

		/// <summary>
		///     Verifies that two text contexts with different matrices compare as inequal
		/// </summary>
		[Test]
		public void TestDifferentMatrices()
		{
			using (var mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			{
				using (var contentManager = mockedGraphicsDeviceService.CreateResourceContentManager(TextBatchResources.ResourceManager))
				{
					using (var effect = contentManager.Load<Effect>("DefaultTextEffect"))
					{
						Matrix matrix1 = Matrix.Identity;
						Matrix matrix2 = new Matrix(
							1.1f, 1.2f, 1.3f, 1.4f,
							2.1f, 2.2f, 2.3f, 2.4f,
							3.1f, 3.2f, 3.3f, 3.4f,
							4.1f, 4.2f, 4.3f, 4.4f
							);

						TextDrawContext context1 = new TextDrawContext(effect, matrix1, Color.White);
						TextDrawContext context2 = new TextDrawContext(effect, matrix2, Color.White);

						Assert.IsFalse(context1.Equals(context2));
					}
				}
			}
		}

		/// <summary>
		///     Verifies that two text contexts with different color compare as inequal
		/// </summary>
		[Test]
		public void TestDifferentColors()
		{
			using (var mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			{
				using (var contentManager = mockedGraphicsDeviceService.CreateResourceContentManager(TextBatchResources.ResourceManager))
				{
					using (var effect = contentManager.Load<Effect>("DefaultTextEffect"))
					{
						Matrix matrix = Matrix.Identity;

						TextDrawContext context1 = new TextDrawContext(effect, matrix, Color.Red);
						TextDrawContext context2 = new TextDrawContext(effect, matrix, Color.Black);

						Assert.IsFalse(context1.Equals(context2));
					}
				}
			}
		}

		/// <summary>
		///     Verifies that the text context can be compared against another context of
		///     a different type
		/// </summary>
		[Test]
		public void TestDifferentEffects()
		{
			using (var mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			{
				using (var contentManager = mockedGraphicsDeviceService.CreateResourceContentManager(TextBatchResources.ResourceManager))
				{
					using (var effect = contentManager.Load<Effect>("DefaultTextEffect"))
					{
						Matrix matrix = Matrix.Identity;
						TextDrawContext context1 = new TextDrawContext(effect, matrix, Color.White);
						using (BasicEffect effect2 = new BasicEffect(mockedGraphicsDeviceService.GraphicsDevice))
						{
							TextDrawContext context2 = new TextDrawContext(effect2, matrix, Color.White);
							Assert.IsFalse(context1.Equals(context2));
						}
					}
				}
			}
		}

		/// <summary>
		///     Verifies that the text context can be compared against another context of
		///     a different type
		/// </summary>
		[Test]
		public void TestDifferentContexts()
		{
			using (var mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			{
				using (var contentManager = mockedGraphicsDeviceService.CreateResourceContentManager(TextBatchResources.ResourceManager))
				{
					using (var effect = contentManager.Load<Effect>("DefaultTextEffect"))
					{
						Matrix matrix = Matrix.Identity;
						TextDrawContext context1 = new TextDrawContext(effect, matrix, Color.White);
						EffectDrawContext context2 = new EffectDrawContext(effect);

						Assert.IsFalse(context1.Equals(context2));
					}
				}
			}
		}

		/// <summary>
		///     Tests the Begin() and End() methods of the draw context without any rendering
		///     taking place inbetween them
		/// </summary>
		[Test]
		public void TestBeginEnd()
		{
			using (var mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			{
				using (var contentManager = mockedGraphicsDeviceService.CreateResourceContentManager(TextBatchResources.ResourceManager))
				{
					using (var effect = contentManager.Load<Effect>("DefaultTextEffect"))
					{
						Matrix matrix = Matrix.Identity;
						TextDrawContext context = new TextDrawContext(effect, matrix, Color.Red);
						for (int pass = 0; pass < context.Passes; ++pass)
						{
							context.Apply(pass);
						}
					}
				}
			}
		}
	}
} // namespace Nuclex.Fonts

#endif // UNITTEST