using System;
using System.Collections.Generic;
using BluEngine.Engine;
using BluEngine.ScreenManager.Screens;
using Microsoft.Xna.Framework;
using BluEngine.ScreenManager.Styles;
using Marzersoft.CSS;

namespace BluEngine.ScreenManager.Widgets
{
    /// <summary>
    /// A special type of widget that does not allow itself to be re-parented and maintains it's bounds at a fixed ratio.
    /// </summary>
    public sealed class ScreenWidget : Widget
    {       
        /// <summary>
        /// The ratio of this widget's width to it's parent's height.
        /// </summary>
        public override float ScreenRatio
        {
            get { return refWidth / refHeight; }
            set { RefWidth = (value < 0.4f || value > 2.5f ? 1.0f : value) * refHeight; }
        }

        /// <summary>
        /// The width used to calculate this widget's ScreenRatio.
        /// </summary>
        public float RefWidth
        {
            get { return refWidth; }
            set { refWidth = (value <= 0.0f || value > 3000.0f ? 1280.0f : value); Invalidate(); }
        }
        private float refWidth = 800.0f;

        /// <summary>
        /// The height used to calculate this widget's ScreenRatio.
        /// </summary>
        public float RefHeight
        {
            get { return refHeight; }
            set { refHeight = (value <= 0.0f || value > 3000.0f ? 720.0f : value); Invalidate(); }
        }
        private float refHeight = 600.0f;

        public override Widget Parent
        {
            get { return null; }
            set { ; }
        }

        public override Vector4 Bounds
        {
            get { return base.Bounds; }
            set { ; }
        }

        public override Vector4 CalculatedBoundsF
        {
            get { return base.CalculatedBoundsF; }
            set { ; }
        }

        public override Rectangle CalculatedBoundsI
        {
            get { return base.CalculatedBoundsI; }
            set { ; }
        }

        public override float Left
        {
            get { return base.Left; }
            set { ; }
        }

        public override float Top
        {
            get { return base.Top; }
            set { ; }
        }

        public override float Right
        {
            get { return base.Right; }
            set { ; }
        }

        public override float Bottom
        {
            get { return base.Bottom; }
            set { ; }
        }

        public override float Width
        {
            get { return base.Width; }
            set { ; }
        }

        public override float Height
        {
            get { return base.Height; }
            set { ; }
        }

        public override List<Type> Hierarchy
        {
            get
            {
                if (hierarchy == null)
                {
                    hierarchy = new List<Type>();
                    hierarchy.Add(typeof(ScreenWidget));
                    hierarchy.AddRange(base.Hierarchy);
                }
                return hierarchy;
            }
        }
        private static List<Type> hierarchy = null;

        /// <summary>
        /// Create a new ScreenWidget.
        /// </summary>
        ///<param name="widgetScreen">The WidgetScreen this will be associated with.</param>
        public ScreenWidget(WidgetScreen widgetScreen)
            : base(widgetScreen) { }

        public override void Refresh()
        {
            IScreenDimensionsProvider dimensionsProvider = DimensionsProvider;
            
            float baseRatio = dimensionsProvider.ScreenRatio;
            float screenWidthRatio = ScreenRatio;
            if (screenWidthRatio == baseRatio) //the same proportions
                base.Bounds = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
            else if (screenWidthRatio < baseRatio) //"narrower" than the screen
            {
                float delta = 1.0f - (screenWidthRatio / baseRatio);
                base.Bounds = new Vector4(delta / 2.0f, 0.0f, 1.0f, 1.0f - delta);
            }
            else //"wider" than the screen
            {
                float delta = 1.0f - (baseRatio / screenWidthRatio);
                base.Bounds = new Vector4(0.0f, delta / 2.0f, 1.0f - delta, 1.0f);
            }

            base.Refresh();
        }
    }
}
