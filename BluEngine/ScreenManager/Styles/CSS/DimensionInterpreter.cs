using Marzersoft.CSS;
using System.Text.RegularExpressions;
using Marzersoft.CSS.Interpreters.Numbers;
using Marzersoft.CSS.Interpreters;

namespace BluEngine.ScreenManager.Styles.CSS
{
    public class DimensionInterpreter : NameInterpreter
    {
        public DimensionInterpreter(CSSParser parser)
            : base(parser, new Regex(
            "left"
            + "|top"
            + "|(?:ref-)?width"
            + "|(?:ref-)?height"
            + "|right"
            + "|bottom"
            //add more dimension properties here as the needs for them in CSS grows
            ))
        {
            ValueInterpreters.Add(new NumberValueInterpreter(parser, new Regex(CSSConstants.NUM + "(px)")));
        }
    }
}
