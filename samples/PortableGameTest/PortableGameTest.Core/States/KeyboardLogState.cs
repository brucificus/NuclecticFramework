using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nuclectic.Game.State;
using Nuclectic.Geometry;
using Nuclectic.Input;
using Nuclectic.Input.Devices;
using PortableGameTest.Core.Graphics;
using PortableGameTest.Framework.Support;

namespace PortableGameTest.Core.States
{
	public class KeyboardLogState
		: DrawableGameState, IDisposable
	{
		private readonly IInputService _InputService;
		private readonly SpriteFont _LucidaSpriteFont;
		private readonly SpriteBatch _SpriteBatch;
		private readonly IKeyboard _Keyboard;
		private ImmutableList<string> _LogLinesPrevious; 
		private string _LogLineCurrent;
		private static readonly Color _ColorBackground = Color.CornflowerBlue;
		private static readonly Color _ColorText = Color.Black;

		public KeyboardLogState(IInputService inputService, [Named("LucidaSpriteFont")] SpriteFont lucidaSpriteFont,
			SpriteBatch spriteBatch)
		{
			_InputService = inputService;
			_LucidaSpriteFont = lucidaSpriteFont;
			_SpriteBatch = spriteBatch;

			_LogLinesPrevious = ImmutableList<string>.Empty.Add("Keyboard Log");
			_LogLineCurrent = "";
			_Keyboard = _InputService.GetKeyboard();
			KeyboardEventsSubscribe();
		}

		public override void Update(GameTime gameTime)
		{
			_InputService.Update();
		}

		public override void Draw(GameTime gameTime)
		{
			_SpriteBatch.GraphicsDevice.Clear(_ColorBackground);
			_SpriteBatch.Begin();

			AdvanceLog(gameTime);
			var logText = ConcatenateText(_LogLinesPrevious);

			var screenCenter = _SpriteBatch.GraphicsDevice.Viewport.Bounds.Center.ToVector2();
			_SpriteBatch.DrawStringCentered(_LucidaSpriteFont, logText, screenCenter, _ColorText);

			_SpriteBatch.End();
		}

		public void Dispose()
		{
			KeyboardEventsUnsubscribe();
		}

		private void KeyboardEventsSubscribe()
		{
			_Keyboard.CharacterEntered += KeyboardOnCharacterEntered;
			_Keyboard.KeyPressed += KeyboardOnKeyPressed;
			_Keyboard.KeyReleased += KeyboardOnKeyReleased;
		}

		private void KeyboardEventsUnsubscribe()
		{
			_Keyboard.CharacterEntered -= KeyboardOnCharacterEntered;
			_Keyboard.KeyPressed -= KeyboardOnKeyPressed;
			_Keyboard.KeyReleased -= KeyboardOnKeyReleased;
		}

		private void KeyboardOnKeyReleased(Keys key)
		{
			AppendToLogLineCurrent("-" + key);
		}

		private void KeyboardOnKeyPressed(Keys key)
		{
			AppendToLogLineCurrent("+" + key);
		}

		private void KeyboardOnCharacterEntered(char character)
		{
			AppendToLogLineCurrent("='" + character + "'");
		}

		private void AppendToLogLineCurrent(string text)
		{
			if (string.IsNullOrEmpty(_LogLineCurrent))
				_LogLineCurrent = text;
			else
				_LogLineCurrent += " " + text;
		}

		private void AdvanceLog(GameTime currentGameTime)
		{
			if (string.IsNullOrWhiteSpace(_LogLineCurrent))
				return;
			var logLine = currentGameTime.TotalGameTime.TotalMilliseconds + " " + _LogLineCurrent;
			_LogLineCurrent = string.Empty;

			_LogLinesPrevious = _LogLinesPrevious.Add(logLine);

			var linesMeasurement = MeasureText(_LogLinesPrevious);
			while (linesMeasurement.Y > _SpriteBatch.GraphicsDevice.Viewport.Height)
			{
				_LogLinesPrevious = _LogLinesPrevious.RemoveAt(0);
				linesMeasurement = MeasureText(_LogLinesPrevious);
			}
		}

		private Vector2 MeasureText(IEnumerable<string> textLines)
		{
			var resultText = ConcatenateText(textLines);
			return _LucidaSpriteFont.MeasureString(resultText);
		}

		private static string ConcatenateText(IEnumerable<string> textLines)
		{
			var sb = new StringBuilder();
			foreach (var line in textLines)
			{
				if (sb.Length > 0)
					sb.AppendLine();
				sb.Append(line);
			}
			var resultText = sb.ToString();
			sb.Clear();
			return resultText;
		}
	}
}
