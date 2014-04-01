using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Game.States;

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
        private IGameStateService _GameStateService;

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
            _GameStateService = _AutofacContainer.Resolve<Nuclex.Game.States.IGameStateService>();
            var splashState = _AutofacContainer.Resolve<SplashState>();
            _GameStateService.Push(splashState, GameStateModality.Exclusive);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            _AutofacContainer.Dispose();
            _AutofacContainer = null;
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

            _GameStateService.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _GameStateService.Draw(gameTime);
        }
    }
}
