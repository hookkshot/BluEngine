#region File Description
//File: Textbox.cs
//Date: 11/08/2012
//Version: 1.0
//
//Copyright (C) Blurift Entertainment
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BluEngine
{
    /// <summary>
    /// A textbox that simple text can be entered ranging from letters and numbers.
    /// </summary>
    public class Textbox : Image
    {
        #region Fields

        public delegate bool TextboxKeyEvent(Textbox sender, Keys key);

        protected int index = 0;

        protected int width;
        protected int height = 30;

        /// <summary>
        /// Allows there to be some pixels in between where text is displayed and actual bounds are
        /// </summary>
        public int Padding = 0;

        protected SpriteFont font;
        protected Color textColor;

        protected bool isSelected;
        protected string textValue;
        protected bool locked;
        protected int max;

        /// <summary>
        /// If this is selected all the characters will appear as *'s.
        /// </summary>
        public bool HideCharacters = false;

        /// <summary>
        /// If this is selected the background image will strecth to the bounds of the textbox.
        /// </summary>
        public bool StretchBackgroundImage = false;

        protected TimeSpan lastSwitch = TimeSpan.FromSeconds(0.0f);
        protected string indexChar = "|";

        public event MenuItemEvent Submitted;
        public event TextboxKeyEvent OnKeyPressed; //return true if the input is handled

        Keys[] keysToCheck = new Keys[] {Keys.A, Keys.B, Keys.C, Keys.D, Keys.E,
            Keys.F, Keys.G, Keys.H, Keys.I, Keys.J,
            Keys.K, Keys.L, Keys.M, Keys.N, Keys.O,
            Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T,
            Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y,
            Keys.Z, Keys.D0, Keys.D1, Keys.D2, Keys.D3,
            Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8,
            Keys.D9, Keys.Back, Keys.Space, Keys.Left, Keys.Right, Keys.Enter };

        #endregion

        #region Properties

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        public string Text
        {
            get { return textValue; }
            set { textValue = value; }
        }

        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        public bool Locked
        {
            get { return locked; }
            set { locked = value; }
        }

        #endregion

        #region Initialization

        public Textbox(int width, int max, Vector2 position, SpriteFont font, Texture2D texture)
            : base (position,texture)
        {
            this.active = true;
            this.isItemInUse = false;

            this.width = width;
            this.max = max;
            this.font = font;
            this.isSelected = false;
            this.textValue = "";
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - TimeSpan.FromSeconds(0.6f) > lastSwitch)
            {
                if (indexChar == "|")
                {
                    indexChar = " ";
                }
                else
                {
                    indexChar = "|";
                }
                lastSwitch = gameTime.TotalGameTime;
            }
            base.Update(gameTime);
        }

        public override void HandleInput(InputControl input, bool inFocus)
        {
            if (inFocus)
            {
                if (input.MousePressed(1))
                {
                    if (input.MouseX() > Position.X && input.MouseX() < (Position.X + width) && input.MouseY() > Position.Y && input.MouseY() < (Position.Y + height))
                    {
                        isSelected = true;

                    }
                    else
                    {
                        isSelected = false;
                    }
                }
                if (isSelected)
                {                   
                    foreach (Keys key in keysToCheck)
                    {
                        if (input.KeyPressed(key) && (OnKeyPressed == null || !OnKeyPressed(this, key)))
                        {
                            switch (key)
                            {
                                case (Keys.Back):
                                    if (textValue.Length > 0 && index > 0)
                                    {
                                        textValue = textValue.Remove(index - 1, 1);
                                        index--;
                                    }
                                    break;
                                case (Keys.Enter):
                                    if(Submitted != null)
                                        Submitted(this);
                                    index = 0;
                                    break;
                                case (Keys.Space):
                                    textValue += " ";
                                    break;
                                case (Keys.Left):
                                    if (index > 0)
                                        index--;
                                    break;
                                case (Keys.Right):
                                    if (index < textValue.Count())
                                        index++;
                                    break;
                                default:
                                    string charToAdd = key.ToString();
                                    if (!input.KeyDown(Keys.RightShift) && !input.KeyDown(Keys.LeftShift))
                                    {
                                        charToAdd = charToAdd.ToLower();

                                    }
                                    if (textValue.Length < this.max)
                                    {
                                        textValue += charToAdd;
                                        index++;
                                    }
                                    break;
                            }
                        }
                    }
                    
                }
            }
            
        }
        

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                if (this.textValue == null)
                {
                    this.textValue = "";
                }

                Texture2D texture = Source;
                if (texture != null)
                {
                    if (StretchBackgroundImage)
                    {
                        spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, width, height), Color);
                    }
                    else
                    {
                        spriteBatch.Draw(texture, new Rectangle((int)Position.X + width / 2 - texture.Width / 2,
                            (int)Position.Y + height / 2 - texture.Height / 2, texture.Width, texture.Height), Color);
                    }
                }
                
                string textToDraw = "";
                for (int i = 0; i < textValue.Count()+1; i++)
                {
                    if (i == index && isSelected)
                        textToDraw += indexChar;
                    if (i < textValue.Count())
                    {
                        if (HideCharacters)
                        {
                            textToDraw += "*";
                        }
                        else
                        {
                            textToDraw += textValue[i];
                        }
                    }
                }

                if (font.MeasureString(textToDraw).X > width - (Padding*2))
                {
                    bool sizeNotFixed = true;

                    while (sizeNotFixed)
                    {
                        string newText = "";
                        for (int i = 1; i < textToDraw.Count(); i++)
                        {
                            newText += textToDraw[i];
                        }
                        textToDraw = newText;
                        if (font.MeasureString(textToDraw).X < width - 12)
                            sizeNotFixed = false;
                    }
                }

                spriteBatch.DrawString(this.font, textToDraw, new Vector2(this.Position.X + Padding, this.Position.Y + (Height/2) - (this.font.MeasureString(textToDraw).Y / 2)), TextColor);

            }

        }

        #endregion
    }
}
