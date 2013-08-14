using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marzersoft.CSS;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BluEngine.ScreenManager.Screens;

namespace BluEngine.ScreenManager.Styles.CSS
{
    public class ImageLayerInterpreter : CSSPropertyInterpreter
    {
        public ImageLayerInterpreter()
            : base(
            "layer-[0-"+(StyleSheet.STYLE_LAYERS-1)+"]",
            "(?:" + CSSConstants.URI + ")|(?:" + CSSConstants.COLOUR + ")"
            ) { }

        protected override ICSSProperty TranslateValue(Match nameMatch, Match valueMatch)
        {
            Texture2D tex = null;
            try
            {
                CSSColor cssColor = new CSSColorProperty(nameMatch.Value,valueMatch).Value;
                Color color = new Color((int)cssColor.R, (int)cssColor.G, (int)cssColor.B, (int)(cssColor.A*255.0f));
                tex = new Texture2D(ScreenManager.Instance.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                tex.SetData<Color>(new Color[] { color });
            }
            catch (ArgumentOutOfRangeException) //CSSColorProperty throws this if the input is invalid, so must be url
            {
                tex = (Parser as BluEngineCSSParser).ActiveScreen.Content.Load<Texture2D>(valueMatch.Groups[1].Value.Replace('/', '\\'));
            }

            ImageLayer layer = new ImageLayer(tex);
            layer.Name = nameMatch.Value;
            return layer; 
        }
    }
}
