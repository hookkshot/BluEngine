using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using BluHelper;

namespace BluEngine
{
    public class InputControl
    {
        public const int MOUSE_LEFT = 1;
        public const int MOUSE_RIGHT = 2;
        public const int MOUSE_MIDDLE = 3;
        public const int MOUSE_4 = 4;
        public const int MOUSE_5 = 5;
        
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        private MouseState currentMouseState;
        private MouseState previousMouseState;

        private TimeSpan[] mouseHold;

        private Vector2 leftMouseClickPosition;
        private bool leftMouseHold;

        public InputControl()
        {
            mouseHold = new TimeSpan[3];
        }

        public void Update(GameTime gameTime)
        {
            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;

            currentMouseState = Mouse.GetState();
            currentKeyboardState = Keyboard.GetState();
        }

        public bool KeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        public bool KeyReleased(Keys key)
        {
            return currentKeyboardState.IsKeyUp(key) && previousKeyboardState.IsKeyDown(key);
        }

        public bool KeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        public bool KeyHold(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyDown(key);
        }

        public bool MousePressed(int i)
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
            ButtonState state;
            return !MouseChanged(i, out state) && state == ButtonState.Pressed;
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

        public Point CurrentMouse
        {
            get {return new Point(currentMouseState.X, currentMouseState.Y);}
        }

        public Point PreviousMouse
        {
            get { return new Point(previousMouseState.X, previousMouseState.Y); }
        }

        public Vector2 MouseMove()
        {
            return new Vector2(currentMouseState.X - previousMouseState.X, currentMouseState.Y - previousMouseState.Y);
        }

        public bool MouseMoved()
        {
            return currentMouseState.X != previousMouseState.X || currentMouseState.Y != previousMouseState.Y;
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
                case MOUSE_5: current = currentMouseState.XButton2; previous = previousMouseState.XButton2; break;
                case MOUSE_4: current = currentMouseState.XButton1; previous = previousMouseState.XButton1; break;
                case MOUSE_MIDDLE: current = currentMouseState.MiddleButton; previous = previousMouseState.MiddleButton; break;
                case MOUSE_RIGHT: current = currentMouseState.RightButton; previous = previousMouseState.RightButton; break;
                default:
                case MOUSE_LEFT: current = currentMouseState.LeftButton; previous = previousMouseState.LeftButton; break;
            }
            return current != previous;
        }
    }
}
