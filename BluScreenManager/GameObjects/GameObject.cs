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

        protected Vector2 position = Vector2.Zero;
        protected List<GameObjectComponent> components = new List<GameObjectComponent>();

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

        /// <summary>
        /// Returns list of all attached Game Object components.
        /// </summary>
        public List<GameObjectComponent> Components
        {
            get { return components; }
        }

        #endregion

        #region Update

        /// <summary>
        /// Allows logic to be processed and updated by the game object.
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            foreach (GameObjectComponent comp in components)
            {
                comp.Update(gameTime);
            }
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
            foreach (GameObjectComponent comp in components)
            {
                comp.Draw(spriteBatch, screenOffset);
            }
        }

        #endregion

        #region Methods

        public virtual GameObject Clone()
        {
            return new GameObject()
            {
                Position = this.Position
            };
        }

        #endregion
    }
}
