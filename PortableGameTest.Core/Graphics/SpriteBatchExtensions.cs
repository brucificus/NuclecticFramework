using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PortableGameTest.Core.Graphics
{
	public static class SpriteBatchExtensions
	{
		public static void DrawStringCentered(this SpriteBatch self, SpriteFont font, string text, Vector2 position, Color color)
		{
			var totalSize = font.MeasureString(text);

			var newPosition = new Vector2(position.X - (totalSize.X/2f), position.Y - (totalSize.Y/2f));

			self.DrawString(font, text, newPosition, color);
		}
	}
}
