using System.Text.RegularExpressions;
using Marzersoft.CSS;
using System;
using Marzersoft.CSS.Interpreters;
using Marzersoft.CSS.Interpreters.Colors;
using Marzersoft.CSS.Interpreters.URIs;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.ScreenManager.Styles.CSS
{
    public class ImageLayerInterpreter : NameInterpreter
    {
        public ImageLayerInterpreter(CSSParser parser)
            : base(parser, new Regex(
            "layer-[0-" + (StyleSheet.STYLE_LAYERS - 1) + "]"
            //add more ImageLayer properties here as the needs for them in CSS grows
            ))
        {
            ValueInterpreters.Add(new ImageLayerRGBAInterpreter(parser));
            ValueInterpreters.Add(new ImageLayerHSLAInterpreter(parser));
            ValueInterpreters.Add(new ImageLayerHexInterpreter(parser));
            ValueInterpreters.Add(new ImageLayerKeywordInterpreter(parser));
            ValueInterpreters.Add(new ImageLayerURIInterpreter(parser));
        }
    }

    public class ImageLayerRGBAInterpreter : ColorRGBAInterpreter
    {
        public ImageLayerRGBAInterpreter(CSSParser parser) : base(parser) { }

        protected override IProperty InterpretInternal(String name, Match valueMatch)
        {
            Color cssColor = base.InterpretInternal(name, valueMatch) as Color;
            Microsoft.Xna.Framework.Color col = new Microsoft.Xna.Framework.Color((int)cssColor.R, (int)cssColor.G, (int)cssColor.B, (int)(cssColor.A * 255.0f));
            return new ImageLayer(name, (Parser as BluCSSParser).DebuggerMode ? null : SolidColours.TexFromColor(col));
        }
    }

    public class ImageLayerHSLAInterpreter : ColorHSLAInterpreter
    {
        public ImageLayerHSLAInterpreter(CSSParser parser) : base(parser) { }

        protected override IProperty InterpretInternal(String name, Match valueMatch)
        {
            Color cssColor = base.InterpretInternal(name, valueMatch) as Color;
            Microsoft.Xna.Framework.Color col = new Microsoft.Xna.Framework.Color((int)cssColor.R, (int)cssColor.G, (int)cssColor.B, (int)(cssColor.A * 255.0f));
            return new ImageLayer(name, (Parser as BluCSSParser).DebuggerMode ? null : SolidColours.TexFromColor(col));
        }
    }

    public class ImageLayerHexInterpreter : ColorHSLAInterpreter
    {
        public ImageLayerHexInterpreter(CSSParser parser) : base(parser) { }

        protected override IProperty InterpretInternal(String name, Match valueMatch)
        {
            Color cssColor = base.InterpretInternal(name, valueMatch) as Color;
            Microsoft.Xna.Framework.Color col = new Microsoft.Xna.Framework.Color((int)cssColor.R, (int)cssColor.G, (int)cssColor.B, (int)(cssColor.A * 255.0f));
            return new ImageLayer(name, (Parser as BluCSSParser).DebuggerMode ? null : SolidColours.TexFromColor(col));
        }
    }

    public class ImageLayerKeywordInterpreter : ColorHSLAInterpreter
    {
        public ImageLayerKeywordInterpreter(CSSParser parser) : base(parser) { }

        protected override IProperty InterpretInternal(String name, Match valueMatch)
        {
            Color cssColor = base.InterpretInternal(name, valueMatch) as Color;
            Microsoft.Xna.Framework.Color col = new Microsoft.Xna.Framework.Color((int)cssColor.R, (int)cssColor.G, (int)cssColor.B, (int)(cssColor.A * 255.0f));
            return new ImageLayer(name, (Parser as BluCSSParser).DebuggerMode ? null : SolidColours.TexFromColor(col));
        }
    }

    public class ImageLayerURIInterpreter : URIValueInterpreter
    {
        public ImageLayerURIInterpreter(CSSParser parser) : base(parser) { }

        protected override IProperty InterpretInternal(String name, Match valueMatch)
        {
            URI uri = base.InterpretInternal(name, valueMatch) as URI;
            BluCSSParser bluParser = (Parser as BluCSSParser);
            return new ImageLayer(name, bluParser.DebuggerMode ? null : bluParser.ActiveScreen.Content.Load<Texture2D>(uri.Value.Replace('/', '\\')));

        }
    }
}
