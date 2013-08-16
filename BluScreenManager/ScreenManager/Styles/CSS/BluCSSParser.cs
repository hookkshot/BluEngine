using System.Collections.Generic;
using BluEngine.ScreenManager.Screens;
using Marzersoft.CSS;
using Marzersoft.CSS.Interpreters;
using Marzersoft.CSS.Interpreters.Colors;
using System.Text.RegularExpressions;

namespace BluEngine.ScreenManager.Styles.CSS
{
	public class BluCSSParser : CSSParser
	{
        private ColorRGBAInterpreter rgba = null;
        public ColorRGBAInterpreter RGBA
        {
            get { return rgba; }
        }
        private ColorHSLAInterpreter hsla = null;
        public ColorHSLAInterpreter HSLA
        {
            get { return hsla; }
        }
        private ColorHexInterpreter hex = null;
        public ColorHexInterpreter Hex
        {
            get { return hex; }
        }
        private ColorKeywordInterpreter keyword = null;
        public ColorKeywordInterpreter Keyword
        {
            get { return keyword; }
        }
        
        public GameScreen ActiveScreen
        {
            get { return screen; }
            set { screen = value; }
        }
        private GameScreen screen = null;

        public bool DebuggerMode
        {
            get { return debuggerMode; }
            set { debuggerMode = value; }
        }
        private bool debuggerMode = false;

        public BluCSSParser(bool debuggerMode)
            : base(new List<NameInterpreter>())
        {
            this.debuggerMode = debuggerMode;

            rgba = new ColorRGBAInterpreter(this);
            hsla = new ColorHSLAInterpreter(this);
            hex = new ColorHexInterpreter(this);
            keyword = new ColorKeywordInterpreter(this);

            Interpreters.Add(new BorderInterpreter(this));
            Interpreters.Add(new DimensionInterpreter(this));
            Interpreters.Add(new PercentageInterpreter(this));
            Interpreters.Add(new ImageLayerInterpreter(this));
        }
        public BluCSSParser() : this(false) { }

	}

    public abstract class BluValueInterpreter : ValueInterpreter
    {
        private BluCSSParser parser = null;
        protected BluCSSParser BluCSSParser { get { return parser; } }

        public BluValueInterpreter(CSSParser parser, Regex regex)
            : base(parser, regex)
        {
            this.parser = parser as BluCSSParser;
        }
    }
}
