using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PortableGameTest.Framework.Geometry
{
	public static class PointExtensions
	{
		public static Vector2 ToVector2(this Point self)
		{
			return new Vector2(self.X, self.Y);
		}
	}
}
