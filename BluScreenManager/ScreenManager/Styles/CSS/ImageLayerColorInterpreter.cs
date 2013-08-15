using System.Text.RegularExpressions;
using Marzersoft.CSS;
using Microsoft.Xna.Framework;

namespace BluEngine.ScreenManager.Styles.CSS
{
    public class ImageLayerColorInterpreter : CSSPropertyInterpreter
    {
        public ImageLayerColorInterpreter()
            : base(
            "layer-[0-" + (StyleSheet.STYLE_LAYERS - 1) + "]",
            //add more ImageLayer properties here as the needs for them in CSS grows
            CSSConstants.COLOUR
            ) { }

        protected override ICSSProperty TranslateValue(Match nameMatch, Match valueMatch)
        {
            BluCSSParser bluParser = (Parser as BluCSSParser);
            
            CSSColor cssColor = new CSSColorProperty("", valueMatch).Value;
            ImageLayer layer = new ImageLayer(bluParser.DebuggerMode ? null : SolidColours.TexFromColor(new Color((int)cssColor.R, (int)cssColor.G, (int)cssColor.B, (int)(cssColor.A * 255.0f))));
            layer.Name = nameMatch.Value;
            return layer;
        }

    }
}
