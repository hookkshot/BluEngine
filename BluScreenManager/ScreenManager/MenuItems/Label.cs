
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.ScreenManager.MenuItems
{
    class Label : MenuItem
    {
        #region Fields

        SpriteFont font;

        string text;

        Alignment align;
        Color color;

        #endregion

        public Label(SpriteFont font, Vector2 pos, string text, Alignment align,Color color)
        {
            this.font = font;
            this.Position = pos;
            this.text = text;
            this.align = align;
            this.color = color;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (align)
            {
                case Alignment.Left:
                    {
                        spriteBatch.DrawString(font, text, Position, color);
                    }
                    break;
                case Alignment.Center:
                    {
                        spriteBatch.DrawString(font, text, Position - new Vector2(font.MeasureString(text).X / 2,0), color);
                    }
                    break;
                case Alignment.Right:
                    {
                        spriteBatch.DrawString(font, text, Position - new Vector2(font.MeasureString(text).X, 0), color);
                    }
                    break;
            }
            
        }
    }
}
