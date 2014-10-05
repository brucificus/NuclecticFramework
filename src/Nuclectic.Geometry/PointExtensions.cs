using Microsoft.Xna.Framework;

namespace Nuclectic.Geometry
{
	public static class PointExtensions
	{
		public static Vector2 ToVector2(this Point self)
		{
			return new Vector2(self.X, self.Y);
		}
	}
}
