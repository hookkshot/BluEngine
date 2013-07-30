using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine
{
    public class Container
    {
        public Container ParentContainer;
        protected Vector2 position = Vector2.Zero;

        public Vector2 Position
        {
            get
            {
                if (ParentContainer != null)
                    return ParentContainer.Position + position;
                return position;
            }
            set { position = value; }
        }

        public int Width = 0;
        public int Height = 0;

        public Color Color = Color.Blue;

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D tex)
        {
            spriteBatch.Draw(tex, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), Color);

        }
    }
}
