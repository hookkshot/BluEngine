using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AurelienRibon.TweenEngine;
using BluEngine.ScreenManager.Widgets;
using Microsoft.Xna.Framework;

namespace BluEngine.TweenAccessors
{
    public class WidgetAccessor : TweenAccessor
    {
        /// <summary>
        /// Tween the widget's Left value.
        /// </summary>
        public const int TWEEN_HORIZONTAL = 1;

        /// <summary>
        /// Tween the widget's Top value.
        /// </summary>
        public const int TWEEN_VERTICAL = 2;

        /// <summary>
        /// Tween the widget's Left,Top values.
        /// </summary>
        public const int TWEEN_POSITION = 3;

        /// <summary>
        /// Tween the widget's Width value.
        /// </summary>
        public const int TWEEN_WIDTH = 4;

        /// <summary>
        /// Tween the widget's Height value.
        /// </summary>
        public const int TWEEN_HEIGHT = 5;

        /// <summary>
        /// Tween the widget's Width, Height values.
        /// </summary>
        public const int TWEEN_SIZE = 6;

        /// <summary>
        /// Tween the widget's Top, Width, Height values.
        /// </summary>
        public const int TWEEN_TOP_SIZE = 7;

        /// <summary>
        /// Tween the widget's TintStrength value.
        /// </summary>
        public const int TWEEN_TINT_STRENGTH = 8;

        /// <summary>
        /// Tween the widget's Top, Width, Height, TintStrength values.
        /// </summary>
        public const int TWEEN_TOP_SIZE_TINT_STRENGTH = 9;

        /// <summary>
        /// Tween the widget's Tint value (R, G, B as floats between 0.0f and 1.0f).
        /// </summary>
        public const int TWEEN_TINT = 10;

        /// <summary>
        /// Tween the widget's Alpha value.
        /// </summary>
        public const int TWEEN_ALPHA = 11;

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

                case TWEEN_TINT:
                    Color tint = widget.Tint;
                    returnValues[0] = (float)tint.R / 255.0f;
                    returnValues[1] = (float)tint.G / 255.0f;
                    returnValues[2] = (float)tint.B / 255.0f;
                    return 3;

                case TWEEN_ALPHA:
                    returnValues[0] = widget.Alpha;
                    return 1;
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

                case TWEEN_TINT:
                    widget.Tint = new Color(newValues[0], newValues[1], newValues[2]);
                    break;

                case TWEEN_ALPHA:
                    widget.Alpha = newValues[0];
                    break;
            }
        }
    }
}
