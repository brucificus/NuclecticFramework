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

using Nuclectic.Fonts;
using Nuclectic.Fonts.Content;
using Nuclectic.Graphics.Helpers;
using Nuclectic.Tests.Mocks;
#if UNITTEST
using NUnit.Framework;

namespace Nuclectic.Tests.Fonts.Content
{
	/// <summary>Unit tests for the vector font reader</summary>
	[TestFixture]
	public class VectorFontReaderTest : TestFixtureBase
	{
		#region class MemoryContentManager

		#endregion // class MemoryContentManager

		/// <summary>
		///     Tests whether the constructor if the vector font reader is working
		/// </summary>
		[Test]
		public void TestConstructor()
		{
			VectorFontReader reader = new VectorFontReader();
			Assert.IsNotNull(reader); // nonsense; avoids compiler warning
		}

		/// <summary>Verifies that the vector font reader can load a vector font</summary>
		[Test]
		[RequiresSTA]
		public void TestVectorFontReading()
		{
			using (var service = PrepareGlobalExclusiveMockedGraphicsDeviceService())
			{
				using (var contentManager = service.CreateResourceContentManager(Resources.UnitTestResources.ResourceManager))
				{
					VectorFont font = contentManager.Load<VectorFont>("UnitTestVectorFont");
					Assert.IsNotNull(font);
				}
			}
		}
	}
} // namespace Nuclex.Fonts.Content

#endif // UNITTEST