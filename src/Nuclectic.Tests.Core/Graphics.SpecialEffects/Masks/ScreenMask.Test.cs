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

using Microsoft.Xna.Framework.Content;
using Nuclectic.Graphics.Helpers;
using Nuclectic.Graphics.TriD.SpecialEffects.Masks;
using Nuclectic.Support;
using Nuclectic.Tests.Mocks;
using SlimDX.Direct3D9;
using Effect = Microsoft.Xna.Framework.Graphics.Effect;
#if UNITTEST
using System;
using NUnit.Framework;

namespace Nuclectic.Tests.Graphics.SpecialEffects.Masks
{
	/// <summary>Unit tests for the screen mask class</summary>
	[TestFixture(IgnoreReason = "Unstable, may freeze test runner.")]
	[RequiresSTA]
	internal class ScreenMaskTest
		: TestFixtureBase
	{
		/// <summary>
		///     Verifies that the constructor of the screen mask class is working
		/// </summary>
		[Test]
		public void TestConstructor()
		{
			using (MakeColorScreenMask())
			{
				// Do nothing
			}
		}

		/// <summary>Tests whether the screen mask is able to draw itself</summary>
		[Test]
		public void TestDraw()
		{
			using (var testMask = MakeColorScreenMask())
			{
				testMask.Draw();
			}
		}

		private static ScreenMask<PositionVertex> MakeColorScreenMask()
		{
			bool createSuccess = false;
			var service = PrepareGlobalExclusiveMockedGraphicsDeviceService();

			try
			{
				var serviceProvider = GraphicsDeviceServiceHelper.MakePrivateServiceProvider(service);
				var contentManager = new ResourceContentManager(serviceProvider, Resources.ScreenMaskResources.ResourceManager);

				try
				{
					var effect = contentManager.Load<Effect>("ScreenMaskEffect");

					try
					{
						// ReSharper disable AccessToDisposedClosure
						var result = new ScreenMask<PositionVertex>(
							service.GraphicsDevice, new Owned<Effect>(
														effect, () =>
														{
															effect.Dispose();
															contentManager.Dispose();
															service.Dispose();
														}), new PositionVertex[4]);
						// ReSharper restore AccessToDisposedClosure
						createSuccess = true;
						return result;
					}
					finally
					{
						if (!createSuccess)
							effect.Dispose();
					}
				}
				finally
				{
					if (!createSuccess)
						contentManager.Dispose();
				}
			}
			finally
			{
				if (!createSuccess)
					service.Dispose();
			}
		}
	}
} // namespace Nuclex.Graphics.SpecialEffects.Masks

#endif // UNITTEST