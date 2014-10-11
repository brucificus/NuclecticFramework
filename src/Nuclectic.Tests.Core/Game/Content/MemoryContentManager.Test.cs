#region CPL License

/*
Nuclex Framework
Copyright (C) 2002-2011 Nuclex Development Labs

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
using Nuclectic.Game.Content;
using Nuclectic.Tests.Mocks;
#if UNITTEST
using NUnit.Framework;

namespace Nuclectic.Tests.Game.Content
{
	/// <summary>Unit test for the embedded content manager class</summary>
	[TestFixture(IgnoreReason = "Unstable, may freeze test runner.")]
	[RequiresSTA]
	internal class MemoryContentManagerTest
		: TestFixtureBase
	{
		/// <summary>Tests the constructor of the embedded content manager</summary>
		[Test]
		public void TestConstructor() { Assert.IsNotNull(this.memoryContentManager); }

		/// <summary>Verifies that assets can be loaded</summary>
		[Test]
		public void TestLoadAsset()
		{
			Effect effect = this.memoryContentManager.Load<Effect>(
																   Resources.UnitTestResources.UnitTestEffect
				);
			Assert.IsNotNull(effect);
		}

		/// <summary>Verifies that uniquely named assets can be loaded</summary>
		[Test]
		public void TestLoadNamedAsset()
		{
			Effect effect = this.memoryContentManager.Load<Effect>(
																   Resources.UnitTestResources.UnitTestEffect, "UnitTestEffect"
				);
			Assert.IsNotNull(effect);
		}

		/// <summary>Verifies that the ReadAsset() method is working</summary>
		[Test]
		public void TestReadAsset()
		{
			Effect effect = this.memoryContentManager.ReadAsset<Effect>(
																	    Resources.UnitTestResources.UnitTestEffect
				);
			Assert.IsNotNull(effect);
		}

		/// <summary>Called before each test is run</summary>
		[SetUp]
		public void Setup()
		{
			this.mockedGraphicsDeviceService = PrepareGlobalExclusiveMockedGraphicsDeviceService();

			this.memoryContentManager = new MemoryContentManager(this.mockedGraphicsDeviceService);
		}

		/// <summary>Called after each test has run</summary>
		[TearDown]
		public void Teardown()
		{
			if (this.memoryContentManager != null)
			{
				this.memoryContentManager.Dispose();
				this.memoryContentManager = null;
			}
			if (this.mockedGraphicsDeviceService != null)
			{
				this.mockedGraphicsDeviceService.DestroyDevice();
				this.mockedGraphicsDeviceService = null;
			}
		}

		/// <summary>Mock of the graphics device service used for unit testing</summary>
		private IMockedGraphicsDeviceService mockedGraphicsDeviceService;

		/// <summary>Content manager which loads resources from in-memory arrays</summary>
		private MemoryContentManager memoryContentManager;
	}
} // namespace Nuclex.Game.Content

#endif // UNITTEST