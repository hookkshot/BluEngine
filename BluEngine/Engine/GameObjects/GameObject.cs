using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using BluHelper;

namespace BluEngine.Engine.GameObjects
{
    public class GameObject
    {
        #region Fields

        private bool active = true;
        private Dictionary<Type, GameObjectComponent> components;

        #endregion

        #region Properties

        /// <summary>
        /// The game world position of the game object.
        /// </summary>
        public virtual Vector2 Position
        {
            get { return ((Transform)this[typeof(Transform)]).Position; }
            set { ((Transform)this[typeof(Transform)]).Position = value; }
        }

        /// <summary>
        /// The game world scale of the game object.
        /// </summary>
        public virtual float Scale
        {
            get { return ((Transform)this[typeof(Transform)]).Scale; }
            set { ((Transform)this[typeof(Transform)]).Scale = value; }
        }

        /// <summary>
        /// The game world rotation of the game object.
        /// </summary>
        public virtual float Rotation
        {
            get { return ((Transform)this[typeof(Transform)]).Rotation; }
            set { ((Transform)this[typeof(Transform)]).Rotation = value; }
        }

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }


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
                value.ConnectedGameObject = this;
            }
        }

        public GameObject()
        {
            components = new Dictionary<Type, GameObjectComponent>();
            components[typeof(Transform)] = new Transform();
        }

        #endregion

        #region Initialize

        public virtual void Initialize()
        {

        }

        public virtual void Initialize(ContentManager content, string path)
        {
            foreach (KeyValuePair<Type, GameObjectComponent> kvp in components)
            {
                kvp.Value.Initialize(content,path);
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
                if (kvp.Value == null)
                {
                    Console.WriteLine("Warning: GameObject.Update() - GameObjectComponent[" + kvp.Key.Name+ "] is null!");
                    continue;
                }
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
                if (kvp.Value == null)
                {
                    Console.WriteLine("Warning: GameObject.Draw() - GameObjectComponent[" + kvp.Key.Name + "] is null!");
                    continue;
                }
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
