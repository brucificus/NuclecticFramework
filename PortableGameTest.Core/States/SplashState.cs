using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PortableGameTest.Core
{
    public class SplashState
        : Nuclex.Game.States.DrawableGameState
    {
        private readonly GraphicsDevice _GraphicsDevice;

        public SplashState(GraphicsDevice graphicsDevice)
        {
            _GraphicsDevice = graphicsDevice;
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            _GraphicsDevice.Clear(Color.CornflowerBlue);
        }
    }
}
