using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
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
        private string text;

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

        #endregion

        #region Constructors

        public TextBox(Widget parent)
            : base(parent)
        {
            HitFlags = HitFlags.Mouse;
        }

        #endregion


        #region Methods

        public override bool MouseEnter(Point pt)
        {
            isMouseEntered = true;
            return base.MouseEnter(pt);
        }

        public override bool MouseLeave(Point pt)
        {
            isMouseEntered = false;
            return base.MouseLeave(pt);
        }

        public override bool MouseDown(Point pt, int button)
        {
            if (button == 1 && isMouseEntered == true)
            {
                Widget.SelectedWidget = this;
            }
            return base.MouseDown(pt, button);
        }

        public override bool KeyDown(Keys key)
        {
            if (Widget.SelectedWidget == this)
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
                                text.Remove(index - 1, index);
                            }
                            break;
                        case Keys.Space:
                            if (text.Length < maxCharacters)
                            {
                                if (index <= text.Length)
                                {
                                    text.Insert(index, " ");
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

                            if (!Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.RightShift))
                                charToAdd = charToAdd.ToLower();

                            if (text.Length < maxCharacters)
                            {
                                if (index <= text.Length)
                                {
                                    text.Insert(index, charToAdd);
                                    index++;
                                }
                            }

                            break;
                    }
                }
            }
            return base.KeyDown(key);
        }

        #endregion
    }
}
