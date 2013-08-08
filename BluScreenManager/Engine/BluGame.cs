﻿using System;
using AurelienRibon.TweenEngine;
using BluEngine.ScreenManager.Screens;
using BluEngine.ScreenManager.Widgets;
using BluEngine.TweenAccessors;
using Microsoft.Xna.Framework;


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

        public BluGame(Type firstScreenType) : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
            firstScreen = firstScreenType;
        }
        public BluGame() : this(null) { }

        protected override void Initialize()
        {
            base.Initialize();
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            RegisterTweenAccessors();
            screenManager = new BluEngine.ScreenManager.ScreenManager(this, "Fonts\\smallfont");
            screenManager.Initialize();
            screenManager.AddScreen(firstScreen); //won't do anything if firstScreen is null or invalid :)
        }

        /// <summary>
        /// Add TweenAccessors you'd like to have available from game initialization in this function. It's mainly just to have a neat, logical grouping for them (as opposed to just having them listed in LoadContent()).
        /// </summary>
        private void RegisterTweenAccessors()
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
