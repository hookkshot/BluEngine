﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.ScreenManager.Widgets
{
    public class Button : Control
    {
        private bool mouseEntered = false;
        private bool mouseDown = false;
        public event MouseEvent OnClick;

        /// <summary>
        /// The style used by this button when it is in a disabled state.
        /// </summary>
        public Style DisabledStyle
        {
            get { return disabledStyle; }
        }
        private Style disabledStyle;
        
        /// <summary>
        /// The style used by this button when the mouse is hovering over it.
        /// </summary>
        public Style HoverStyle
        {
            get { return hoverStyle; }
        }
        private Style hoverStyle;

        /// <summary>
        /// The style used by this button when it is currently being clicked/pressed.
        /// </summary>
        public Style DownStyle
        {
            get { return downStyle; }
        }
        private Style downStyle;
        
        /// <summary>
        /// Create a new Button with a given parent.
        /// </summary>
        /// <param name="parent">The Widget's parent.</param>
        public Button(Widget parent) : base(parent)
        {
            HitFlags = Engine.HitFlags.Mouse;
            Style.Parent = Widget.Styles.Button;
            disabledStyle = new Style(Widget.Styles.ButtonDisabled);
            downStyle = new Style(Widget.Styles.ButtonDown);
            hoverStyle = new Style(Widget.Styles.ButtonHover);
        }

        public virtual void SetAllStateFills(Texture2D tex)
        {
            Style.Fill = tex;
            disabledStyle.Fill = tex;
            downStyle.Fill = tex;
            hoverStyle.Fill = tex;
        }

        public override Style CurrentStyle
        {
            get { return Enabled ? (mouseDown ? (mouseEntered ? downStyle : Style) : (mouseEntered ? hoverStyle : Style)) : disabledStyle; }
        }

        public override void MouseEnter(Point pt)
        {
            base.MouseEnter(pt);
            mouseEntered = true;
        }

        public override void MouseLeave(Point pt)
        {
            mouseEntered = false;
            base.MouseLeave(pt);
        }

        public override void MouseDown(Point pt, int button)
        {
            base.MouseDown(pt, button);
            if (button == 1 && Enabled)
                mouseDown = true;
        }

        public override void MouseUp(Point pt, int button)
        {
            if (button == 1)
            {
                if (mouseDown)
                {
                    mouseDown = false;
                    if (Enabled && CalculatedBoundsI.Contains(pt))
                    {
                        Console.WriteLine("Click! ^_^");
                        
                        if (OnClick != null)
                            OnClick(pt);
                    }
                }
            }
            base.MouseUp(pt, button);
        }
    }
}