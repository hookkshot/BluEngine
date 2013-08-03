using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BluEngine;
using BluEngine.ScreenManager;
using BluEngine.ScreenManager.Widgets;
using BluEngine.TweenAccessors;
using AurelienRibon.TweenEngine;


namespace BluEngine.Engine
{
	public class BluGame : Game
	{
        private GraphicsDeviceManager graphics;
        private Type firstScreen = null;

        /// <summary>
        /// ScreenManager instance in use by this BluGame.
        /// </summary>
        public BluEngine.ScreenManager.ScreenManager ScreenManager
        {
            get { return screenManager; }
        }
        private BluEngine.ScreenManager.ScreenManager screenManager;

        public BluGame(Type firstScreen) : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
            this.firstScreen = firstScreen;
        }
        public BluGame() : this(null) { }

        protected override void Initialize()
        {
            base.Initialize();
            this.IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            RegisterTweenAccessors();
            screenManager = new BluEngine.ScreenManager.ScreenManager(this, "Fonts\\smallfont");
            screenManager.Initialize();
            if (firstScreen != null)
                screenManager.AddScreen((GameScreen)firstScreen.GetConstructor(new Type[] {}).Invoke(new object[] {}));
        }

        protected virtual void RegisterTweenAccessors()
        {
            Tween.RegisterAccessor(typeof(Widget), new WidgetAccessor());
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            screenManager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
            screenManager.Draw(gameTime);
        }
	}
}
