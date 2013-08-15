using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marzersoft.CSS;
using BluEngine.ScreenManager.Screens;

namespace BluEngine.ScreenManager.Styles.CSS
{
	public class BluCSSParser : CSSParser
	{
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

        public BluCSSParser(List<CSSPropertyInterpreter> interpreters, bool debuggerMode) : base(interpreters)
        {
            this.debuggerMode = debuggerMode;
        }
        public BluCSSParser(List<CSSPropertyInterpreter> interpreters) : this(interpreters,false) { }
	}
}
