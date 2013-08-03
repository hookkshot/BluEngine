using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AurelienRibon.TweenEngine;
using BluEngine.ScreenManager.Widgets;

namespace BluEngine.TweenAccessors
{
    public class WidgetAccessor : TweenAccessor
    {
        public const int TWEEN_HORIZONTAL = 1;
        public const int TWEEN_VERTICAL = 2;
        public const int TWEEN_HORIZ_VERT = 3;

        public override int GetValues(object target, int tweenType, float[] returnValues)
        {
            Widget widget = target as Widget;
            switch (tweenType)
            {
                case TWEEN_HORIZONTAL:
                    returnValues[0] = widget.Left;
                    return 1;

                case TWEEN_VERTICAL:
                    returnValues[0] = widget.Top;
                    return 1;

                case TWEEN_HORIZ_VERT:
                    returnValues[0] = widget.Left;
                    returnValues[1] = widget.Top;
                    return 2;
            }
            return 0;
        }

        public override void SetValues(object target, int tweenType, float[] newValues)
        {
            Widget widget = target as Widget;
            switch (tweenType)
            {
                case TWEEN_HORIZONTAL:
                    widget.Left = newValues[0];
                    break;

                case TWEEN_VERTICAL:
                    widget.Top = newValues[0];
                    break;

                case TWEEN_HORIZ_VERT:
                    widget.Left = newValues[0];
                    widget.Top = newValues[1];
                    break;

            }
        }
    }
}
