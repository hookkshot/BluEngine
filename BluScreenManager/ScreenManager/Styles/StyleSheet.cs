using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BluEngine.Engine;
using BluEngine.ScreenManager.Widgets;

namespace BluEngine.ScreenManager.Styles
{
    public sealed class StyleSheet
    {
        private Dictionary<Type, Style> styles;
        private Style baseStyle;
        private List<Style> currentStyleHierarchy = null;
        private String[] currentStateList = null;

        public Style this[Type t]
        {
            get
            {
                if (t == null || (t != typeof(Widget) && !t.IsSubclassOf(typeof(Widget))))
                    return baseStyle;

                Style style = null;
                if (!styles.TryGetValue(t, out style))
                {
                    style = new Style();
                    styles.Add(t, style);
                }
                return style;
            }
        }

        public StyleSheet()
        {
            styles = new Dictionary<Type, Style>();
            baseStyle = new Style();
        }

        /// <summary>
        /// Gets the Style for the given Type; unlike the [] indexer, this will NOT create a Style that did not already exist.
        /// </summary>
        /// <param name="t">The Type of the style to access.</param>
        /// <returns>The Style object for the type, or null if it did not exist.</returns>
        public Style Get(Type t)
        {
            if (t == null || (t != typeof(Widget) && !t.IsSubclassOf(typeof(Widget))))
                return baseStyle;

            Style style = null;
            styles.TryGetValue(t, out style);
            return style;
        }

        public void StartLookup(Widget widget, String statelist)
        {
            //determine the style hierarchy
            currentStyleHierarchy = new List<Style>();
            if (widget != null)
            {
                if (widget.Style != null)
                    currentStyleHierarchy.Add(widget.Style);

                List<Type> typeHierarchy = widget.Hierarchy;
                foreach (Type type in typeHierarchy)
                {
                    Style style = Get(type);
                    if (style != null)
                        currentStyleHierarchy.Add(style);
                }
            }
            currentStyleHierarchy.Add(baseStyle); //base style

            //determine the state list
            if (statelist == null || statelist.Length == 0)
                statelist = "normal";
            if (!statelist.Contains("normal"))
                statelist += "|normal";

            currentStateList = statelist.Split(new char[]{'|'},StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Gets the Reference attribute from the set style hierarchy (set using StartLookup).
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve.</typeparam>
        /// <param name="attribute">The name of the attribute.</param>
        /// <returns>The attribute value, or null if it was not found or was an incompatible type.</returns>
        public T ReferenceAttributeLookup<T>(String attribute) where T : class
        {
            if (currentStateList == null || currentStyleHierarchy == null)
                return null;

            //walk the tree
            foreach (String currentState in currentStateList)
            {
                foreach (Style currentStyle in currentStyleHierarchy)
                {
                    StyleAttributes attrs = currentStyle.Get(currentState);
                    if (attrs != null)
                    {
                        object attr = attrs[attribute];
                        if (attr != null)
                        {
                            T ret = (attr as T);
                            if (ret != null)
                                return ret;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the Nullable attribute from the set style hierarchy (set using StartLookup).
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve.</typeparam>
        /// <param name="attribute">The name of the attribute.</param>
        /// <returns>The attribute value, or null if it was not found or was an incompatible type.</returns>
        public Nullable<T> ValueAttributeLookup<T>(String attribute) where T : struct
        {           
            if (currentStateList == null || currentStyleHierarchy == null)
                return null;

            //walk the tree
            foreach (String currentState in currentStateList)
            {
                foreach (Style currentStyle in currentStyleHierarchy)
                {
                    StyleAttributes attrs = currentStyle.Get(currentState);
                    if (attrs != null)
                    {
                        object attr = attrs[attribute];
                        if (attr != null)
                        {
                            try
                            {
                                return (T)attr;
                            }
                            catch
                            {
                                ; //keep searching through the hierarchy for a value that might work
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
