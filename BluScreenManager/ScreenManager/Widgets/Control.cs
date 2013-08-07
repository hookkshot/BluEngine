using System;
using System.Collections.Generic;

namespace BluEngine.ScreenManager.Widgets
{
    /// <summary>
    /// Base class for a user input control widget.
    /// </summary>
    public class Control : Widget
    {
        public override List<Type> Hierarchy
        {
            get
            {
                if (hierarchy == null)
                {
                    hierarchy = new List<Type>();
                    hierarchy.Add(typeof(Control));
                    hierarchy.AddRange(base.Hierarchy);
                }
                return hierarchy;
            }
        }
        private static List<Type> hierarchy = null;
        
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
        /// <param name="parent">The Control's parent.</param>
        public Control(Widget parent) : base(parent) { }
    }
}
