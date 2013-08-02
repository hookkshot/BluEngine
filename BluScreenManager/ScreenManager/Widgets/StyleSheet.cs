using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BluEngine.ScreenManager.Widgets
{
    /// <summary>
    /// Container for styles used as a 'base' for individual control styles.
    /// </summary>
    public class StyleSheet
    {
        private Style baseStyle;
        private Style control;
        private Style button;
        private Style buttonDisabled;
        private Style buttonHover;
        private Style buttonDown;

        public StyleSheet()
        {
            //raw base style
            baseStyle = new Style();

            //control styles
            control = new Style(baseStyle);

            //button styles
            button = new Style(control) { Alpha = 0.5f };
            buttonDisabled = new Style(button);
            buttonDisabled.Overlay = new Texture2D(ScreenManager.Instance.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            buttonDisabled.Overlay.SetData<Color>(new Color[] { Color.DimGray * 0.3f });
            buttonHover = new Style(button) { Alpha = 1.0f };
            buttonDown = new Style(buttonHover);
            buttonDown.Overlay = new Texture2D(ScreenManager.Instance.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            buttonDown.Overlay.SetData<Color>(new Color[] { Color.White * 0.3f });
        }

        public Style Base { get { return baseStyle; } }
        public Style Control { get { return control; } }
        public Style Button { get { return button; } }
        public Style ButtonDisabled { get { return buttonDisabled; } }
        public Style ButtonHover { get { return buttonHover; } }
        public Style ButtonDown { get { return buttonDown; } }
    }
}
