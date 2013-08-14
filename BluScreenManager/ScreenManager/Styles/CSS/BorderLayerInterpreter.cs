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
            "border",
            "([0-9]+)(?:px)?[ \t]+(none|dotted|dashed|double|solid|hidden)[ \t]+("+CSSConstants.COLOUR+")"
            ) { }

        protected override ICSSProperty TranslateValue(Match nameMatch, Match valueMatch)
        {
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
            
            //color
            Color bc;
            try
            {
                CSSColor cssColor = new CSSColorProperty(nameMatch.Value, valueMatch.Groups[3].Value).Value;
                bc = new Color((int)cssColor.R, (int)cssColor.G, (int)cssColor.B, (int)(cssColor.A * 255.0f));
            }
            catch (ArgumentOutOfRangeException) //CSSColorProperty throws this if the input is invalid, so must be url
            {
                bc = Color.White;
            }

            BorderLayer bl = new BorderLayer(bw, bs, bc);
            bl.Name = nameMatch.Value;
            return bl;
        }
    }
}
