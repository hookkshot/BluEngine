using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BluEngine.ScreenManager.Styles;

namespace BluEngine.ScreenManager.Widgets
{
    /// <summary>
    /// A specialised widget designed for handling click events.
    /// </summary>
    public class Button : Control
    {
        private bool mouseEntered = false;
        private bool mouseDown = false;
        public event MouseEvent OnClick;

        public override List<Type> Hierarchy
        {
            get
            {
                if (hierarchy == null)
                {
                    hierarchy = new List<Type>();
                    hierarchy.Add(typeof(Button));
                    hierarchy.AddRange(base.Hierarchy);
                }
                return hierarchy;
            }
        }
        private static List<Type> hierarchy = null;
       
        /// <summary>
        /// Create a new Button with a given parent.
        /// </summary>
        /// <param name="parent">The Widget's parent.</param>
        public Button(Widget parent) : base(parent)
        {
            HitFlags = Engine.HitFlags.Mouse;
        }

        public override String State
        {
            get { return Enabled ? (OnClick != null ? (mouseDown ? (mouseEntered ? "down" : base.State) : (mouseEntered ? "hover" : base.State)) : base.State) : "disabled"; }
        }

        public override bool MouseEnter(Point pt)
        {
            mouseEntered = true;
            return base.MouseEnter(pt);
        }

        public override bool MouseLeave(Point pt)
        {
            mouseEntered = false;
            return base.MouseLeave(pt);
        }

        public override bool MouseDown(Point pt, int button)
        {
            if (button == 1 && Enabled)
                mouseDown = true;
            return base.MouseDown(pt, button);
        }

        public override bool MouseUp(Point pt, int button)
        {
            if (button == 1)
            {
                if (mouseDown)
                {
                    mouseDown = false;
                    if (Enabled && CalculatedBoundsI.Contains(pt))
                    {
                        if (OnClick != null)
                            OnClick(this, pt);
                    }
                }
            }
            return base.MouseUp(pt, button);
        }
    }
}
