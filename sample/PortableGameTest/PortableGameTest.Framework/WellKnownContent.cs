using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PortableGameTest.Framework
{
	internal static class WellKnownContent
	{
		public static Effect ScreenMaskEffect(ContentManager contentManager)
		{
			return contentManager.Load<Effect>("ScreenMaskEffect");
		}

		public static Effect DefaultTextEffect(ContentManager contentManager)
		{
			return contentManager.Load<Effect>("DefaultTextEffect");
		}

		public static Effect SolidColorEffect(ContentManager contentManager)
		{
			return contentManager.Load<Effect>("SolidColorEffect");
		}

		public static SpriteFont LucidaSpriteFont(ContentManager contentManager)
		{
			return contentManager.Load<SpriteFont>("LucidaSpriteFont");
		}

		public static class Skins
		{
			public static class Suave
			{
				public static SpriteFont DefaultFont(ContentManager contentManager)
				{
					return contentManager.Load<SpriteFont>("Skins/Suave/DefaultFont");
				}

				public static Texture2D SuaveSheet(ContentManager contentManager)
				{
					return contentManager.Load<Texture2D>("Skins/Suave/SuaveSheet");
				}

				public static SpriteFont TitleFont(ContentManager contentManager)
				{
					return contentManager.Load<SpriteFont>("Skins/Suave/TitleFont");
				}
			}
		}
	}
}
