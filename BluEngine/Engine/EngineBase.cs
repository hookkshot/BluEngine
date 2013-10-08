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
        private RenderTarget2D _depthMapRenderTarget;
        private RenderTarget2D _normalMapRenderTarget;
        private RenderTarget2D _shadowMapRenderTarget;

        private Texture2D _colorMapTexture;
        private Texture2D _depthMapTexture;
        private Texture2D _normalMapTexture;
        private Texture2D _shadowMapTexture;

        private VertexDeclaration _vertexDeclaration;
        private VertexPositionTexture[] _vertices;

        private Effect _lightEffect1;
        private Effect _lightEffect2;

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
            _depthMapRenderTarget = new RenderTarget2D(Screenmanager.GraphicsDevice, width, height, true, format, pp.DepthStencilFormat);
            _normalMapRenderTarget = new RenderTarget2D(Screenmanager.GraphicsDevice, width, height, true, format, pp.DepthStencilFormat);
            _shadowMapRenderTarget = new RenderTarget2D(Screenmanager.GraphicsDevice, width, height, true, format, pp.DepthStencilFormat);

            //_lightEffect1 = Content.Load<Effect>("ShadersLightingShadow");
            //_lightEffect2 = Content.Load<Effect>("ShadersLightingCombined");


            _vertices = new VertexPositionTexture[4];
            _vertices[0] = new VertexPositionTexture(new Vector3(-1,1,0), new Vector2(0,0));
            _vertices[1] = new VertexPositionTexture(new Vector3(1,1,0), new Vector2(1,0));
            _vertices[2] = new VertexPositionTexture(new Vector3(-1,-1,0), new Vector2(0,1));
            _vertices[3] = new VertexPositionTexture(new Vector3(1,-1,0), new Vector2(1,1));
            //_vertexDeclaration = new VertexDeclaration(Screenmanager.GraphicsDevice, VertexPositionTexture.VertexDeclaration);
 
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
            GraphicsDevice gd = Screenmanager.GraphicsDevice;

            gd.SetRenderTarget(_colorMapRenderTarget);

            gd.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1, 0);

            foreach (GameObject item in gameObjects)
            {
                item.Draw(spriteBatch, Vector2.Zero);
            }

            gd.SetRenderTarget(null);

            _colorMapTexture = _colorMapRenderTarget;

            spriteBatch.Draw(_colorMapTexture, new Rectangle(0,0,gd.Viewport.Width,gd.Viewport.Height), Color.White); 
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
