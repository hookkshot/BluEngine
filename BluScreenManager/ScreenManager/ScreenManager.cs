#region File Description
//File: ScreenManager.cs
//Date: 11/08/2012
//Version: 1.0
//
//Copyright (C) Blurift Entertainment
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using BluEngine.Engine;
using BluEngine.ScreenManager.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace BluEngine.ScreenManager
{
    /// <summary>
    /// The screen manager is a component which manages one or more GameScreen
    /// instances. It maintains a stack of screens, calls their Update and Draw
    /// methods at the appropriate times, and automatically routes input to the
    /// topmost active screen.
    /// </summary>
    /// 

    public class ScreenManager : DrawableGameComponent, IScreenDimensionsProvider
    {
        #region Fields

        public const string version = "BluScreenManager Version 1.0.3";
        private static ScreenManager instance = null;
        private List<GameScreen> screens = new List<GameScreen>();
        private List<GameScreen> screensToUpdate = new List<GameScreen>();
        private InputControl input = new InputControl();
        private string defaultFontLocation;
        private int soundLevel = 100;
        private int musicLevel = 100;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        Texture2D filler;
        private bool isInitialized;
        private bool traceEnabled;
        Vector2 mousePosition;

        #endregion

        #region Properties
        public Texture2D Filler { get { return filler; } }

        /// <summary>
        /// A static reference to the current ScreenManager instance.
        /// </summary>
        public static ScreenManager Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// A default SpriteBatch shared by all the screens. This saves
        /// each screen having to bother creating their own local instance.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public InputControl Input
        {
            get { return input; }
        }

        /// <summary>
        /// A default font shared by all the screens. This saves
        /// each screen having to bother loading their own local copy.
        /// </summary>
        public SpriteFont Font
        {
            get { return font; }
        }

        /// <summary>
        /// If true, the manager prints out a list of all the screens
        /// each time it is updated. This can be useful for making sure
        /// everything is being added and removed at the right times.
        /// </summary>
        public bool TraceEnabled
        {
            get { return traceEnabled; }
            set { traceEnabled = value; }
        }


        #endregion

        #region Initialization

        /// <summary>
        /// Constructs a new screen manager component.
        /// </summary>
        public ScreenManager(Game game, string fontLocation)
            : base(game)
        {
            if (instance == null)
                instance = this;
            
            this.defaultFontLocation = fontLocation;
        }


        /// <summary>
        /// Initializes the screen manager component.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            isInitialized = true;
            Console.WriteLine("Blu2D engine started. Version: " + version);
        }


        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            // Load content belonging to the screen manager.
            ContentManager content = Game.Content;
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            filler = new Texture2D(GraphicsDevice, 1, 1);
            filler.SetData(new Color[] { Color.White });

            font = content.Load<SpriteFont>(this.defaultFontLocation);

            // Tell each of the screens to load their content.
            foreach (GameScreen screen in screens)
                screen.LoadContent();
        }


        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Tell each of the screens to unload their content.
            foreach (GameScreen screen in screens)
                screen.UnloadContent();
            Console.WriteLine("Screen Unloaded");
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Read the keyboard and gamepad.
            input.Update(gameTime);

            mousePosition = new Vector2(input.MouseX(), input.MouseY());

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            screensToUpdate.AddRange(screens);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (screensToUpdate.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];

                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                // Update the screen.
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.Active)
                {
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(input);

                        otherScreenHasFocus = true;
                    }

                    if (!screen.IsPopup)
                    {
                        coveredByOtherScreen = true;
                    }
                        
                }
            }
        }

        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                //if (screen.ScreenState == ScreenState.Hidden)
                //    continue;

                screen.Draw(gameTime);
            }

        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;

            // If we have a graphics device, tell the screen to load content.
            if (isInitialized)
            {
                screen.LoadContent();

            }

            screens.Add(screen);

        }

        /// <summary>
        /// Adds a new screen to the screen manager by instantiating a new instance of the provided GameScreen type. The provided type must implement the parameterless constructor.
        /// </summary>
        public void AddScreen(Type screenType)
        {
            if (screenType == null)
                return;

            //check that the supplied type is valid
            if (screenType != typeof(GameScreen) && !screenType.IsSubclassOf(typeof(GameScreen)))
            {
                Console.WriteLine("Reflection error: Type supplied to BluGame xtor is not a valid GameScreen type!");
                return;
            }

            AddScreen((GameScreen)screenType.GetConstructor(new Type[] { }).Invoke(new object[] { }));
        }

        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }

        /// <summary>
        /// Removes a screen from the screen manager. You should normally
        /// use GameScreen.ExitScreen instead of calling this directly, so
        /// the screen can gradually transition off rather than just being
        /// instantly removed.
        /// </summary>
        public void RemoveScreen(GameScreen screen)
        {
            // If we have a graphics device, tell the screen to unload content.
            if (isInitialized)
                screen.UnloadContent();

            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }

        
        public void SaveData()
        {
            //StreamWriter stream = new StreamWriter("data.dat");
            //stream.WriteLine();
            //stream.Close();
        }

        #endregion

        public float ScreenX
        {
            get { return 0.0f; }
        }

        public float ScreenY
        {
            get { return 0.0f; }
        }

        public float ScreenWidth
        {
            get { return (float)Game.Window.ClientBounds.Width; }
        }

        public float ScreenHeight
        {
            get { return (float)Game.Window.ClientBounds.Height; }
        }

        public float ScreenRatio
        {
            get { return (float)Game.Window.ClientBounds.Width / (float)Game.Window.ClientBounds.Height; }
        }
    }
}
