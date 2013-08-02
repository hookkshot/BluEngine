﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BluEngine.Engine;

namespace BluEngine.ScreenManager.Widgets
{
    /// <summary>
    /// A resolution-independant UI control.
    /// </summary>
    public class Widget : HierarchicalDrawable, IInvalidatable, IScreenDimensionsProvider
    {
        public delegate bool MouseEvent(int button, Point mousePos);
        public delegate bool KeyEvent(Keys key);
        
        private static Style baseStyle = new Style();
        private Vector4 bounds = new Vector4(0.0f,0.0f,1.0f,1.0f); //percentages of the parent control (W = Width, Z = Height)
        private Rectangle calcBoundsI; //actual bounds in screen dimensions (int)
        private Vector4 calcBoundsF; //actual bounds in screen dimensions (float)
        private bool valid = false;
        private Style style = new Style(Widget.BaseStyle);
        public event MouseEvent OnMouseDown;
        public event MouseEvent OnMouseUp;
        public event KeyEvent OnKeyDown;
        public event KeyEvent OnKeyUp;

        /// <summary>
        /// The widget's parent (container) widget.
        /// </summary>
        new public Widget Parent
        {
            get { return base.Parent == null ? null : base.Parent as Widget; }
            set { base.Parent = value; }
        }

        /// <summary>
        /// The IScreenDimensionsProvider object currently acting as the source resolution for this widget.
        /// </summary>
        public IScreenDimensionsProvider DimensionsProvider
        {
            get
            {
                Widget parent = Parent;
                return parent == null ? ScreenManager.Instance as IScreenDimensionsProvider : parent;
            }
        }

        /// <summary>
        /// Base style inherited by all widgets.
        /// </summary>
        public static Style BaseStyle
        {
            get { return baseStyle; }
        }

        /// <summary>
        /// If true, this widget's dimensions are in need of refreshing.
        /// </summary>
        public bool Invalidated
        {
            get { return valid; }
        }

        /// <summary>
        /// Flags this widget as in need of refreshing before next redraw.
        /// </summary>
        public virtual void Invalidate()
        {
            valid = false;
        }

        /// <summary>
        /// This widget's bounds in percentages, relative to the parent (or the Screen if parent is null; W = Width, Z = Height).
        /// Note that changes made here will not be reflected in the CalculatedBounds values until the next call to Update().
        /// </summary>
        public Vector4 Bounds
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
        public Vector4 CalculatedBoundsF
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
        public Rectangle CalculatedBoundsI
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
        public float Left
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
        public float Top
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
        public float Right
        {
            get { return bounds.X + bounds.W; }
            set { Left = value - bounds.W; }
        }

        /// <summary>
        /// The percentage Bottom of this widget.
        /// </summary>
        public float Bottom
        {
            get { return bounds.Y + bounds.Z; }
            set { Top = value - bounds.Z; }
        }

        /// <summary>
        /// The percentage width of this widget.
        /// </summary>
        public float Width
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
        public float Height
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
        /// Create a new Widget.
        /// </summary>
        public Widget() : this(null) { }

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

        protected override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D fill = style.Fill;
            if (fill != null)
                spriteBatch.Draw(fill, CalculatedBoundsI, Color.White);
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
    }
}
