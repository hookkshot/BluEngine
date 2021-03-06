﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.Engine.GameObjects
{
    public abstract class GameObjectComponent
    {
        #region Fields

        protected GameObject connectedGameObject = null;

        #endregion

        #region Properties

        public virtual GameObject ConnectedGameObject
        {
            get { return connectedGameObject; }
            set { connectedGameObject = value; }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Sets up the component if it needs extra work after being loaded.
        /// </summary>
        /// <param name="path">The path that all your game content is placed</param>
        public virtual void Initialize(ContentManager content, string path)
        {

        }

        #endregion

        #region Update

        public virtual void Update(GameTime gameTime)
        {
        }

        #endregion

        #region Draw

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 screenOffset)
        {
        }

        #endregion

        #region Methods

        public virtual void ConnectedGameObjectApply()
        {
        }

        #endregion
    }
}
