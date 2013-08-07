using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.Engine.GameObjects
{
    public class GameObject
    {
        #region Fields

        protected Vector2 position = Vector2.Zero;
        protected float rotation = 0.0f;
        protected float scale = 1.0f;

        private Dictionary<Type, GameObjectComponent> components = new Dictionary<Type, GameObjectComponent>();

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
        ///
        /// I'd recommend not leaving the actual data structure exposed as it will cause a bit of a security issue, since in doing that you can't police how the collection is accessed directly...
        /// </summary>
        //public Dictionary<Type, GameObjectComponent> Components
        //{
        //    get { return components; }
        //}

        /// <summary>
        /// The attached GameObjectComponent of the given Type.
        /// </summary>
        /// <param name="t">The type of the component to access (a subclass of GameObjectComponent).</param>
        /// <returns>The GameObjectComponent, or null.</returns>
        public GameObjectComponent this[Type t]
        {
            get
            {
                if (t == null || !t.IsSubclassOf(typeof(GameObjectComponent)))
                    return null;
                GameObjectComponent outValue = null;
                components.TryGetValue(t, out outValue);
                return outValue;
            }
            set
            {
                if (t == null || !t.IsSubclassOf(typeof(GameObjectComponent)))
                    return;
                components[t] = value;
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Allows logic to be processed and updated by the game object.
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            foreach (KeyValuePair<Type,GameObjectComponent> kvp in components)
            {
                kvp.Value.Update(gameTime);
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
            foreach (KeyValuePair<Type, GameObjectComponent> kvp in components)
            {
                kvp.Value.Draw(spriteBatch, screenOffset);
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
