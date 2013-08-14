using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BluEngine.ScreenManager.Widgets;

namespace BluEngine.ScreenManager.Styles
{
    public class BorderLayer : ImageLayer
    {
        public BorderStyle BorderStyle
        {
            get { return borderStyle; }
            set { borderStyle = value; }
        }
        private BorderStyle borderStyle;

        public float BorderWidth
        {
            get { return borderWidth; }
            set { borderWidth = value; }
        }
        private float borderWidth;

        public Color BorderColour
        {
            get { return borderColour; }
            set { borderColour = value; }
        }
        private Color borderColour;

        public BorderLayer() : base(SolidColours.White) { }

        public override void Draw(SpriteBatch spriteBatch, Widget widget, Color col)
        {
            if (Texture == null
                || col.A == 0
                || BorderStyle == Styles.BorderStyle.None
                || BorderStyle == Styles.BorderStyle.Hidden
                || BorderWidth <= 0.0f)
                return;

            int left = (int)((float)widget.CalculatedBoundsI.X * Width);
            int top = (int)((float)widget.CalculatedBoundsI.Y * Height);
            int width = (int)((float)widget.CalculatedBoundsI.Width * Width);
            int height = (int)((float)widget.CalculatedBoundsI.Height * Height);
            int bw = (int)BorderWidth;

            if (BorderStyle == Styles.BorderStyle.Solid || BorderStyle == Styles.BorderStyle.Double)
            {
                //top and bottom
                spriteBatch.Draw(Texture, new Rectangle(left, top, width, bw), col);
                spriteBatch.Draw(Texture, new Rectangle(left, top + height - bw, width, bw), col);

                //left and right
                spriteBatch.Draw(Texture, new Rectangle(left, top + bw, bw, height - 2 * bw), col);
                spriteBatch.Draw(Texture, new Rectangle(left + width - bw, top + bw, bw, height - 2 * bw), col);
                

                if (BorderStyle == Styles.BorderStyle.Double)
                {
                    left += bw * 2;
                    top += bw * 2;
                    width -= bw * 4;
                    height -= bw * 4;

                    //top and bottom
                    spriteBatch.Draw(Texture, new Rectangle(left, top, width, bw), col);
                    spriteBatch.Draw(Texture, new Rectangle(left, top + height - bw, width, bw), col);

                    //left and right
                    spriteBatch.Draw(Texture, new Rectangle(left, top + bw, bw, height - 2 * bw), col);
                    spriteBatch.Draw(Texture, new Rectangle(left + width - bw, top + bw, bw, height - 2 * bw), col);
                }
            }
            else if (BorderStyle == Styles.BorderStyle.Dotted || BorderStyle == Styles.BorderStyle.Dashed)
            {
                int len = bw * (BorderStyle == Styles.BorderStyle.Dotted ? 1 : 4);
                
                //top and bottom
                int pos = 0;
                while (pos < width)
                {
                    spriteBatch.Draw(Texture, new Rectangle(left + pos, top, len, bw), col);
                    spriteBatch.Draw(Texture, new Rectangle(left + pos, top + height - bw, len, bw), col);
                    pos += len * 2;
                }

                //left and right
                height -= bw * 2;
                pos = 0;
                while (pos < height)
                {
                    spriteBatch.Draw(Texture, new Rectangle(left, top + bw + pos, bw, len), col);
                    spriteBatch.Draw(Texture, new Rectangle(left + width - bw, top + bw + pos, bw, len), col);
                    pos += len * 2;
                }
            }
        }
    }
}
