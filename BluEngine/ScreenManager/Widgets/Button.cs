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
        public bool IsMouseEntered
        {
            get { return mouseEntered; }
            protected set { mouseEntered = value; State = CurrentState; }
        }
        private bool mouseEntered = false;
        
        public bool IsMouseDown
        {
            get { return mouseDown; }
            protected set { mouseDown = value; State = CurrentState; }
        }
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
            get { return (Enabled ? (mouseEntered ? (mouseDown ? "down|" : "") + "hover|" : "") : "") + base.CurrentState; }
        }

        public override bool MouseEnter(Point pt)
        {
            IsMouseEntered = true;
            return base.MouseEnter(pt);
        }

        public override bool MouseLeave(Point pt)
        {
            IsMouseEntered = false;
            return base.MouseLeave(pt);
        }

        public override bool MouseDown(Point pt, int button)
        {
            if (button == 1 && Enabled)
                IsMouseDown = true;
            return base.MouseDown(pt, button);
        }

        public override bool MouseUp(Point pt, int button)
        {
            if (button == 1)
            {
                if (mouseDown)
                {
                    if (Enabled && CalculatedBoundsI.Contains(pt))
                    {
                        if (onClick != null)
                            onClick(this, pt);
                    }
                    IsMouseDown = false;
                }
            }
            return base.MouseUp(pt, button);
        }
    }
}
