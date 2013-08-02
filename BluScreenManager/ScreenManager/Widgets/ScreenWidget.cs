using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BluEngine.ScreenManager.Widgets
{
    /// <summary>
    /// A special type of widget that does not allow itself to be re-parented.
    /// </summary>
    public class ScreenWidget : Widget
    {
        /// <summary>
        /// The widget's parent (container) widget.
        /// </summary>
        new public Widget Parent
        {
            get { return null; }
            set { return; }
        }
    }
}
