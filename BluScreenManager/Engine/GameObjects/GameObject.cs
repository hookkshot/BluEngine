using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BluEngine.Engine.GameObjects
{
    public class GameObject
    {
        #region Fields

        private bool active = true;
        private Transform transform;
        private Dictionary<Type, GameObjectComponent> components;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public virtual Transform Transform
        {
            get { return transform; }
            protected set
            {
                if (value == null || value == transform)
                    return;
                transform = value;
                components[typeof(Transform)] = value;
            }
        }

        /// <summary>
        /// The game world position of the game object.
        /// </summary>
        public virtual Vector2 Position
        {
            get { return transform.Position; }
            set { transform.Position = value; }
        }

        /// <summary>
        /// The game world scale of the game object.
        /// </summary>
        public virtual float Scale
        {
            get { return transform.Scale; }
            set { transform.Scale = value; }
        }

        /// <summary>
        /// The game world rotation of the game object.
        /// </summary>
        public virtual float Rotation
        {
            get { return transform.Rotation; }
            set { transform.Rotation = value; }
        }

        public bool Active
        {
            get { return active; }
            set { active = value; }
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
                value.ConnectedGameObject = this;
            }
        }

        public GameObject()
        {
            transform = new Transform();
            components = new Dictionary<Type, GameObjectComponent>();
            components[typeof(Transform)] = transform;
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
