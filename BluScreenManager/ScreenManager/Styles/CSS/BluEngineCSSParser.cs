using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marzersoft.CSS;
using BluEngine.ScreenManager.Screens;

namespace BluEngine.ScreenManager.Styles.CSS
{
	public class BluEngineCSSParser : CSSParser
	{
        public GameScreen ActiveScreen
        {
            get { return screen; }
            set { screen = value; }
        }
        private GameScreen screen = null;

        public BluEngineCSSParser(List<CSSPropertyInterpreter> interpreters) : base(interpreters) { }
	}
}
