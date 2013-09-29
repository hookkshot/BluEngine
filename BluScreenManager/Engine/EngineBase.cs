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

        #endregion

        #region Initialize

        public Engine()
        {
        }

        public virtual void LoadContent()
        {

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
            foreach (GameObject item in gameObjects)
            {
                item.Draw(spriteBatch, Vector2.Zero);
            }
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
