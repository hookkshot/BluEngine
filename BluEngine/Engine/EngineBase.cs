using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using BluEngine.ScreenManager;
using BluEngine.Engine.GameObjects;

namespace BluEngine.Engine
{
    public class Engine
    {
        #region Fields

        public Microsoft.Xna.Framework.Content.ContentManager Content
        {
            get { return content; }
            set { content = value; }
        }
        private Microsoft.Xna.Framework.Content.ContentManager content;

        protected BluEngine.ScreenManager.ScreenManager Screenmanager
        {
            get { return BluEngine.ScreenManager.ScreenManager.Instance; }
        }

        /// <summary>
        /// Run collision for game objects.
        /// </summary>
        protected CollisionSimulator CollisionSimulator
        {
            get { return CollisionSimulator.Instance; }
        }

        /// <summary>
        /// Use this class for generatin random variables.
        /// </summary>
        protected Random Random
        {
            get { return random; }
        }
        private Random random;


        protected List<GameObject> GameObjects
        {
            get { return gameObjects; }
        }
        private List<GameObject> gameObjects = new List<GameObject>();

        //Render Targets
        private RenderTarget2D _colorMapRenderTarget;

        private ViewScreen viewScreen;

        /// <summary>
        /// The viewscreen showing the gameworld
        /// </summary>
        public ViewScreen ViewScreen
        {
            get { return viewScreen; }
            set { viewScreen = value; }
        }

        #endregion

        #region Initialize

        public Engine()
        {
        }

        public virtual void LoadContent()
        {
            _colorMapRenderTarget = NewScreenRenderTarget();
        }

        #endregion

        #region Update

        public virtual void Update(GameTime gameTime)
        {
            foreach (GameObject item in gameObjects)
            {
                item.Update(gameTime);
            }
        }

        #endregion

        #region Draw

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Create image to draw to screen
            #region Base Color Map
            GraphicsDevice gd = Screenmanager.GraphicsDevice;

            gd.SetRenderTarget(_colorMapRenderTarget);

            gd.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1, 0);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);

            foreach (GameObject item in gameObjects)
            {
                item.Draw(spriteBatch, Vector2.Zero);
            }

            spriteBatch.End();

            
            #endregion

            Screenmanager.GraphicsDevice.SetRenderTarget(null);
            Screenmanager.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(PostProcess(_colorMapRenderTarget,spriteBatch,gd), Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        public virtual Texture2D PostProcess(Texture2D screenTexture, SpriteBatch spriteBatch, GraphicsDevice gd)
        {
            return screenTexture;
        }

        #endregion

        #region KeyEvents

        public virtual void KeyDown(Keys key)
        {
        }

        public virtual void KeyUp(Keys key)
        {
        }

        #endregion

        #region Methods

        protected RenderTarget2D NewScreenRenderTarget()
        {
            PresentationParameters pp = Screenmanager.GraphicsDevice.PresentationParameters;
            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;
            SurfaceFormat format = pp.BackBufferFormat;

            return new RenderTarget2D(Screenmanager.GraphicsDevice, width, height);
        }

        #endregion
    }
}
