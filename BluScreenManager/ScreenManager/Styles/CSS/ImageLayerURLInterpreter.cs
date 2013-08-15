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
    public class ImageLayerURLInterpreter : CSSPropertyInterpreter
    {
        public ImageLayerURLInterpreter()
            : base(
            "layer-[0-"+(StyleSheet.STYLE_LAYERS-1)+"]",
            //add more ImageLayer properties here as the needs for them in CSS grows
            CSSConstants.URI
            ) { }

        protected override ICSSProperty TranslateValue(Match nameMatch, Match valueMatch)
        {
            BluCSSParser bluParser = (Parser as BluCSSParser);

            ImageLayer layer = new ImageLayer(bluParser.DebuggerMode ? null : bluParser.ActiveScreen.Content.Load<Texture2D>(valueMatch.Groups[1].Value.Replace('/', '\\')));
            layer.Name = nameMatch.Value;
            return layer; 
        }
    }
}
