using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BluEngine.ScreenManager.Widgets
{
    public delegate void TextEntered(string text);

    public class TextBox : Control
    {
        #region Fields

        public TextEntered OnEnter
        {
            get { return onEnter; }
            set { onEnter = value; State = CurrentState; }
        }
        private event TextEntered onEnter;

        public override List<Type> Hierarchy
        {
            get
            {
                if (hierarchy == null)
                {
                    hierarchy = new List<Type>();
                    hierarchy.Add(typeof(TextBox));
                    hierarchy.AddRange(base.Hierarchy);
                }
                return hierarchy;
            }
        }
        private static List<Type> hierarchy = null;

        private bool isMouseEntered = false;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        private string text = "";

        Keys[] keysToCheck = new Keys[] {Keys.A, Keys.B, Keys.C, Keys.D, Keys.E,
            Keys.F, Keys.G, Keys.H, Keys.I, Keys.J,
            Keys.K, Keys.L, Keys.M, Keys.N, Keys.O,
            Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T,
            Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y,
            Keys.Z, Keys.D0, Keys.D1, Keys.D2, Keys.D3,
            Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8,
            Keys.D9, Keys.Back, Keys.Space, Keys.Left, Keys.Right, Keys.Enter };

        /// <summary>
        /// If this is selected all the characters will appear as *'s.
        /// </summary>
        public bool HideCharacters = false;

        public int MaxCharacters
        {
            get { return maxCharacters;  }
            set { maxCharacters = value; }
        }
        private int maxCharacters;

        private int index = 0;

        public SpriteFont Font
        {
            set { font = value; }
        }
        private SpriteFont font;

        #endregion

        #region Constructors

        public TextBox(Widget parent)
            : base(parent)
        {
            HitFlags = HitFlags.Mouse;
        }

        #endregion

        protected override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (font != null)
            {
                string textToDraw = text.Substring(0, index) + "|" + text.Substring(index, text.Length - index);

                Vector2 pos = new Vector2(CalculatedBoundsF.X, CalculatedBoundsF.Y);

                Vector2 textsize = font.MeasureString(textToDraw);

                float maxWidth = CalculatedBoundsF.W-6;

                while(font.MeasureString(textToDraw).X > maxWidth)
                {
                    if (textToDraw.Length - index >= index)
                        textToDraw = textToDraw.Substring(0, textToDraw.Length - 1);
                    else
                        textToDraw = textToDraw.Substring(1, textToDraw.Length - 1);
                }

                Vector2 insidePos = new Vector2(3,CalculatedBoundsF.Z/2-textsize.Y/2);

                spriteBatch.DrawString(font, textToDraw, pos+insidePos, Color.White);
            }
        }

        #region Methods

        public override bool MouseDown(Point pt, int button)
        {
            return base.MouseDown(pt, button);
        }

        public override bool KeyDown(Keys key)
        {
            if(keysToCheck.Contains(key))
            {
                switch(key)
                {
                    case Keys.Enter:
                        if (onEnter != null)
                            onEnter(text);
                        break;
                    case Keys.Back:
                        if (index > 0 && index <= text.Length)
                        {
                            text = text.Substring(0, index - 1) + text.Substring(index, text.Length - index);
                            index--;
                        }
                        break;
                    case Keys.Space:
                        if (text.Length < maxCharacters)
                        {
                            if (index <= text.Length)
                            {
                                InsertText(" ");
                                index++;
                            }
                        }
                        break;
                    case Keys.Left:
                        if (index > 0)
                            index--;
                        break;
                    case Keys.Right:
                        if (index < text.Length)
                            index++;
                        break;
                    default:
                        string charToAdd = key.ToString();

                        if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !Keyboard.GetState().IsKeyDown(Keys.RightShift))
                            charToAdd = charToAdd.ToLower();

                        if (text.Length < maxCharacters)
                        {
                            if (index <= text.Length)
                            {
                                //text.Insert(index, charToAdd);
                                InsertText(charToAdd);
                                index++;
                            }
                        }

                        break;
                }

                return true;
            }
            return base.KeyDown(key);
        }

        private void InsertText(string insert)
        {
            text = text.Substring(0, index) + insert + text.Substring(index, text.Length-index);
        }

        #endregion
    }
}
