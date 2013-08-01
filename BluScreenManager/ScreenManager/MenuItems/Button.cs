#region File Description
//File: Button.cs
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
using Microsoft.Xna.Framework.Audio;
using BluEngine.ScreenManager;

namespace BluEngine
{
    public class Button : MenuItem
    {
        #region Fields

        //Misc
        private BluEngine.ScreenManager.ScreenManager screenManager;

        //Input
        protected bool down;
        protected bool downPrevious;

        //Graphical
        protected Texture2D texture;
        protected Texture2D textureDown;
        protected Texture2D textureOver;

        //Events
        public event EventHandler Selected;

        //Sound
        protected SoundEffect clickSound;

        //Alignment
        protected Alignment align = Alignment.Center;

        //Text
        protected string text;
        protected SpriteFont font;

        #endregion

        #region Properties

        public BluEngine.ScreenManager.ScreenManager ScreenManager
        {
            set { this.screenManager = value; }
        }

        public Texture2D Texture
        {
            set { this.texture = value; }
        }

        public Texture2D TextureDown
        {
            set { this.textureDown = value; }
        }

        public Texture2D TextureOver
        {
            set { this.textureOver = value; }
        }

        public Alignment Alignment
        {
            get { return this.align; }
            set { this.align = value; }
        }

        #endregion

        #region Initialization

        public Button(Vector2 position, Texture2D texture, Texture2D textureDown)
        {
            this.position = position;
            this.active = true;
            this.isItemInUse = false;
            
            this.texture = texture;
            this.textureDown = textureDown;
        }

        public Button(Vector2 position, Texture2D texture, Texture2D textureDown, Alignment align, string text, SpriteFont font) : this(position,texture,textureDown)
        {
            this.font = font;
            this.text = text;
            this.align = align;
        }

        public Button(Vector2 position, Texture2D texture, Texture2D textureDown, Alignment align, string text, SpriteFont font, SoundEffect clickSound)
            : this(position, texture, textureDown, align, text, font)
        {
            this.clickSound = clickSound;
        }

        #endregion

        #region Update and Draw

        public override void HandleInput(InputControl input, bool inFocus)
        {
            if (inFocus)
            {
                downPrevious = down;
                down = false;

                if (texture != null)
                {
                    int offset = (int)GetAlignment(texture).X;
                    if (input.MouseX() > Position.X-offset && input.MouseX() < (Position.X-offset + texture.Width) && input.MouseY() > Position.Y && input.MouseY() < (Position.Y + texture.Height))
                    {
                        down = input.MouseHold(1);

                        if (downPrevious == true && down == false)
                        {
                            if (Selected != null)
                            {
                                Selected(this, new EventArgs());
                            }
                        }

                        // Check to see button is just being clicked and play a sound
                        if (down == true && downPrevious == false)
                        {
                            if (clickSound != null)
                            {
                                clickSound.Play();
                            }
                        }
                    }
                }
                else
                {
                    int mw = (int)font.MeasureString(text).X;
                    int mh = (int)font.MeasureString(text).Y;
                    int mx = (int)Position.X;
                    switch (align)
                    {
                        case Alignment.Center: { mx -= mw / 2; } break;
                        case Alignment.Right: { mx -= mw; } break;
                    }
                    int my = (int)position.Y;

                    if (input.MouseX() > mx && input.MouseX() < mx + mw && input.MouseY() > my && input.MouseY() < my + mh)
                    {
                        down = input.MouseHold(1);

                        if (downPrevious == true && down == false)
                        {
                            if (Selected != null)
                            {
                                Selected(this, new EventArgs());
                            }
                        }

                        // Check to see button is just being clicked and play a sound
                        if (down == true && downPrevious == false) if (clickSound != null) clickSound.Play();
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.active)
            {
                if (down)
                {
                    if (texture != null)
                    {
                        if (textureDown != null)
                        {
                            spriteBatch.Draw(this.textureDown, Position - GetAlignment(textureDown), Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(this.texture, Position - GetAlignment(texture), Color.White);
                        }
                    }
                }
                else
                {
                    if (texture != null) spriteBatch.Draw(this.texture, Position - GetAlignment(texture), Color.White);

                    if(font != null || text != null)
                        spriteBatch.DrawString(font, text, Position - GetAlignment(text, font), Color.Black);
                }

            }
        }

        #endregion

        #region Methods

        protected Vector2 GetAlignment(Texture2D texture)
        {
            Vector2 offset = Vector2.Zero;
            switch (align)
            {
                case Alignment.Center:
                    offset.X = texture.Width / 2;
                    break;
                case Alignment.Right:
                    offset.X = texture.Width;
                    break;
            }
            return offset;
        }

        protected Vector2 GetAlignment(string text, SpriteFont font)
        {
            Vector2 offset = Vector2.Zero;
            switch (align)
            {
                case Alignment.Center:
                    offset.X = font.MeasureString(text).X / 2;
                    break;
                case Alignment.Right:
                    offset.X = font.MeasureString(text).X;
                    break;
            }
            return offset;
        }

        #endregion
    }
}