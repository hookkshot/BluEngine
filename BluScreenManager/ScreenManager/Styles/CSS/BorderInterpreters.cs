using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marzersoft.CSS.Interpreters;
using Marzersoft.CSS;
using System.Text.RegularExpressions;
using Marzersoft.CSS.Interpreters.Colors;

namespace BluEngine.ScreenManager.Styles.CSS
{
    public class BorderInterpreter : NameInterpreter
    {
        public BorderInterpreter(CSSParser parser)
            : base(parser, new Regex("border"))
        {
            ValueInterpreters.Add(new BorderValueInterpreter(parser));
        }

        public class BorderValueInterpreter : BluValueInterpreter
        {
            public BorderValueInterpreter(CSSParser parser)
                : base(parser, new Regex("([0-9]+)(?:px)?[ \t]+(none|dotted|dashed|double|solid|hidden)[ \t]+(" + CSSConstants.COLOUR + ")")) { }

            protected override IProperty InterpretInternal(String name, Match valueMatch)
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

                BorderLayer bl = null;
                if (!BluCSSParser.DebuggerMode)
                {
                    
                    Match match = CSSConstants.REGEX_COLOUR.Match(valueMatch.Groups[3].Value);
                    Color cssColor = null;
                    if ((match = CSSConstants.REGEX_COLOUR_RGBA.Match(valueMatch.Groups[3].Value)).Success)
                        cssColor = BluCSSParser.RGBA.Interpret(name, match.Value) as Color;
                    else if ((match = CSSConstants.REGEX_COLOUR_HSLA.Match(valueMatch.Groups[3].Value)).Success)
                        cssColor = BluCSSParser.HSLA.Interpret(name, match.Value) as Color;
                    else if ((match = CSSConstants.REGEX_COLOUR_HEX.Match(valueMatch.Groups[3].Value)).Success)
                        cssColor = BluCSSParser.Hex.Interpret(name, match.Value) as Color;
                    else if ((match = CSSConstants.REGEX_COLOUR_KEYWORD.Match(valueMatch.Groups[3].Value)).Success)
                        cssColor = BluCSSParser.Keyword.Interpret(name, match.Value) as Color;

                    //color
                    Microsoft.Xna.Framework.Color bc = new Microsoft.Xna.Framework.Color((int)cssColor.R, (int)cssColor.G, (int)cssColor.B, (int)(cssColor.A * 255.0f));
                    bl = new BorderLayer(name, bw, bs, bc);
                }
                else
                    bl = new BorderLayer(name, bw, bs);
                return bl;
            }
        }
    }
}
