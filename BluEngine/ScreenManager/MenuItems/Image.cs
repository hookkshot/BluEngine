﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.ScreenManager.MenuItems
{
    public class Image : MenuItem
    {
        private Texture2D texture;

        public Texture2D Source
        {
            get { return texture; }
            set { texture = value; }
        }

        public Image(Vector2 position, Texture2D texture)
            : base(position)
        {
            this.texture = texture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, this.Position, Color);
        }
    }
}
