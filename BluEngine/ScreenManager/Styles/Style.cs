﻿using System;
using System.Collections.Generic;

namespace BluEngine.ScreenManager.Styles
{
    public class Style
    {       
        private Dictionary<String, StyleAttributes> states;

        /// <summary>
        /// Access the StyleAttributes of the style according to the given state. Example: Widgets.Styles[typeof(Button)]["hover"] ...;
        /// </summary>
        /// <param name="state">The string ID of the state to access.</param>
        /// <returns>The StyleAttributes object for the state. If it did not exist it will be created.</returns>
        public StyleAttributes this[String state]
        {
            get
            {
                if (state == null || state.Length == 0)
                    return null;

                StyleAttributes attrs = null;
                if (!states.TryGetValue(state, out attrs))
                {
                    attrs = new StyleAttributes();
                    states.Add(state, attrs);
                }
                return attrs;
            }
        }

        /// <summary>
        /// Gets the StyleAttributes of the style according to the given state; unlike the [] indexer, this will NOT create a state that did not already exist.
        /// </summary>
        /// <param name="state">The string ID of the state to access.</param>
        /// <returns>The StyleAttributes object for the state, or null if it did not exist.</returns>
        public StyleAttributes Get(String state)
        {
            if (state == null || state.Length == 0)
                return null;

            StyleAttributes attrs = null;
            states.TryGetValue(state, out attrs);
            return attrs;
        }

        public void Delete(String attribute)
        {
            states.Remove(attribute);
        }

        public Style()
        {
            states = new Dictionary<String, StyleAttributes>();
        }

        /// <summary>
        /// Clears all custom styling information from the Style.
        /// </summary>
        public void Clear()
        {
            states.Clear();
        }


    }
}
