using System;
using System.Collections.Generic;
using BluEngine.Engine;
using BluEngine.ScreenManager.Screens;
using BluEngine.ScreenManager.Styles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BluEngine.ScreenManager.Widgets
{
    /// <summary>
    /// A resolution-independant UI control.
    /// </summary>
    public class Widget : IInvalidatable, IScreenDimensionsProvider
    {
        public delegate bool MouseEvent(Widget widget, Point mousePos);
        public delegate bool MouseButtonEvent(Widget widget, Point mousePos, int button);
        public delegate bool KeyEvent(Widget widget, Keys key);

        private Vector4 bounds = new Vector4(0.0f,0.0f,1.0f,1.0f); //percentages of the parent control (W = Width, Z = Height)
        private Rectangle calcBoundsI; //actual bounds in screen dimensions (int)
        private Vector4 calcBoundsF; //actual bounds in screen dimensions (float)
        private bool valid = false;
        private HitFlags hitFlags = HitFlags.None;

        public event MouseEvent OnMouseEnter;
        public event MouseEvent OnMouseLeave;
        public event MouseButtonEvent OnMouseDown;
        public event MouseButtonEvent OnMouseUp;
        public event KeyEvent OnKeyDown;
        public event KeyEvent OnKeyUp;

        /// <summary>
        /// The WidgetScreen that this widget (and it's hierarchy) belongs to.
        /// </summary>
        public WidgetScreen Screen
        {
            get { return widgetScreen; }
            private set { widgetScreen = value; }
        }
        private WidgetScreen widgetScreen = null;

        /// <summary>
        /// A list representing the widget class hierarchy for this widget up to (and including) itself.
        /// </summary>
        public virtual List<Type> Hierarchy
        {
            get
            {
                if (hierarchy == null)
                {
                    hierarchy = new List<Type>();
                    hierarchy.Add(typeof(Widget));
                }
                return hierarchy;
            }
        }
        private static List<Type> hierarchy = null;

        /// <summary>
        /// The list of children belonging to this widget. Do not modify this list directly!
        /// </summary>
        protected List<Widget> Children
        {
            get { return children; }
        }
        private List<Widget> children = new List<Widget>();

        /// <summary>
        /// This widget's parent in the hierarchy.
        /// Will throw an InvalidOperationException if an attempt to create a hierarchical loop is made.
        /// </summary>
        public virtual Widget Parent
        {
            get { return parent; }
            set
            {
                if (parent == value)
                    return;

                if (value != null && value.isAncestor(this))
                    throw new InvalidOperationException("You cannot set a Widget's parent to one of it's children!");

                if (parent != null)
                    parent.children.Remove(this);
                parent = value;
                if (parent != null)
                    parent.children.Add(this);
            }
        }
        private Widget parent;

        /// <summary>
        /// The IScreenDimensionsProvider object currently acting as the source resolution for this widget.
        /// </summary>
        public virtual IScreenDimensionsProvider DimensionsProvider
        {
            get
            {
                Widget parent = Parent;
                return parent == null ? ScreenManager.Instance as IScreenDimensionsProvider : parent;
            }
        }

        /// <summary>
        /// A mask containing the set of flags this widget should respond to for ChildAtPoint calls.
        /// </summary>
        public HitFlags HitFlags
        {
            get { return hitFlags; }
            set { hitFlags = value; }
        }

        /// <summary>
        /// This widget's Style["normal"]["tint"], an XNA Color object.
        /// </summary>
        public Color Tint
        {
            get
            {
                object attr = Style["normal"]["tint"];
                Color val = Color.White;
                if (attr != null)
                {
                    try
                    {
                        val = (Color)attr;
                    }
                    catch
                    {
                        ;
                    }
                }
                return val;
            }
            set
            {
                Style["normal"]["tint"] = value;
            }
        }

        /// <summary>
        /// Gets one of this widget's Style["normal"] percentage float values.
        /// </summary>
        /// <param name="property">The property to retrieve.</param>
        /// <param name="fallback">A fallback value to use if the property wasn't found.</param>
        /// <returns>The property's value</returns>
        private float GetPercentageStyleValue(String property, float fallback)
        {
            object attr = Style["normal"][property];
            float val = fallback;
            if (attr != null)
            {
                try
                {
                    val = Math.Min(Math.Max((float)attr, 0.0f), 1.0f);
                }
                catch //cast exception
                {
                    ;
                }
            }
            return val;
        }

        /// <summary>
        /// Sets one of this widget's Style["normal"] percentage float values.
        /// </summary>
        /// <param name="property">The property to set.</param>
        /// <param name="value">The new value.</param>
        private void SetPercentageStyleValue(String property, float value)
        {
            Style["normal"][property] = Math.Min(Math.Max(value, 0.0f), 1.0f);
        }

        /// <summary>
        /// This widget's Style["normal"]["tint-strength"], between 0.0f and 1.0f.
        /// </summary>
        public float TintStrength
        {
            get { return GetPercentageStyleValue("tint-strength", 1.0f); }
            set { SetPercentageStyleValue("tint-strength", value); }
        }

        /// <summary>
        /// This widget's Style["normal"]["alpha"], between 0.0f and 1.0f.
        /// </summary>
        public float Alpha
        {
            get { return GetPercentageStyleValue("alpha", 1.0f); }
            set { SetPercentageStyleValue("alpha", value); }
        }


        /// <summary>
        /// Get this widget's Style["normal"]["layer-N-alpha"], between 0.0f and 1.0f.
        /// </summary>
        /// <param name="N">The N layer index.</param>
        public float GetLayerNAlpha(int N)
        {
            if (N < 0 || N >= Style.STYLE_LAYERS)
                return 1.0f;
            return GetPercentageStyleValue("layer-"+N+"-alpha", 1.0f);
        }

        /// <summary>
        /// Set this widget's Style["normal"]["layer-N-alpha"], between 0.0f and 1.0f.
        /// </summary>
        /// <param name="N">The N layer index.</param>
        /// <param name="value">The new value.</param>
        public void SetLayerNAlpha(int N, float value)
        {
            if (N < 0 || N >= Style.STYLE_LAYERS)
                return;
            SetPercentageStyleValue("layer-" + N + "-alpha", 1.0f);
        }

        /// <summary>
        /// This widget's individal style object. Use this to alter style attributes for a specific widget without modifying the global styles.
        /// </summary>
        public Style Style
        {
            get
            {
                if (style == null)
                    style = new Style();
                return style;
            }
        }
        private Style style = null;

        /// <summary>
        /// String representing the statelist of this widget (used for styles). Multiple states may be represented in descending order of precedence by separating them with a bar, e.g. "down|hover|normal".
        /// </summary>
        public virtual String State
        {
            get { return "normal"; }
        }

        /// <summary>
        /// If true, this widget's dimensions are in need of refreshing.
        /// </summary>
        public bool Invalidated
        {
            get { return !valid; }
        }

        /// <summary>
        /// Flags this widget (and all children) as in need of refreshing before next redraw.
        /// </summary>
        public void Invalidate()
        {
            valid = false;
            foreach (Widget child in children)
                child.Invalidate();
        }

        /// <summary>
        /// This widget's bounds in percentages, relative to the parent (or the Screen if parent is null; W = Width, Z = Height).
        /// Note that changes made here will not be reflected in the CalculatedBounds values until the next call to Update().
        /// </summary>
        public virtual Vector4 Bounds
        {
            get { return bounds; }
            set
            {
                bounds.X = value.X;
                bounds.Y = value.Y;
                bounds.W = Math.Max(value.W, 0.0f);
                bounds.Z = Math.Max(value.Z, 0.0f);
                Invalidate();
            }
        }

        /// <summary>
        /// This widget's bounds in pixels as floats, relative to the parent (or the Screen if parent is null; W = Width, Z = Height).
        /// Note that changes made here will not be reflected in the CalculatedBounds values until the next call to Update().
        /// </summary>
        public virtual Vector4 CalculatedBoundsF
        {
            get { return calcBoundsF; }
            set
            {
                IScreenDimensionsProvider dimensionsProvider = DimensionsProvider;
                bounds = new Vector4(
                    value.X / dimensionsProvider.ScreenWidth,
                    value.Y / dimensionsProvider.ScreenHeight,
                    value.W / dimensionsProvider.ScreenWidth,
                    value.Z / dimensionsProvider.ScreenHeight
                    );
                Invalidate();
            }
        }

        /// <summary>
        /// This widget's bounds in pixels as integers, relative to the parent (or the Screen if parent is null; W = Width, Z = Height).
        /// Note that changes made here will not be reflected in the CalculatedBounds values until the next call to Update().
        /// </summary>
        public virtual Rectangle CalculatedBoundsI
        {
            get { return calcBoundsI; }
            set
            {
                IScreenDimensionsProvider dimensionsProvider = DimensionsProvider;
                bounds = new Vector4(
                    (float)value.X / dimensionsProvider.ScreenWidth,
                    (float)value.Y / dimensionsProvider.ScreenHeight,
                    (float)value.Width / dimensionsProvider.ScreenWidth,
                    (float)value.Height / dimensionsProvider.ScreenHeight
                    );
                Invalidate();
            }
        }

        /// <summary>
        /// The percentage Left of this widget.
        /// </summary>
        public virtual float Left
        {
            get { return bounds.X; }
            set
            {
                bounds.X = value;
                Invalidate();
            }
        }

        /// <summary>
        /// The percentage Top of this widget.
        /// </summary>
        public virtual float Top
        {
            get { return bounds.Y; }
            set
            {
                bounds.Y = value;
                Invalidate();
            }
        }

        /// <summary>
        /// The percentage Right of this widget.
        /// </summary>
        public virtual float Right
        {
            get { return bounds.X + bounds.W; }
            set { float diff = value - bounds.W; bounds.W += diff; bounds.X -= diff; }
        }

        /// <summary>
        /// The percentage Bottom of this widget.
        /// </summary>
        public virtual float Bottom
        {
            get { return bounds.Y + bounds.Z; }
            set { float diff = value - bounds.Z; bounds.Z += diff; bounds.Y -= diff; }
        }

        /// <summary>
        /// The percentage width of this widget.
        /// </summary>
        public virtual float Width
        {
            get { return bounds.W; }
            set
            {
                bounds.W = Math.Max(value, 0.0f);
                Invalidate();
            }
        }

        /// <summary>
        /// The percentage Height of this widget.
        /// </summary>
        public virtual float Height
        {
            get { return bounds.Z; }
            set
            {
                bounds.Z = Math.Max(value, 0.0f);
                Invalidate();
            }
        }

        /// <summary>
        /// Create a new Widget.
        /// </summary>
        /// <param name="screen">The Widget's screen.</param>
        /// <param name="parent">The Widget's parent.</param>
        private Widget(WidgetScreen screen, Widget parent)
        {
            if (screen == null)
                throw new ArgumentNullException("screen");
            Screen = screen;
            Parent = parent;
        }

        /// <summary>
        /// Create a new Widget.
        /// </summary>
        /// <param name="screen">The Widget's screen.</param>
        protected Widget(WidgetScreen screen) : this(screen, null) { }

        /// <summary>
        /// Create a new Widget.
        /// </summary>
        /// <param name="parent">The Widget's parent.</param>
        public Widget(Widget parent) : this(parent.Screen, parent) { }

        /// <summary>
        /// Check if a Widget is an ancestor of this one.
        /// </summary>
        /// <param name="potentialAncestor">The widget to check.</param>
        /// <returns>True if potentialAncestor was in the parent hierarchy of the current Widget.</returns>
        public bool isAncestor(Widget potentialAncestor)
        {
            if (potentialAncestor == null)
                return false;

            if (potentialAncestor == this)
                return true;

            return parent == null ? false : parent.isAncestor(potentialAncestor);
        }



        /// <summary>
        /// Refresh resolution-dependant properties.
        /// </summary>
        public virtual void Refresh()
        {
            IScreenDimensionsProvider dimensionsProvider = DimensionsProvider;
            calcBoundsF.X = dimensionsProvider.ScreenX + dimensionsProvider.ScreenWidth * bounds.X;
            calcBoundsF.Y = dimensionsProvider.ScreenY + dimensionsProvider.ScreenHeight * bounds.Y;
            calcBoundsF.W = dimensionsProvider.ScreenWidth * bounds.W;
            calcBoundsF.Z = dimensionsProvider.ScreenHeight * bounds.Z;

            calcBoundsI.X = (int)calcBoundsF.X;
            calcBoundsI.Y = (int)calcBoundsF.Y;
            calcBoundsI.Width = (int)calcBoundsF.W;
            calcBoundsI.Height = (int)calcBoundsF.Z;
        }

        /// <summary>
        /// Does updating logic on this widget, then on children. Call this once on your base object.
        /// </summary>
        /// <param name="gameTime">The gametime passed since the last frame.</param>
        public void UpdateAll (GameTime gameTime)
        {
            Update(gameTime);
            foreach (Widget child in children)
                child.UpdateAll(gameTime);
        }

        /// <summary>
        /// Does updating logic on this widget.
        /// You do not need to call this directly; it is called hierarchically by the base object via UpdateAll().
        /// </summary>
        /// <param name="gameTime">The gametime passed since the last frame.</param>
        protected virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Return the top-most widget at the given screen coords.
        /// </summary>
        /// <param name="pt">The screen coords.</param>
        /// <param name="hitflags">The set of flags to check for.</param>
        /// <returns>The found widget, or null.</returns>
        public Widget ChildAtPoint(Point pt, HitFlags hitflags)
        {
            for (int i = children.Count - 1; i >= 0; i--)
            {
                Widget child = children[i].ChildAtPoint(pt, hitflags);
                if (child != null)
                    return child;
            }
            return (HitFlags & hitflags) != 0 && CalculatedBoundsI.Contains(pt) ? this : null;
        }

        /// <summary>
        /// Draws itself then all children. If you are using a WidgetScreen you will never need to call this directly, otherwise only call this once on your base widget.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch object used for drawing.</param>
        public void DrawAll(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch);
            foreach (Widget child in children)
                child.DrawAll(spriteBatch);
        }

        /// <summary>
        /// Draws this widget.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch passed in from the ScreenManager.</param>
        protected virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Invalidated)
            {
                Refresh();
                valid = true;
            }

            //set the target hierarchy to that of this widget
            Screen.Styles.StartLookup(this, State);

            //alpha
            float alpha = Screen.Styles.FloatLookup("alpha", 1.0f);

            //color tint
            Color tint = Screen.Styles.ColorLookup("tint", Color.White);

            //tint strength
            float tintStrength = Screen.Styles.FloatLookup("tint-strength", 1.0f);

            float delta = 255.0f * (1.0f - tintStrength);
            int R = (int)(((float)tint.R * tintStrength) + delta);
            int G = (int)(((float)tint.G * tintStrength) + delta);
            int B = (int)(((float)tint.B * tintStrength) + delta);
            tint = new Color(R, G, B);

            //layers
            for (int i = 0; i < Style.STYLE_LAYERS; i++)
            {
                ImageLayer layer = Screen.Styles.ImageLayerLookup("layer-" + i);
                float layerAlpha = Screen.Styles.FloatLookup("layer-"+i+"-alpha", 1.0f);

                if (layer != null)
                    layer.Draw(spriteBatch, this, tint * alpha * layerAlpha);
            }
        }

        /// <summary>
        /// Function called when this Widget recieves a MouseEnter event from the WidgetScreen base.
        /// </summary>
        /// <param name="pt">The screen coords of the event.</param>
        /// <returns>True if this event has been handled and should not be passed down to a "world" layer (if one is present), false otherwise.</returns>
        public virtual bool MouseEnter(Point pt)
        {
            return OnMouseEnter != null ? OnMouseEnter(this, pt) : false;
        }

        /// <summary>
        /// Function called when this Widget recieves a MouseLeave event from the WidgetScreen base.
        /// </summary>
        /// <param name="pt">The screen coords of the event.</param>
        /// <returns>True if this event has been handled and should not be passed down to a "world" layer (if one is present), false otherwise.</returns>
        public virtual bool MouseLeave(Point pt)
        {
            return OnMouseLeave != null ? OnMouseLeave(this, pt) : false;
        }

        /// <summary>
        /// Function called when this Widget recieves a MouseDown event from the WidgetScreen base.
        /// </summary>
        /// <param name="pt">The screen coords of the event.</param>
        /// <param name="button">The 1-indexed number of the mouse button according to the constants defined in InputControl.</param>
        /// <returns>True if this event has been handled and should not be passed down to a "world" layer (if one is present).</returns>
        public virtual bool MouseDown(Point pt, int button)
        {
            return OnMouseDown != null ? OnMouseDown(this, pt, button) : false;
        }

        /// <summary>
        /// Function called when this Widget recieves a MouseUp event from the WidgetScreen base.
        /// </summary>
        /// <param name="pt">The screen coords of the event.</param>
        /// <param name="button">The 1-indexed number of the mouse button according to the constants defined in InputControl.</param>
        /// <returns>True if this event has been handled and should not be passed down to a "world" layer (if one is present).</returns>
        public virtual bool MouseUp(Point pt, int button)
        {
            return OnMouseUp != null ? OnMouseUp(this, pt, button) : false;
        }

        /// <summary>
        /// Function called when this Widget recieves a KeyDown event from the WidgetScreen base.
        /// </summary>
        /// <param name="key">The key that was pressed.</param>
        /// <returns>True if this event has been handled and should not be passed down to a "world" layer (if one is present).</returns>
        public virtual bool KeyDown(Keys key)
        {
            return OnKeyDown != null ? OnKeyDown(this, key) : false;
        }

        /// <summary>
        /// Function called when this Widget recieves a KeyUp event from the WidgetScreen base.
        /// </summary>
        /// <param name="key">The key that was released.</param>
        /// <returns>True if this event has been handled and should not be passed down to a "world" layer (if one is present).</returns>
        public virtual bool KeyUp(Keys key)
        {
            return OnKeyUp != null ? OnKeyUp(this, key) : false;
        }

        public float ScreenX
        {
            get { return CalculatedBoundsF.X; }
        }

        public float ScreenY
        {
            get { return CalculatedBoundsF.Y; }
        }

        public float ScreenWidth
        {
            get { return CalculatedBoundsF.W; }
        }

        public float ScreenHeight
        {
            get { return CalculatedBoundsF.Z; }
        }

        public virtual float ScreenRatio
        {
            get { return CalculatedBoundsF.W / CalculatedBoundsF.Z; }
            set { ; }
        }
    }
}
