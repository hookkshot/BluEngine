using Marzersoft.CSS;

namespace BluEngine.ScreenManager.Styles.CSS
{
    public class PercentageInterpreter : CSSDimensionInterpreter
    {
        public PercentageInterpreter() : base(
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
            ,
            CSSConstants.NUM + "([%])") { }
    }
}
