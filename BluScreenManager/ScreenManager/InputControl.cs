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
            return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        public bool KeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        public bool MousePress(int i)
        {
            ButtonState state;
            return MouseChanged(i, out state) && state == ButtonState.Pressed;
        }

        public bool MouseReleased(int i)
        {
            ButtonState state;
            return MouseChanged(i, out state) && state == ButtonState.Released;
        }
        
        public bool MouseHold(int i)
        {
            ButtonState current;
            switch (i)
            {
                case 5: current = currentMouseState.XButton2; break;
                case 4: current = currentMouseState.XButton1; break;
                case 3: current = currentMouseState.MiddleButton; break;
                case 2: current = currentMouseState.RightButton; break;
                default:
                case 1: current = currentMouseState.LeftButton; break;
            }

            return current == ButtonState.Pressed;
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

        public bool MouseChanged(int button, out ButtonState current)
        {
            ButtonState previous;
            switch (button)
            {
                case 5: current = currentMouseState.XButton2; previous = previousMouseState.XButton2; break;
                case 4: current = currentMouseState.XButton1; previous = previousMouseState.XButton1; break;
                case 3: current = currentMouseState.MiddleButton; previous = previousMouseState.MiddleButton; break;
                case 2: current = currentMouseState.RightButton; previous = previousMouseState.RightButton; break;
                default:
                case 1: current = currentMouseState.LeftButton; previous = previousMouseState.LeftButton; break;
            }
            return current != previous;
        }
    }
}
