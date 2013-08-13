using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BluEngine.ScreenManager.Widgets
{
    /// <summary>
    /// A specialised widget designed for handling click events.
    /// </summary>
    public class Button : Control
    {
        private bool mouseEntered = false;
        private bool mouseDown = false;
        public MouseEvent OnClick
        {
            get { return onClick; }
            set { onClick = value; State = CurrentState; }
        }
        private event MouseEvent onClick;

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
            HitFlags = HitFlags.Mouse;
        }

        protected override String CurrentState
        {
            get { return (Enabled ? (onClick != null && mouseEntered ? (mouseDown ? "down|" : "") + "hover|" : "") : "") + base.CurrentState; }
        }

        public override bool MouseEnter(Point pt)
        {
            mouseEntered = true;
            State = CurrentState;
            return base.MouseEnter(pt);
        }

        public override bool MouseLeave(Point pt)
        {
            mouseEntered = false;
            State = CurrentState;
            return base.MouseLeave(pt);
        }

        public override bool MouseDown(Point pt, int button)
        {
            if (button == 1 && Enabled)
                mouseDown = true;
            State = CurrentState;
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
                        if (onClick != null)
                            onClick(this, pt);
                    }
                    State = CurrentState;
                }
            }
            return base.MouseUp(pt, button);
        }
    }
}
