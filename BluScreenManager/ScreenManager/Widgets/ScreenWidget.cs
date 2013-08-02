using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BluEngine.Engine;

namespace BluEngine.ScreenManager.Widgets
{
    /// <summary>
    /// A special type of widget that does not allow itself to be re-parented and maintains it's bounds at a fixed ratio.
    /// </summary>
    public class ScreenWidget : Widget
    {
        /// <summary>
        /// The ratio of this widget's width to it's parent's height.
        /// </summary>
        new public float ScreenRatio
        {
            get { return screenWidthRatio; }
            set
            {
                screenWidthRatio = value < 0.4f || value > 2.5f ? 1.0f : value;
                Invalidate();
            }
        }
        private float screenWidthRatio = 1024.0f / 768.0f;
        
        /// <summary>
        /// The widget's parent (container) widget.
        /// </summary>
        new public Widget Parent
        {
            get { return null; }
            set { return; }
        }

        new public Vector4 Bounds
        {
            get { return base.Bounds; }
            protected set { base.Bounds = value; }
        }

        new public Vector4 CalculatedBoundsF
        {
            get { return base.CalculatedBoundsF; }
        }

        new public Rectangle CalculatedBoundsI
        {
            get { return base.CalculatedBoundsI; }
        }

        new public float Left
        {
            get { return base.Left; }
        }

        new public float Top
        {
            get { return base.Top; }
        }

        new public float Right
        {
            get { return base.Right; }
        }

        new public float Bottom
        {
            get { return base.Bottom; }
        }

        new public float Width
        {
            get { return base.Width; }
        }

        new public float Height
        {
            get { return base.Height; }
        }

        public override void Refresh()
        {
            IScreenDimensionsProvider dimensionsProvider = DimensionsProvider;
            
            float baseRatio = dimensionsProvider.ScreenRatio;
            if (screenWidthRatio == baseRatio) //the same proportions
                Bounds = new Vector4(0.0f, 0.0f, 1.0f, 1.0f);
            else if (screenWidthRatio < baseRatio) //"narrower" than the screen
            {
                float delta = 1.0f - (screenWidthRatio / baseRatio);
                Bounds = new Vector4(delta / 2.0f, 0.0f, 1.0f, 1.0f - delta);
            }
            else //"wider" than the screen
            {
                float delta = 1.0f - (baseRatio / screenWidthRatio);
                Bounds = new Vector4(0.0f, delta / 2.0f, 1.0f - delta, 1.0f);
            }

            base.Refresh();
        }
    }
}
