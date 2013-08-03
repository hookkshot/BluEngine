using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BluEngine.ScreenManager.Widgets;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BluEngine.Engine;
using Microsoft.Xna.Framework.Graphics;


namespace BluEngine.ScreenManager
{
    public class WidgetScreen : GameScreen
    {
        private ScreenWidget baseWidget = new ScreenWidget();
        private Widget[] mouseDownWidgets = new Widget[]{null,null,null,null,null};
        private Widget mouseHoverWidget = null;
        private Widget selectedWidget = null;
        private Keys[] watchedKeys = new Keys[] {
            Keys.Back, Keys.Tab, Keys.Enter,
            Keys.Pause, Keys.CapsLock, Keys.Escape,
            Keys.Space, Keys.PageUp, Keys.PageDown,
            Keys.End, Keys.Home, Keys.Left,
            Keys.Up, Keys.Right, Keys.Down,
            Keys.Select, Keys.Insert, Keys.Delete,
            Keys.Help, Keys.D0, Keys.D1,
            Keys.D2, Keys.D3, Keys.D4,
            Keys.D5, Keys.D6, Keys.D7,
            Keys.D8, Keys.D9, Keys.A,
            Keys.B, Keys.C, Keys.D,
            Keys.E, Keys.F, Keys.G,
            Keys.H, Keys.I, Keys.J, Keys.K,
            Keys.L, Keys.M, Keys.N, Keys.O,
            Keys.P, Keys.Q, Keys.R, Keys.S,
            Keys.T, Keys.U, Keys.V, Keys.W,
            Keys.X, Keys.Y, Keys.Z, Keys.LeftWindows,
            Keys.RightWindows, Keys.NumPad0, Keys.NumPad1, Keys.NumPad2,
            Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6,
            Keys.NumPad7, Keys.NumPad8, Keys.NumPad9, Keys.Multiply,
            Keys.Add, Keys.Separator, Keys.Subtract, Keys.Decimal, Keys.Divide,
            Keys.F1, Keys.F2, Keys.F3, Keys.F4,
            Keys.F5, Keys.F6, Keys.F7, Keys.F8,
            Keys.F9, Keys.F10, Keys.F11, Keys.F12, Keys.F13,
            Keys.F14, Keys.F15, Keys.F16, Keys.F17, Keys.F18, Keys.F19,
            Keys.F20, Keys.F21, Keys.F22, Keys.F23, Keys.F24, Keys.NumLock,
            Keys.Scroll, Keys.LeftShift, Keys.RightShift, Keys.LeftControl,
            Keys.RightControl, Keys.LeftAlt, Keys.RightAlt
        };

        /// <summary>
        /// The base widget object. Add your child widgets to this via their constructors or by setting their Parent property.
        /// </summary>
        public ScreenWidget Base
        {
            get { return baseWidget; }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            baseWidget.UpdateAll(gameTime);
        }

        public override void HandleInput(InputControl input)
        {
            base.HandleInput(input);
            
            //store mouse pos
            Point mousePos = new Point(input.MouseX(), input.MouseY());
            
            //get widget at mouse point
            Widget widgetAtPoint = baseWidget.ChildAtPoint(mousePos,HitFlags.Mouse);

            //fire off mouse enter/leave events
            if (widgetAtPoint != mouseHoverWidget)
            {
                if (mouseHoverWidget != null)
                    mouseHoverWidget.MouseLeave(mousePos);

                mouseHoverWidget = widgetAtPoint;

                if (mouseHoverWidget != null)
                    mouseHoverWidget.MouseEnter(mousePos);
            }

            //check mouse button events
            for (int i = 1; i <= 5; i++)
            {
                ButtonState current;
                if (input.MouseChanged(i,out current))
                {
                    if (current == ButtonState.Pressed)
                    {
                        mouseDownWidgets[i-1] = widgetAtPoint;
                        if (widgetAtPoint != null)
                            widgetAtPoint.MouseDown(mousePos,i);
                    }
                    else
                    {
                        if (mouseDownWidgets[i-1] != null)
                            mouseDownWidgets[i-1].MouseUp(mousePos,i);
                        mouseDownWidgets[i-1] = null;
                    }

                }
            }

            //check keyboard events
            if (selectedWidget != null)
            {
                for (int i = 0; i < watchedKeys.Length; i++)
                {
                    if (input.KeyReleased(watchedKeys[i]))
                        selectedWidget.KeyUp(watchedKeys[i]);

                    if (input.KeyPressed(watchedKeys[i]))
                        selectedWidget.KeyDown(watchedKeys[i]);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            baseWidget.DrawAll(spriteBatch);
            spriteBatch.End();
        }
    }


}
