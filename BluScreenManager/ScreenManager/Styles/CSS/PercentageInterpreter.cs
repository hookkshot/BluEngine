using Marzersoft.CSS;
using System.Text.RegularExpressions;
using Marzersoft.CSS.Interpreters.Numbers;

namespace BluEngine.ScreenManager.Styles.CSS
{
    public class PercentageInterpreter : DimensionNameInterpreter
    {
        public PercentageInterpreter(CSSParser parser)
            : base(parser, new Regex(
                "alpha"
                + "|tint-strength"
                + "|layer-[0-" + (StyleSheet.STYLE_LAYERS - 1) + "]-alpha"
                + "|left"
                + "|top"
                + "|width"
                + "|height"
                + "|right"
                + "|bottom"
                //add more percentage properties here as the needs for them in CSS grows
                ))
        {
            ValueInterpreters.Add(new NumberValueInterpreter(parser, new Regex(CSSConstants.NUM + "([%])")));
        }
    }
}
