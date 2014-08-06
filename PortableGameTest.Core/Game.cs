using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Features.OwnedInstances;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Game.States;
using PortableGameTest.Framework.Game.States;

namespace PortableGameTest.Core
{
    public class Game<TGamePlatform>
        : Microsoft.Xna.Framework.Game
        where TGamePlatform : GamePlatformToken, new()
    {
        private readonly TGamePlatform _GamePlatform;
        private readonly ContainerBuilder _AutofacContainerBuilder;
        private readonly GraphicsDeviceManager _GraphicsDeviceManager;
        private IContainer _AutofacContainer;
        private Owned<IAutoGameStateService> _OwnedGameStateService;

	    public Game()
            : base()
        {
            _GamePlatform = new TGamePlatform();
            _AutofacContainerBuilder = new Autofac.ContainerBuilder();
            _GraphicsDeviceManager = new GraphicsDeviceManager(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _AutofacContainerBuilder.RegisterInstance(_GamePlatform).As<GamePlatformToken>().SingleInstance();
            _AutofacContainerBuilder.RegisterInstance(_GraphicsDeviceManager)
                .As<IGraphicsDeviceManager>()
                .As<IGraphicsDeviceService>()
                .ExternallyOwned()
                .SingleInstance();
            _AutofacContainerBuilder.RegisterInstance(this)
                .As<Game>()
                .ExternallyOwned()
                .SingleInstance();
            foreach (var moduleType in _GamePlatform.GetAutofacModuleTypes())
                _AutofacContainerBuilder.RegisterModule((IModule)Activator.CreateInstance(moduleType));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _AutofacContainer = _AutofacContainerBuilder.Build();
            _OwnedGameStateService = _AutofacContainer.Resolve<Owned<IAutoGameStateService>>();
	        _OwnedGameStateService.Value.Push<SplashState>(GameStateModality.Exclusive);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
	        try
	        {
		        _OwnedGameStateService.Dispose();
	        }
	        finally
	        {
		        _OwnedGameStateService = null;
	        }
	        try
	        {
		        _AutofacContainer.Dispose();
	        }
	        finally
	        {
				_AutofacContainer = null;
	        }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            _OwnedGameStateService.Value.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _OwnedGameStateService.Value.Draw(gameTime);
        }
    }
}
