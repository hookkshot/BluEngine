using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marzersoft.CSS;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.ScreenManager.Styles.CSS
{
    public class BorderLayerInterpreter : CSSPropertyInterpreter
    {
        public BorderLayerInterpreter()
            : base(
            "border"
            //add more BorderLayer properties here as the needs for them in CSS grows
            ,
            "([0-9]+)(?:px)?[ \t]+(none|dotted|dashed|double|solid|hidden)[ \t]+("+CSSConstants.COLOUR+")"
            ) { }

        protected override ICSSProperty TranslateValue(Match nameMatch, Match valueMatch)
        {
            BluCSSParser bluParser = (Parser as BluCSSParser);

            //width
            int bw = Int32.Parse(valueMatch.Groups[1].Value);
            
            //style
            BorderStyle bs = BorderStyle.None;
            switch (valueMatch.Groups[2].Value)
            {
                case "hidden": bs = BorderStyle.Hidden; break;
                case "dotted": bs = BorderStyle.Dotted; break;
                case "dashed": bs = BorderStyle.Dashed; break;
                case "solid": bs = BorderStyle.Solid; break;
                case "double": bs = BorderStyle.Double; break;
            }

            BorderLayer bl = null;
            if (!bluParser.DebuggerMode)
            {
                //color
                CSSColor cssColor = new CSSColorProperty("", valueMatch.Groups[3].Value).Value;
                Color bc = new Color((int)cssColor.R, (int)cssColor.G, (int)cssColor.B, (int)(cssColor.A * 255.0f));
                bl = new BorderLayer(bw, bs, bc);
            }
            else
                bl = new BorderLayer(bw, bs);
            bl.Name = nameMatch.Value;
            return bl;
        }
    }
}
