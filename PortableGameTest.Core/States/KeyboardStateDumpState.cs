using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nuclectic.Geometry;
using Nuclex.Input;
using Nuclex.UserInterface;
using PortableGameTest.Core.Graphics;
using PortableGameTest.Framework.Support;

namespace PortableGameTest.Core.States
{
	public class KeyboardStateDumpState
		: Nuclex.Game.States.DrawableGameState
	{
		private readonly IInputService _InputService;
		private readonly SpriteFont _LucidaSpriteFont;
		private readonly SpriteBatch _SpriteBatch;
		private static readonly Color _ColorBackground = Color.CornflowerBlue;
		private static readonly Color _ColorText = Color.Black;

		public KeyboardStateDumpState(IInputService inputService, [Named("LucidaSpriteFont")] SpriteFont lucidaSpriteFont,
			SpriteBatch spriteBatch)
		{
			_InputService = inputService;
			_LucidaSpriteFont = lucidaSpriteFont;
			_SpriteBatch = spriteBatch;
		}

		public override void Update(GameTime gameTime)
		{
			_InputService.Update();
		}

		public override void Draw(GameTime gameTime)
		{
			_SpriteBatch.GraphicsDevice.Clear(_ColorBackground);
			_SpriteBatch.Begin();

			var testResults = new StringBuilder();

			testResults.AppendLine("KeyboardState Dump");
			testResults.AppendLine();

			var keyboard = _InputService.GetKeyboard();

			var keyboardState = keyboard.GetState();

			foreach (var key in keyboardState.GetPressedKeys())
			{
				if (testResults.Length > 0)
					testResults.Append(' ');
				testResults.Append(key);
			}

			var screenCenter = _SpriteBatch.GraphicsDevice.Viewport.Bounds.Center.ToVector2();
			_SpriteBatch.DrawStringCentered(_LucidaSpriteFont, testResults.ToString(), screenCenter, _ColorText);

			_SpriteBatch.End();
		}

	}
}