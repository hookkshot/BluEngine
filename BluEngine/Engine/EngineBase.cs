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
            get { return Random; }
        }
        private Random random;


        protected List<GameObject> GameObjects
        {
            get { return gameObjects; }
        }
        private List<GameObject> gameObjects = new List<GameObject>();

        //Render Targets
        private RenderTarget2D _colorMapRenderTarget;
        private RenderTarget2D _lightMapRenderTarget;
        private RenderTarget2D _normalMapRenderTarget;

        private Effect _lightEffect;

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
            
            PresentationParameters pp = Screenmanager.GraphicsDevice.PresentationParameters;
            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;
            SurfaceFormat format = pp.BackBufferFormat;

            //_colorMapRenderTarget = new RenderTarget2D(Screenmanager.GraphicsDevice, width, height,true, format)
            _colorMapRenderTarget = new RenderTarget2D(Screenmanager.GraphicsDevice, width, height, true, format, pp.DepthStencilFormat);
            _lightMapRenderTarget = new RenderTarget2D(Screenmanager.GraphicsDevice, width, height, true, format, pp.DepthStencilFormat);

            _lightEffect = Content.Load<Effect>("BluEngine\\LightEffect");
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

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            foreach (GameObject item in gameObjects)
            {
                item.Draw(spriteBatch, Vector2.Zero);
            }

            spriteBatch.End();

            
            #endregion

            gd.SetRenderTarget(null);

            #region Light Map
            gd.SetRenderTarget(_lightMapRenderTarget);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            spriteBatch.End();
            #endregion

            gd.SetRenderTarget(_lightMapRenderTarget);
            gd.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            LightEngine.Instance.Draw(spriteBatch, ViewScreen.Position);
            spriteBatch.End();
            gd.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _lightEffect.Parameters["lightMask"].SetValue(_lightMapRenderTarget);
            _lightEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(_colorMapRenderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
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
    }
}
