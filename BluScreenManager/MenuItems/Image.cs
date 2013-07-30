using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine
{
    public class Image : MenuItem
    {
        protected Texture2D source;

        public Texture2D Source
        {
            get { return source; }
            set { source = value; }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(source, this.Position, Color);
        }
    }
}
