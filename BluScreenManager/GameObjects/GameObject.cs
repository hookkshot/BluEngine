using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.GameObjects
{
    public class GameObject
    {
        #region Fields

        protected Vector2 position;

        #endregion

        #region Properties

        /// <summary>
        /// The game world position of the game object.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        #endregion

        #region Update

        /// <summary>
        /// Allows logic to be processed and updated by the game object.
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draw's this gameobject to the screen using the spriteBatch at the current position.
        /// </summary>
        /// <param name="spriteBatch">Allows the game object to add sprites to the spritebatch to be drawn.</param>
        /// <param name="screenOffset">The offset of the gamescreen to the game world.</param>
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 screenOffset)
        {
        }

        #endregion
    }
}
