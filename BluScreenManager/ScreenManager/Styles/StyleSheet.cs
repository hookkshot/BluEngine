using System;
using System.Collections.Generic;
using BluEngine.ScreenManager.Widgets;
using System.IO;
using Marzersoft.CSS;

namespace BluEngine.ScreenManager.Styles
{
    public sealed class StyleSheet
    {
        private Dictionary<Type, Style> styles;
        private Style baseStyle;
        private List<Style> currentStyleHierarchy = null;
        private String[] currentStateList = null;

        /// <summary>
        /// <para>The Widget Style associated with the given Type.</para>
        /// </summary>
        /// <param name="t">The type for which you'd like to look up the style.</param>
        /// <returns>The style associated with the passed Type. Every style inherits from a Type-less base Style, so if you pass a null type or a type that does not inherit from Widget, the base style will be returned.</returns>
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

        /// <summary>
        /// Create a new Stylesheet instance.
        /// </summary>
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

        /// <summary>
        /// Sets the lookup state hierarchy information according to the provided widget and/or statelist.
        /// </summary>
        /// <param name="widget">A widget from which to take a hierarchy.</param>
        /// <param name="statelist">A statelist string from which to build a descending-precedence list of states.</param>
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

        /// <summary>
        /// Shortcut for looking up ImageLayer attributes.
        /// </summary>
        /// <param name="attribute">The name of the ImageLayer attribute to look up.</param>
        /// <returns>The ImageLayer, or null.</returns>
        public ImageLayer ImageLayerLookup(String attribute)
        {
            return ReferenceAttributeLookup<ImageLayer>(attribute);
        }

        /// <summary>
        /// Shortcut for looking up Float attributes.
        /// </summary>
        /// <param name="attribute">The name of the Float attribute to look up.</param>
        /// <param name="fallback">The value to use as default if the attribute wasn't found.</param>
        /// <returns>The attribute value or the default.</returns>
        public float FloatLookup(String attribute, float fallback)
        {
            return ValueAttributeLookup<float>(attribute) ?? fallback;
        }
        /// <summary>
        /// Shortcut for looking up Double attributes.
        /// </summary>
        /// <param name="attribute">The name of the Double attribute to look up.</param>
        /// <param name="fallback">The value to use as default if the attribute wasn't found.</param>
        /// <returns>The attribute value or the default.</returns>
        public double DoubleLookup(String attribute, double fallback)
        {
            return ValueAttributeLookup<double>(attribute) ?? fallback;
        }

        /// <summary>
        /// Shortcut for looking up Integer attributes.
        /// </summary>
        /// <param name="attribute">The name of the Integer attribute to look up.</param>
        /// <param name="fallback">The value to use as default if the attribute wasn't found.</param>
        /// <returns>The attribute value or the default.</returns>
        public int IntLookup(String attribute, int fallback)
        {
            return ValueAttributeLookup<int>(attribute) ?? fallback;
        }

        /// <summary>
        /// Shortcut for looking up Char attributes.
        /// </summary>
        /// <param name="attribute">The name of the Char attribute to look up.</param>
        /// <param name="fallback">The value to use as default if the attribute wasn't found.</param>
        /// <returns>The attribute value or the default.</returns>
        public char CharLookup(String attribute, char fallback)
        {
            return ValueAttributeLookup<char>(attribute) ?? fallback;
        }

        public bool LoadCSSFile(string name)
        {
            CSSDocument document = new CSSDocument("Content\\Styles\\" + name + ".css", true);
            CSSRulesets rulesets = document.Rulesets;

            foreach (KeyValuePair<String, CSSRuleset> ruleset in rulesets)
            {
                Console.WriteLine("Ruleset: {0} ({1} properties)", ruleset.Key, ruleset.Value.Count);
                String[] selector = ruleset.Key.Substring(1).Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                name = selector[0];
                String state = selector.Length > 1 ? selector[1] : "normal";
                Console.WriteLine("  Name: {0}\n  State: {1}", name, state);

                foreach (KeyValuePair<string, string> rule in ruleset.Value)
                {
                    Console.WriteLine("    {0}: {1}", rule.Key, rule.Value);

                    if (ruleset.Key[0] == '#') //it's an implicit Type style
                    {
                        Console.WriteLine("a");
                        Type type = Type.GetType(name + ", BluEngine");
                        if (type != null)
                        {
                            Console.WriteLine("b");
                            this[type][state][rule.Key] = rule.Value;
                        }

                    }
                    else if (ruleset.Key[0] == '.') //it's an explicit instance style
                    {

                    }
                }
            }
            return true;
        }
    }
}
