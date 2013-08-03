#region File Description
//File: Gamescreen.cs
//Date: 11/08/2012
//Version: 1.0
//
//Copyright (C) Blurift Entertainment
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Content;
using AurelienRibon.TweenEngine;
#endregion

namespace BluEngine
{
    /// <summary>
    /// Enum describes the screen transition state.
    /// </summary>
    public enum ScreenState
    {
        Active,
        Hidden,
    }

    /// <summary>
    /// A screen is a single layer that has update and draw logic, and which
    /// can be combined with other layers to build up a complex menu system.
    /// For instance the main menu, the options menu, the "are you sure you
    /// want to quit" message box, and the main game itself are all implemented
    /// as screens.
    /// </summary>
    public abstract class GameScreen
    {
        #region Properties

        /// <summary>
        /// TweenManager instance for animations, etc.
        /// </summary>
        public TweenManager TweenManager
        {
            get { return tweenManager; }
        }
        private TweenManager tweenManager;

        public float TweenSpeed
        {
            get { return tweenSpeed; }
            set { tweenSpeed = value < 0.1f || value > 5.0f ? 1.0f : value; }
        }
        private float tweenSpeed = 1.0f;

        public ContentManager Content
        {
            get { return content; }
        }
        private ContentManager content;

        /// <summary>
        /// Normally when one screen is brought up over the top of another,
        /// the first screen will transition off to make room for the new
        /// one. This property indicates whether the screen is only a small
        /// popup, in which case screens underneath it do not need to bother
        /// transitioning off.
        /// </summary>
        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }
        private bool isPopup = false;

        /// <summary>
        /// Gets the current screen transition state.
        /// </summary>
        public ScreenState ScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }
        private ScreenState screenState = ScreenState.Active;

        private bool otherScreenHasFocus;

        /// <summary>
        /// Gets the manager that this screen belongs to.
        /// </summary>
        public BluEngine.ScreenManager.ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }
        private BluEngine.ScreenManager.ScreenManager screenManager;

        #endregion

        #region Initialization

        /// <summary>
        /// Load graphics content for the screen.
        /// </summary>
        public virtual void LoadContent()
        {
            content = new ContentManager(ScreenManager.Game.Services);
            content.RootDirectory = "Content";
            tweenManager = new TweenManager();
        }


        /// <summary>
        /// Unload content for the screen.
        /// </summary>
        public virtual void UnloadContent()
        {
            tweenManager.KillAll();
            content.Dispose();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Allows the screen to run logic, such as updating the transition position.
        /// Unlike HandleInput, this method is called regardless of whether the screen
        /// is active, hidden, or in the middle of a transition.
        /// </summary>
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            tweenManager.Update((float)gameTime.ElapsedGameTime.TotalSeconds * tweenSpeed);
        }

        /// <summary>
        /// Allows the screen to handle user input. Unlike Update, this method
        /// is only called when the screen is active, and not when some other
        /// screen has taken the focus.
        /// </summary>
        public virtual void HandleInput(InputControl input) { }

        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        public virtual void Draw(GameTime gameTime) { }


        #endregion

        #region Public Methods


        /// <summary>
        /// Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which
        /// instantly kills the screen, this method respects the transition timings
        /// and will give the screen a chance to gradually transition off.
        /// </summary>
        public void ExitScreen()
        {
            ScreenManager.RemoveScreen(this);
        }


        #endregion
    }
}
