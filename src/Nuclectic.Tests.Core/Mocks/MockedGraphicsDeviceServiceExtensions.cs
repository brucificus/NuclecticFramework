using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Nuclectic.Graphics.Helpers;

namespace Nuclectic.Tests.Mocks
{
	public static class MockedGraphicsDeviceServiceExtensions
	{
		public static ResourceContentManager CreateResourceContentManager(this IMockedGraphicsDeviceService self, ResourceManager resourceManager)
		{
			var result = new ResourceContentManager(GraphicsDeviceServiceHelper.MakePrivateServiceProvider(self), Resources.UnitTestResources.ResourceManager);
			return result;
		}
	}
}