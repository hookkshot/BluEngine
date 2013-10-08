#region File Description
//File: DropdownBox.cs
//Date: 14/08/2012
//Version: 1.0
//
//Copyright (C) Blurift Entertainment
#endregion

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.ScreenManager.MenuItems
{
    class DropdownBox : Image
    {
        #region Fields

        protected List<string> values;
        protected Texture2D fillTexture;
        protected SpriteFont font;
        protected int current;

        public bool Locked;

        //Selected

        #endregion

        #region Properties

        public string Value
        {
            get {
                if (current < values.Count())
                {
                    return values[current];  
                } 
                else
                {
                    return "null";
                }
            }
        }

        public int GetCurrent
        {
            get { return this.current; }
        }

        public int SetValue
        {
            set
            {
                if (value < values.Count())
                {
                    current = value;
                }
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Dropdown box
        /// </summary>
        /// <param name="position">The top left position</param>
        /// <param name="texture"></param>
        /// <param name="fillTexture">This texture is what will be used as the background of the drop down menu. a solid color is preferable.</param>
        /// <param name="font"></param>
        public DropdownBox(Vector2 position, Texture2D texture, Texture2D fillTexture, SpriteFont font)
            : base(position,texture)
        {
            values = new List<string>();
            this.fillTexture = fillTexture;
            this.font = font;
            this.current = 0;
            this.Locked = false;
        }

        #endregion

        #region Update

        public override void HandleInput(InputControl input, bool inFocus)
        {
            if (!Locked && inFocus)
            {
                Texture2D texture = Source;
                if (!isItemInUse)
                {
                    if (input.MouseX() > Position.X && input.MouseX() < (Position.X + texture.Width) && input.MouseY() > Position.Y && input.MouseY() < (Position.Y + texture.Height))
                    {
                        if (input.MouseReleased(1))
                        {
                            isItemInUse = true;
                        }
                    }
                }
                else
                {
                    if (input.MouseX() > Position.X && input.MouseX() < (Position.X + texture.Width) && input.MouseY() > Position.Y && input.MouseY() < (Position.Y + texture.Height))
                    {
                        if (input.MouseReleased(1))
                        {
                            isItemInUse = false;
                        }
                    }
                    for (int i = 0; i < values.Count(); i++)
                    {
                        if (input.MouseX() > Position.X && input.MouseX() < (Position.X + texture.Width) && input.MouseY() > Position.Y + (texture.Height * (i + 1))
                            && input.MouseY() < (Position.Y + texture.Height + (texture.Height * (i + 1))))
                        {
                            if (input.MouseReleased(1))
                            {
                                isItemInUse = false;
                                current = i;
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
            Texture2D texture = Source;
            spriteBatch.Draw(texture, Position, Color.White);

            if (values.Count() > 0)
            {
                if (current < values.Count())
                {
                    spriteBatch.DrawString(this.font, values[current],new Vector2(
                        Position.X + (texture.Width / 2) - (font.MeasureString(values[current]).X / 2),
                        Position.Y + (texture.Height / 2) - (font.MeasureString(values[current]).Y / 2)),
                        Color.White);
                }
            }

            if (isItemInUse)
            {
                for (int i = 0; i < values.Count(); i++)
                {
                    spriteBatch.Draw(fillTexture, new Rectangle((int)Position.X, (int)Position.Y + (texture.Height * (i + 1)), texture.Width, texture.Height), Color.Blue);
                    spriteBatch.DrawString(this.font, values[i],new Vector2(
                        Position.X + (texture.Width / 2) - (font.MeasureString(values[i]).X / 2),
                        Position.Y + (texture.Height / 2) - (font.MeasureString(values[i]).Y / 2) + (texture.Height * (i + 1))),
                        Color.White);
                }
            }
            
        }

        #endregion

        #region Methods

        public void AddValue(string value)
        {
            values.Add(value);
        }
        #endregion
    }
}
