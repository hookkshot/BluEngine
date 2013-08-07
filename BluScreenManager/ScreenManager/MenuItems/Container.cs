using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.ScreenManager.MenuItems
{
    public class Container
    {
        public Container ParentContainer;
        private Vector2 position;
        private int width = 0;
        private int height = 0;

        public Container(Container parent, Vector2 position, int width, int height)
        {
            ParentContainer = parent;
            Position = position;
            this.height = height;
            this.width = width;
        }
        public Container(Vector2 position) : this(null, position, 0, 0) { }
        public Container() : this(null, Vector2.Zero, 0, 0) { }

        public Vector2 Position
        {
            get
            {
                if (ParentContainer != null)
                    return position + ParentContainer.Position;
                return position;
            }
            set { position = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

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
