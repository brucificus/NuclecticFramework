using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Input;

namespace PortableGameTest.Core
{
    public class SplashState
        : Nuclex.Game.States.DrawableGameState
    {
        private readonly GraphicsDevice _GraphicsDevice;
        private readonly IInputService _InputService;

        public SplashState(GraphicsDevice graphicsDevice, IInputService inputService)
        {
            _GraphicsDevice = graphicsDevice;
            _InputService = inputService;
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            var branch = (int)(gameTime.TotalGameTime.TotalSeconds % 6);
            if(branch == 0 && _InputService.GetKeyboard().IsAttached)
                _GraphicsDevice.Clear(Color.Khaki);
            else if(branch == 1 && _InputService.GetGamePad(ExtendedPlayerIndex.One).IsAttached)
                _GraphicsDevice.Clear(Color.Gold);
            else if(branch == 2 && _InputService.GetGamePad(ExtendedPlayerIndex.Five).IsAttached)
                _GraphicsDevice.Clear(Color.Goldenrod);
            else if(branch == 3 && _InputService.GetMouse().IsAttached)
                _GraphicsDevice.Clear(Color.Magenta);
            else if(branch == 5 && _InputService.GetTouchPanel().IsAttached)
                _GraphicsDevice.Clear(Color.Tomato);
            else
                _GraphicsDevice.Clear(Color.CornflowerBlue);
        }
    }
}
