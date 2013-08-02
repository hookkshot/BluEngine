using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BluEngine.ScreenManager.Widgets
{
    /// <summary>
    /// Base class for a user input control widget.
    /// </summary>
    public class Control : Widget
    {
        /// <summary>
        /// The current "enabled" state of this widget.
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        private bool enabled = true;

        /// <summary>
        /// Create a new Control with a given parent.
        /// </summary>
        /// <param name="parent">The Widget's parent.</param>
        public Control(Widget parent) : base(parent)
        {
            Style.Parent = Widget.Styles.Control;
        }
    }
}
