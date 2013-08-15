using Marzersoft.CSS;

namespace BluEngine.ScreenManager.Styles.CSS
{
    public class DimensionInterpreter : CSSDimensionInterpreter
    {
        public DimensionInterpreter()
            : base(
            "left"
            + "|top"
            + "|(?:ref-)?width"
            + "|(?:ref-)?height"
            + "|right"
            + "|bottom"
            //add more dimension properties here as the needs for them in CSS grows
            ,
            CSSConstants.NUM + "(px)") { }
    }
}
