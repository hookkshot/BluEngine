﻿using System;
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
        public String ContentFolder = "Content";
        public String FontsFolder = "Fonts";
        public String StylesFolder = "Styles";
        public String TexturesFolder = "Textures";
        public String SoundsFolder = "Sounds";
        public String MusicFolder = "Music";
        public String ShadersFolder = "Shaders";
        public String DefaultFont = "smallfont";

        public Color GraphicsClearColor = Color.CornflowerBlue;
        public int ScreenWidth = 1280;
        public int ScreenHeight = 720;
        
        public Type FirstScreenType = null;
        public bool IsMouseVisible = true;
        public bool IsFullscreen = false;
        
        public int TweenAttributeLimit = 5;
        public Dictionary<Type, TweenAccessor> TweenAccessors = new Dictionary<Type, TweenAccessor>();
        
        public BluGameInitSettings(Type firstScreenType)
        {
            FirstScreenType = firstScreenType;
            TweenAccessors.Add(typeof(Widget), new WidgetAccessor());
        }
        public BluGameInitSettings() : this(null) { }
    }
}
