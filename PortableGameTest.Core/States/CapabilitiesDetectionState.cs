using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclectic.Geometry;
using Nuclectic.Input.Abstractions;
using Nuclex.Input;
using PortableGameTest.Core.Graphics;
using PortableGameTest.Framework.Support;

namespace PortableGameTest.Core.States
{
	public class CapabilitiesDetectionState
       : Nuclex.Game.States.DrawableGameState
    {
        private readonly IInputService _InputService;
	    private readonly SpriteFont _LucidaSpriteFont;
	    private readonly SpriteBatch _SpriteBatch;
	    private readonly List<Tuple<Func<bool>, string>> _InputPresenceTests;
	    private static readonly Color _ColorBackground = Color.CornflowerBlue;
		private static readonly Color _ColorText = Color.Black;

		public CapabilitiesDetectionState(IInputService inputService, [Named("LucidaSpriteFont")] SpriteFont lucidaSpriteFont, SpriteBatch spriteBatch)
        {
            _InputService = inputService;
	        _LucidaSpriteFont = lucidaSpriteFont;
		    _SpriteBatch = spriteBatch;

		    _InputPresenceTests = new List<Tuple<Func<bool>, string>>()
			{
				new Tuple<Func<bool>, string>(()=>_InputService.GetKeyboard().IsAttached, "Keyboard"),
				new Tuple<Func<bool>, string>(()=>_InputService.GetGamePad(ExtendedPlayerIndex.One).IsAttached, "GamePad One"),
				new Tuple<Func<bool>, string>(()=>_InputService.GetGamePad(ExtendedPlayerIndex.Five).IsAttached, "GamePad Five"),
				new Tuple<Func<bool>, string>(()=>_InputService.GetMouse().IsAttached, "Mouse"),
				new Tuple<Func<bool>, string>(()=>_InputService.GetTouchPanel().IsAttached, "TouchPanel")
			};
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
			testResults.AppendLine("Capabilities Detection");

	        TestInputs(testResults);
	        
			testResults.AppendLine();

	        var currentDisplayMode = _SpriteBatch.GraphicsDevice.Adapter.CurrentDisplayMode;
			testResults.AppendFormat("Graphics Device: {0}x{1} @ {2}hz ({3},{4} - {5},{6} safe)", currentDisplayMode.Width, currentDisplayMode.Height, currentDisplayMode.RefreshRate, currentDisplayMode.TitleSafeArea.Left, currentDisplayMode.TitleSafeArea.Top, currentDisplayMode.TitleSafeArea.Right, currentDisplayMode.TitleSafeArea.Bottom);
	        testResults.AppendLine();
			var viewport = _SpriteBatch.GraphicsDevice.Viewport;
			testResults.AppendFormat("Viewport: {0}x{1} ({2},{3} - {4},{5}) ({6},{7} - {8},{9} safe)", viewport.Width, viewport.Height, viewport.Bounds.Left, viewport.Bounds.Top, viewport.Bounds.Right, viewport.Bounds.Bottom , viewport.TitleSafeArea.Left, viewport.TitleSafeArea.Top, viewport.TitleSafeArea.Right, viewport.TitleSafeArea.Bottom); ;

			var screenCenter = _SpriteBatch.GraphicsDevice.Viewport.Bounds.Center.ToVector2();
			_SpriteBatch.DrawStringCentered(_LucidaSpriteFont, testResults.ToString(), screenCenter, _ColorText);

	        _SpriteBatch.End();
        }

		private void TestInputs(StringBuilder testResults)
		{
			foreach (var test in _InputPresenceTests)
			{
				if (testResults.Length > 0)
					testResults.AppendLine();
				if (test.Item1())
				{
					testResults.AppendFormat("{0} Detected", test.Item2);
				}
				else
				{
					testResults.AppendFormat("{0} NOT FOUND", test.Item2);
				}
			}
			testResults.AppendLine();
		}
    }
}
