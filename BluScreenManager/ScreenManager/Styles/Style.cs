using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using BluEngine.Engine;

namespace BluEngine.ScreenManager.Widgets
{
    /// <summary>
    /// Class containing all visual information for the drawing of a widget.
    /// Styles cascade down the hierarchy; the deepest version of an attribute in the style hierarchy will be the one used by the widget.
    /// Use these to alter widget appearance, particularly for state-based widgets (like buttons).
    /// </summary>
    public class Style : HierarchicalObject
    {       
        /// <summary>
        /// The number of ImageLayers permissible per style.
        /// </summary>
        public const int STYLE_LAYERS = 3;
        
        /// <summary>
        /// This style's parent. Any unset attributes in this style will be inherited from the parent (or it's parent, etc).
        /// </summary>
        new public Style Parent
        {
            get { return base.Parent == null ? null : base.Parent as Style; }
            set { base.Parent = value; }
        }

        /// <summary>
        /// The global transparency value used for the drawing of the widget.
        /// </summary>
        public float? Alpha
        {
            get { return alpha ?? (Parent != null ? Parent.Alpha : null); }
            set { alpha = value; }
        }
        private float? alpha = null;

        /// <summary>
        /// Set the ImageLayers of this Style, or Get the imagelayers of this or the parent(s).
        /// </summary>
        /// <param name="i">The index of the ImageLayer you want to manipulate, between Zero and Style.STYLE_LAYERS-1 (inclusive).</param>
        /// <returns>Setter: the ImageLayer of this style only (or null if none was set); Getter: The ImageLayer of this style or the first available parent one at the given index.</returns>
        public ImageLayer this[int i]
        {
            get
            {
                if (i < 0 || i >= STYLE_LAYERS)
                    throw new IndexOutOfRangeException("Attempt to retrieve a Style's imageLayer with index " + i);
                
                return imageLayers[i] ?? (Parent != null ? Parent[i] : null);
            }
            set
            {
                if (i < 0 || i >= STYLE_LAYERS)
                    throw new IndexOutOfRangeException("Attempt to set a Style's imageLayer with index " + i);
                imageLayers[i] = value;
            }
        }
        private ImageLayer[] imageLayers = new ImageLayer[STYLE_LAYERS];

        public Style(Style parent) : base(parent) { }
        public Style() : this(null) { }
    }
}
