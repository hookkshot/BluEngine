using System;
using System.Collections.Generic;
using BluEngine.ScreenManager.Widgets;
using System.IO;
using Marzersoft.CSS;
using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Graphics;
using BluEngine.ScreenManager.Screens;

namespace BluEngine.ScreenManager.Styles
{
    public sealed class StyleSheet
    {
        public const int TYPE_IMAGELAYER = 100;

        /// <summary>
        /// <para>The maximum number of ImageLayers that may be used in a Style (accessed by "layer-x" where X is the 0-based layer index).</para>
        /// <para>There is of course nothing stopping you from assigning ImageLayers to whatever you like ("layer-dicks"), but the only ones that
        /// will actually get rendered by the Widget class's Draw() is "layer-0" to "layer-STYLE_LAYERS".</para>
        /// </summary>
        public const int STYLE_LAYERS = 5;
        
        private Dictionary<Type, Style> styles;
        private Style baseStyle;
        private List<Style> currentStyleHierarchy = null;
        private String[] currentStateList = null;
        private static Regex REGEX_LAYER_N_ALPHA = new Regex("layer-([0-9]+)-alpha");
        private static Regex REGEX_LAYER_N = new Regex("layer-([0-9]+)");
        private WidgetScreen screen;

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
        public StyleSheet(WidgetScreen owner)
        {
            if (owner == null)
                throw new ArgumentNullException("owner");
            screen = owner;
            styles = new Dictionary<Type, Style>();
            baseStyle = new Style();
        }

        /// <summary>
        /// Clears all custom styling information from the stylesheet.
        /// </summary>
        public void Clear()
        {
            styles.Clear();
            baseStyle.Clear();
            currentStyleHierarchy = null;
            currentStateList = null;
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
        public void StartLookup(Widget widget)
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
            currentStateList = widget.StateList;
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
                        T val = attrs.ReferenceAttributeLookup<T>(attribute);
                        if (val != null)
                            return val;
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
                        T? attVal = attrs.ValueAttributeLookup<T>(attribute);
                        if (attVal.HasValue)
                            return attVal.Value;
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
        /// Shortcut for looking up BorderLayer attributes.
        /// </summary>
        /// <param name="attribute">The name of the BorderLayer attribute to look up.</param>
        /// <returns>The BorderLayer, or null.</returns>
        public BorderLayer BorderLayerLookup(String attribute)
        {
            return ReferenceAttributeLookup<BorderLayer>(attribute);
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
        /// Shortcut for looking up Float attributes.
        /// </summary>
        /// <param name="attribute">The name of the Float attribute to look up.</param>
        /// <returns>The attribute value or null.</returns>
        public float? FloatLookup(String attribute)
        {
            return ValueAttributeLookup<float>(attribute);
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

        /// <summary>
        /// Shortcut for looking up Color attributes.
        /// </summary>
        /// <param name="attribute">The name of the Color attribute to look up.</param>
        /// <param name="fallback">The value to use as default if the attribute wasn't found.</param>
        /// <returns>The attribute value or the default.</returns>
        public Color ColorLookup(String attribute, Color fallback)
        {
            return ValueAttributeLookup<Color>(attribute) ?? fallback;
        }

        /// <summary>
        /// Shortcut for looking up Color attributes.
        /// </summary>
        /// <param name="attribute">The name of the Color attribute to look up.</param>
        /// <returns>The attribute value or null.</returns>
        public Color? ColorLookup(String attribute)
        {
            return ValueAttributeLookup<Color>(attribute);
        }

        /// <summary>
        /// Handles the translating of CSS rulesets to WidgetStyle values.
        /// </summary>
        /// <param name="widget">The Widget object that is the recipient.</param>
        /// <param name="state">The state of the recipient.</param>
        /// <param name="ruleset">The ruleset to parse.</param>
        public void ApplyCSSStylesToWidget(Widget widget, String state, List<ICSSProperty> ruleset)
        {
            ApplyCSSStyles(widget.Style[state],ruleset);
            widget.ApplyStateBasedStyles();
        }

        /// <summary>
        /// Handles the translating of CSS rulesets to WidgetStyle values.
        /// </summary>
        /// <param name="widget">The Widget object that is the recipient.</param>
        /// <param name="state">The state of the recipient.</param>
        /// <param name="ruleset">The ruleset to parse.</param>
        public void ApplyCSSStylesToType(Type type, String state, List<ICSSProperty> ruleset)
        {
            ApplyCSSStyles(this[type][state], ruleset);

            //check if we've applied ScreenWidget-specific ref-width/ref-height values
            if (type == typeof(ScreenWidget))
            {
                Style style = this.Get(typeof(ScreenWidget));
                if (style != null)
                {
                    StyleAttributes attrs = style.Get("normal");
                    if (attrs != null)
                    {

                        float? refWidth = attrs.ValueAttributeLookup<float>("ref-width");
                        if (refWidth.HasValue)
                            screen.Base.RefWidth = refWidth.Value;
                        float? refHeight = attrs.ValueAttributeLookup<float>("ref-height");
                        if (refHeight.HasValue)
                            screen.Base.RefHeight = refHeight.Value;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the translating of CSS rulesets to WidgetStyle values.
        /// </summary>
        /// <param name="state">The StyleAttributes object that is the recipient.</param>
        /// <param name="ruleset">The ruleset to parse.</param>
        public void ApplyCSSStyles(StyleAttributes state, List<ICSSProperty> ruleset)
        {
            foreach (ICSSProperty property in ruleset)
            {
                switch (property.PropertyType)
                {
                    case StyleSheet.TYPE_IMAGELAYER:
                        state[property.Name] = property;
                        break;

                    case CSSConstants.TYPE_COLOR:
                        CSSColor cssColor = (property as CSSColorProperty).Value;
                        Color color = new Color((int)cssColor.R, (int)cssColor.G, (int)cssColor.B, (int)(cssColor.A*255.0f));
                        state[property.Name] = color;
                        break;

                    case CSSConstants.TYPE_NUMBER:
                        state[property.Name] = (property as CSSNumberProperty).Value;
                        break;

                    case CSSConstants.TYPE_STRING:
                        state[property.Name] = (property as CSSStringProperty).Value;
                        break;
                }
            }
        }
    }
}
