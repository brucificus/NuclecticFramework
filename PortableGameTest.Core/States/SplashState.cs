using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac.Features.OwnedInstances;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Game.States;
using Nuclex.Input;
using PortableGameTest.Framework.Support;

namespace PortableGameTest.Core
{
    public class SplashState
        : Nuclex.Game.States.DrawableGameState
    {
        private readonly GraphicsDevice _GraphicsDevice;
        private readonly IInputService _InputService;
	    private readonly SpriteFont _LucidaSpriteFont;
	    private readonly SpriteBatch _SpriteBatch;
	    private readonly List<Tuple<Func<bool>, string>> _Tests;
	    private static readonly Color _TestsColorBackground = Color.CornflowerBlue;
	    private static readonly Color _TestsColorForegroundFound = Color.Black;
	    private static readonly Color _TestsColorForegroundNotFound = Color.DarkRed;

	    public SplashState(GraphicsDevice graphicsDevice, IInputService inputService, [Named("LucidaSpriteFont")] SpriteFont lucidaSpriteFont, SpriteBatch spriteBatch)
        {
            _GraphicsDevice = graphicsDevice;
            _InputService = inputService;
	        _LucidaSpriteFont = lucidaSpriteFont;
		    _SpriteBatch = spriteBatch;

		    _Tests = new List<Tuple<Func<bool>, string>>()
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
            //throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
	        const double branchDurationSeconds = 0.6;
            var branch = (int)((gameTime.TotalGameTime.TotalSeconds / branchDurationSeconds) % _Tests.Count);
			_GraphicsDevice.Clear(_TestsColorBackground);
	        _SpriteBatch.Begin();
	        var test = _Tests[branch];
			if (test.Item1())
	        {
				_SpriteBatch.DrawString(_LucidaSpriteFont, "Detected: " + test.Item2, new Vector2(0,0), _TestsColorForegroundFound);
	        }
			else
			{
				_SpriteBatch.DrawString(_LucidaSpriteFont, test.Item2 + " NOT FOUND!!", new Vector2(0,0), _TestsColorForegroundNotFound);
			}
	        _SpriteBatch.End();
        }
    }
}
