using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BluEngine.TweenAccessors;
using BluEngine.ScreenManager.Widgets;
using AurelienRibon.TweenEngine;

namespace BluEngine.Engine
{
    public class BluGameInitSettings
    {
        public int ScreenWidth = 1280;
        public int ScreenHeight = 720;
        public String ContentRoot = "Content";
        public Type FirstScreenType = null;
        public bool IsMouseVisible = true;
        public bool IsFullscreen = false;
        public bool IsBorderless = false;
        public String DefaultFont = "Fonts\\smallfont";
        public int TweenAttributeLimit = 5;
        public Dictionary<Type, TweenAccessor> TweenAccessors = new Dictionary<Type, TweenAccessor>();
        public Color GraphicsClearColor = Color.CornflowerBlue;


        public BluGameInitSettings(Type firstScreenType)
        {
            FirstScreenType = firstScreenType;
            TweenAccessors.Add(typeof(Widget), new WidgetAccessor());
        }
        public BluGameInitSettings() : this(null) { }

    }
}
