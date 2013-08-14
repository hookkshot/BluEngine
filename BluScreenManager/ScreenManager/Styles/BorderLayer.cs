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

        public int BorderWidth
        {
            get { return borderWidth; }
            set { borderWidth = value; }
        }
        private int borderWidth;

        public Color BorderColour
        {
            get { return borderColour; }
            set { borderColour = value; }
        }
        private Color borderColour;

        public BorderLayer(int width, BorderStyle style, Color color) : base(SolidColours.White)
        {
            borderWidth = width;
            borderStyle = style;
            borderColour = color;
        }
        public BorderLayer() : this(0,BorderStyle.Hidden,Color.Black) { }

        public override void Draw(SpriteBatch spriteBatch, Widget widget, Color col)
        {
            if (Texture == null
                || col.A == 0
                || BorderStyle == Styles.BorderStyle.None
                || BorderStyle == Styles.BorderStyle.Hidden
                || BorderWidth <= 0)
                return;

            int left = (int)((float)widget.CalculatedBoundsI.X * Width);
            int top = (int)((float)widget.CalculatedBoundsI.Y * Height);
            int width = (int)((float)widget.CalculatedBoundsI.Width * Width);
            int height = (int)((float)widget.CalculatedBoundsI.Height * Height);

            if (BorderStyle == Styles.BorderStyle.Solid || BorderStyle == Styles.BorderStyle.Double)
            {
                //top and bottom
                spriteBatch.Draw(Texture, new Rectangle(left, top, width, BorderWidth), col);
                spriteBatch.Draw(Texture, new Rectangle(left, top + height - BorderWidth, width, BorderWidth), col);

                //left and right
                spriteBatch.Draw(Texture, new Rectangle(left, top + BorderWidth, BorderWidth, height - 2 * BorderWidth), col);
                spriteBatch.Draw(Texture, new Rectangle(left + width - BorderWidth, top + BorderWidth, BorderWidth, height - 2 * BorderWidth), col);
                

                if (BorderStyle == Styles.BorderStyle.Double)
                {
                    left += BorderWidth * 2;
                    top += BorderWidth * 2;
                    width -= BorderWidth * 4;
                    height -= BorderWidth * 4;

                    //top and bottom
                    spriteBatch.Draw(Texture, new Rectangle(left, top, width, BorderWidth), col);
                    spriteBatch.Draw(Texture, new Rectangle(left, top + height - BorderWidth, width, BorderWidth), col);

                    //left and right
                    spriteBatch.Draw(Texture, new Rectangle(left, top + BorderWidth, BorderWidth, height - 2 * BorderWidth), col);
                    spriteBatch.Draw(Texture, new Rectangle(left + width - BorderWidth, top + BorderWidth, BorderWidth, height - 2 * BorderWidth), col);
                }
            }
            else if (BorderStyle == Styles.BorderStyle.Dotted || BorderStyle == Styles.BorderStyle.Dashed)
            {
                int len = BorderWidth * (BorderStyle == Styles.BorderStyle.Dotted ? 1 : 4);
                
                //top and bottom
                int pos = 0;
                while (pos < width)
                {
                    spriteBatch.Draw(Texture, new Rectangle(left + pos, top, len, BorderWidth), col);
                    spriteBatch.Draw(Texture, new Rectangle(left + pos, top + height - BorderWidth, len, BorderWidth), col);
                    pos += len * 2;
                }

                //left and right
                height -= BorderWidth * 2;
                pos = 0;
                while (pos < height)
                {
                    spriteBatch.Draw(Texture, new Rectangle(left, top + BorderWidth + pos, BorderWidth, len), col);
                    spriteBatch.Draw(Texture, new Rectangle(left + width - BorderWidth, top + BorderWidth + pos, BorderWidth, len), col);
                    pos += len * 2;
                }
            }
        }
    }
}
