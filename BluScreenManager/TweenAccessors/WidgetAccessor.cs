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
        public const int TWEEN_POSITION = 3;
        public const int TWEEN_WIDTH = 4;
        public const int TWEEN_HEIGHT = 5;
        public const int TWEEN_SIZE = 6;
        public const int TWEEN_TOP_SIZE = 7;
        public const int TWEEN_TINT_STRENGTH = 8;
        public const int TWEEN_TOP_SIZE_TINT_STRENGTH = 9;

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

                case TWEEN_POSITION:
                    returnValues[0] = widget.Left;
                    returnValues[1] = widget.Top;
                    return 2;

                case TWEEN_WIDTH:
                    returnValues[0] = widget.Width;
                    return 1;

                case TWEEN_HEIGHT:
                    returnValues[0] = widget.Height;
                    return 1;

                case TWEEN_SIZE:
                    returnValues[0] = widget.Width;
                    returnValues[1] = widget.Height;
                    return 2;

                case TWEEN_TOP_SIZE:
                    returnValues[0] = widget.Top;
                    returnValues[1] = widget.Width;
                    returnValues[2] = widget.Height;
                    return 3;

                case TWEEN_TINT_STRENGTH:
                    returnValues[0] = widget.TintStrength;
                    return 1;

                case TWEEN_TOP_SIZE_TINT_STRENGTH:
                    returnValues[0] = widget.Top;
                    returnValues[1] = widget.Width;
                    returnValues[2] = widget.Height;
                    returnValues[3] = widget.TintStrength;
                    return 4;
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

                case TWEEN_POSITION:
                    widget.Left = newValues[0];
                    widget.Top = newValues[1];
                    break;

                case TWEEN_WIDTH:
                    widget.Width = newValues[0];
                    break;

                case TWEEN_HEIGHT:
                    widget.Height = newValues[0];
                    break;

                case TWEEN_SIZE:
                    widget.Width = newValues[0];
                    widget.Height = newValues[1];
                    break;

                case TWEEN_TOP_SIZE:
                    widget.Top = newValues[0];
                    widget.Width = newValues[1];
                    widget.Height = newValues[2];
                    break;

                case TWEEN_TINT_STRENGTH:
                    widget.TintStrength = newValues[0];
                    break;

                case TWEEN_TOP_SIZE_TINT_STRENGTH:
                    widget.Top = newValues[0];
                    widget.Width = newValues[1];
                    widget.Height = newValues[2];
                    widget.TintStrength = newValues[3];
                    break;
            }
        }
    }
}
