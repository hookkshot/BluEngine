using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

using BluHelper;
using BluEngine.ScreenManager;
using BluEngine.Engine.GameObjects;

namespace BluEngine.Engine
{
    public class Engine
    {
        #region Fields

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

        public virtual void Update(InstanceTime gameTime)
        {

        }

        #endregion

        #region Draw

        public virtual void Draw(InstanceTime gameTime, SpriteBatch spriteBatch)
        {
        }

        #endregion
    }
}
