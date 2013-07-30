using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BluEngine
{
    public class InputControl
    {
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        private MouseState currentMouseState;
        private MouseState previousMouseState;

        private TimeSpan[] mouseHold;

        private Vector2 leftMouseClickPosition;
        private bool leftMouseHold;

        private GameTime gameTime;

        public InputControl()
        {
            mouseHold = new TimeSpan[3];
        }

        public void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;

            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;

            currentMouseState = Mouse.GetState();
            currentKeyboardState = Keyboard.GetState();
        }

        public bool KeyHit(Keys key)
        {
            bool ret = false;

            if (currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key))
            {
                ret = true;
            }

            return ret;
        }

        public bool KeyDown(Keys key)
        {
            if(currentKeyboardState.IsKeyDown(key)) return true;
            return false;
        }

        public bool MousePress(int i)
        {
            bool ret = false;

            if (i == 1)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
                {
                    ret = true;
                }
            }

            if (i == 2)
            {
                if (currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton != ButtonState.Pressed)
                {
                    ret = true;
                }
            }

            return ret;
        }

        public bool MouseReleased(int i)
        {
            bool ret = false;

            if (i == 1)
            {
                if (currentMouseState.LeftButton != ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    ret = true;
                }
            }

            return ret;
        }

        public bool MouseHold(int i)
        {
            bool ret = false;

            switch (i)
            {
                case 1:
                    {
                        if (currentMouseState.LeftButton == ButtonState.Pressed)
                        {
                            ret = true;
                        }
                    }
                    break;
                case 2:
                    {
                        if (currentMouseState.RightButton == ButtonState.Pressed)
                        {
                            ret = true;
                        }
                    }
                    break;
            }
            

            return ret;
        }

        public Vector2 LeftMouseOrigin()
        {
            return this.leftMouseClickPosition;
        }

        public bool LeftMouseHold()
        {
            bool ret = false;

            if (currentMouseState.X > leftMouseClickPosition.X + 3 || currentMouseState.X < leftMouseClickPosition.X - 3)
            {
                if (currentMouseState.Y > leftMouseClickPosition.Y + 3 || currentMouseState.Y < leftMouseClickPosition.Y - 3)
                {
                    ret = this.leftMouseHold;
                }
            }

            return ret;
        }

        public int MouseX()
        {
            return currentMouseState.X;
        }

        public int MouseY()
        {
            return currentMouseState.Y;
        }

        public Vector2 MouseMove()
        {
            return new Vector2(currentMouseState.X - previousMouseState.X, currentMouseState.Y - previousMouseState.Y);
        }

        public string InputField(string val)
        {
            
            return val;
        }
    }
}
