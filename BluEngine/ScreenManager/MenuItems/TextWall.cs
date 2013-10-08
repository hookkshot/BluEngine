using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.ScreenManager.MenuItems
{
    public class TextWall : MenuItem
    {
        public string Text = "";
        public SpriteFont Font;
        public Alignment Alignment = Alignment.Center;

        public int MaxWidth = 100;
        public int MaxHeight = 100;

        #region Properties

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Font != null)
            {

                string[] lines = Text.Split('\n');

                List<string> linesToPrint = new List<string>();

                for (int i = 0; i < lines.Count(); i++)
                {
                    string[] wordsInLine = lines[i].Split(' ');

                    int index = 0;
                    string newLine = "";
                    bool drawing = true;

                    while (drawing)
                    {

                        if (index < wordsInLine.Count())
                        {
                            if (Font.MeasureString(newLine + " " + wordsInLine[index]).X < Width)
                            {
                                newLine += " " + wordsInLine[index];
                            }
                            else
                            {
                                linesToPrint.Add(newLine);
                                newLine = wordsInLine[index];
                            }
                        }
                        else if (newLine != "")
                        {
                            linesToPrint.Add(newLine);
                            newLine = "";
                        }
                        else

                            drawing = false;
                        index++;
                    }
                }

                for (int x = 0; x < linesToPrint.Count; x++)
                {
                    spriteBatch.DrawString(Font, linesToPrint[x], Position + new Vector2(-Font.MeasureString(linesToPrint[x]).X/2, x * 13), Color);
                }
            }
        }

        #endregion
    }
}
