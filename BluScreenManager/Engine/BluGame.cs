using System;
using System.Collections.Generic;
using AurelienRibon.TweenEngine;
using Microsoft.Xna.Framework;


namespace BluEngine.Engine
{
	public class BluGame : Game
	{
        private GraphicsDeviceManager graphics;
        private BluGameInitSettings initSettings = null;

        public String ContentRoot { get { return initSettings.ContentFolder; } }
        public String FontsPath { get { return initSettings.FontsFolder + "\\"; } }
        public String StylesPath { get { return initSettings.StylesFolder + "\\"; } }
        public String TexturesPath { get { return initSettings.TexturesFolder + "\\"; } }
        public String SoundsPath { get { return initSettings.SoundsFolder + "\\"; } }
        public String MusicPath { get { return initSettings.MusicFolder + "\\"; } }
        public String ShadersPath { get { return initSettings.ShadersFolder + "\\"; } }
        
        /// <summary>
        /// ScreenManager instance in use by this BluGame.
        /// </summary>
        public BluEngine.ScreenManager.ScreenManager ScreenManager
        {
            get { return screenManager; }
        }
        private BluEngine.ScreenManager.ScreenManager screenManager;

        /// <summary>
        /// Create a new instance of BluGame.
        /// </summary>
        /// <param name="settings">A settings blob to pass. Passing null will assume defaults.</param>
        public BluGame(BluGameInitSettings settings)
            : base()
        {
            initSettings = settings ?? new BluGameInitSettings();

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = initSettings.ScreenWidth;
            graphics.PreferredBackBufferHeight = initSettings.ScreenHeight;
            graphics.IsFullScreen = initSettings.IsFullscreen;
            Content.RootDirectory = ContentRoot;
        }

        /// <summary>
        /// Create a new instance of BluGame with default settings.
        /// </summary>
        public BluGame() : this(null) { }

        protected override void Initialize()
        {
            base.Initialize();
            IsMouseVisible = initSettings.IsMouseVisible;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Tween.SetCombinedAttributesLimit(initSettings.TweenAttributeLimit);
            if (initSettings.TweenAccessors != null)
            {
                foreach (KeyValuePair<Type, TweenAccessor> kvp in initSettings.TweenAccessors)
                    Tween.RegisterAccessor(kvp.Key, kvp.Value);
            }
            screenManager = new BluEngine.ScreenManager.ScreenManager(this, FontsPath+initSettings.DefaultFont);
            screenManager.Initialize();
            screenManager.AddScreen(initSettings.FirstScreenType); //won't do anything if firstScreen is null or invalid :)
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            screenManager.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(initSettings.GraphicsClearColor);
            base.Draw(gameTime);
            screenManager.Draw(gameTime);
        }
	}
}
