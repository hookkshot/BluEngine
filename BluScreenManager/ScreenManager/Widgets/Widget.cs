using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BluEngine.Engine;
using Microsoft.Xna.Framework.Input;

namespace BluEngine.ScreenManager.Widgets
{
    /// <summary>
    /// A resolution-independant UI control.
    /// </summary>
    public class Widget : HierarchicalDrawable, IInvalidatable, IScreenDimensionsProvider
    {
        public delegate bool MouseEvent(Widget widget, Point mousePos);
        public delegate bool MouseButtonEvent(Widget widget, Point mousePos, int button);
        public delegate bool KeyEvent(Widget widget, Keys key);

        private static StyleSheet styles = new StyleSheet();
        private Vector4 bounds = new Vector4(0.0f,0.0f,1.0f,1.0f); //percentages of the parent control (W = Width, Z = Height)
        private Rectangle calcBoundsI; //actual bounds in screen dimensions (int)
        private Vector4 calcBoundsF; //actual bounds in screen dimensions (float)
        private bool valid = false;
        private Style style = new Style(Styles.Base);
        private HitFlags hitFlags = HitFlags.None;

        public event MouseEvent OnMouseEnter;
        public event MouseEvent OnMouseLeave;
        public event MouseButtonEvent OnMouseDown;
        public event MouseButtonEvent OnMouseUp;
        public event KeyEvent OnKeyDown;
        public event KeyEvent OnKeyUp;

        /// <summary>
        /// The widget's parent (container) widget.
        /// </summary>
        new public virtual Widget Parent
        {
            get { return base.Parent == null ? null : base.Parent as Widget; }
            set { base.Parent = value; }
        }

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
        /// Base styles inherited by all widgets.
        /// </summary>
        public static StyleSheet Styles
        {
            get { return styles; }
        }

        /// <summary>
        /// This style used by this widget when it is in a normal state.
        /// </summary>
        public Style Style
        {
            get { return style; }
        }

        /// <summary>
        /// The style currently being used by this widget, according to state.
        /// </summary>
        public virtual Style CurrentStyle
        {
            get { return style; }
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
            foreach (HierarchicalObject obj in Children)
            {
                Widget widget = obj as Widget;
                if (widget != null)
                    widget.Invalidate();
            }
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
        /// Create a new Widget with a given parent.
        /// </summary>
        /// <param name="parent">The Widget's parent.</param>
        public Widget(Widget parent) : base(parent) { }

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
            foreach (HierarchicalObject obj in Children)
            {
                Widget widget = obj as Widget;
                if (widget != null)
                    widget.UpdateAll(gameTime);
            }
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
            List<HierarchicalObject> children = Children;
            for (int i = children.Count - 1; i >= 0; i--)
            {
                Widget widget = children[i] as Widget;
                if (widget != null)
                {
                    Widget child = widget.ChildAtPoint(pt, hitflags);
                    if (child != null)
                        return child;
                }
            }
            return (HitFlags & hitflags) != 0 && CalculatedBoundsI.Contains(pt) ? this : null;
        }

        /// <summary>
        /// Draws this widget.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch passed in from the ScreenManager.</param>
        protected override void Draw(SpriteBatch spriteBatch)
        {
            if (Invalidated)
            {
                Refresh();
                valid = true;
            }

            Style currentStyle = CurrentStyle;
            float alpha = currentStyle.Alpha ?? 1.0f;

            for (int i = 0; i < Style.STYLE_LAYERS; i++)
            {
                ImageLayer layer = currentStyle[i];
                if (layer != null)
                    layer.Draw(spriteBatch,this,Color.White * alpha);
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
