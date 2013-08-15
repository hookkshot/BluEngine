using Marzersoft.CSS;

namespace BluEngine.ScreenManager.Styles.CSS
{
    public class ColorInterpreter : CSSColorInterpreter
    {
        public ColorInterpreter()
            : base(
            "tint"
            + "|border-color"
            //add more colour properties here as the needs for them in CSS grows
            ) { }
    }
}
