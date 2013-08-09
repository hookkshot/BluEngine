#region File Description
//File: Gamescreen.cs
//Date: 11/08/2012
//Version: 1.0
//
//Copyright (C) Blurift Entertainment
#endregion

#region Using Statements
using AurelienRibon.TweenEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using BluEngine.Engine;
#endregion

namespace BluEngine.ScreenManager.Screens
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
        /// <summary>
        /// A pre-initialized random number generator shared between all screens.
        /// </summary>
        public static Random Rand
        {
            get { return random; }
        }
        private static Random random = new Random();
       
        #region Properties

        /// <summary>
        /// TweenManager instance for animations, etc.
        /// </summary>
        public TweenManager TweenManager
        {
            get { return tweenManager; }
        }
        private TweenManager tweenManager;

        /// <summary>
        /// The global speed multiplier of all Tweens used on this screen.
        /// </summary>
        public float TweenSpeed
        {
            get { return tweenSpeed; }
            set { tweenSpeed = value < 0.1f || value > 5.0f ? 1.0f : value; }
        }
        private float tweenSpeed = 1.0f;

        /// <summary>
        /// ContentManager for this screen's content.
        /// </summary>
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
            get { return ScreenManager.Instance; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Load graphics content for the screen.
        /// </summary>
        public virtual void LoadContent()
        {
            content = new ContentManager(ScreenManager.Game.Services);
            content.RootDirectory = ScreenManager.Game.ContentRoot;
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

        /// <summary>
        /// Called when this screen is added to the screen manager.
        /// </summary>
        public virtual void Added() { }

        /// <summary>
        /// Called when this screen is removed from the screen manager.
        /// </summary>
        public virtual void Removed() { }



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
        public virtual void Draw(GameTime gameTime)
        {
        
        }

        /// <summary>
        /// Use this for drawing debug information (toggled with the F5 key).
        /// </summary>
        /// <param name="gameTime">The gametime object passed in from Draw().</param>
        /// <param name="spriteBatch">The SpriteBatch object passed in from the ScreenManager.</param>
        /// <param name="stringBuilder">The StringBuilder object passed in from the ScreenManager that will be used to output a string after the chain of DrawDebug() calls has finished.</param>
        public virtual void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch, StringBuilder stringBuilder) { }


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
