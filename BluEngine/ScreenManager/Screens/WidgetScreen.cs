﻿using System;
using System.Collections.Generic;
using BluEngine.ScreenManager.Styles;
using BluEngine.ScreenManager.Styles.CSS;
using BluEngine.ScreenManager.Widgets;
using Marzersoft.CSS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Marzersoft.CSS.Interpreters;
using System.IO;
using Marzersoft.CSS.Rulesets;

namespace BluEngine.ScreenManager.Screens
{
    /// <summary>
    /// A generalized screen that maintains a layer of UI "widgets" (resolution-independant controls) over the top of a user-definable "world" layer (can be nothing).
    /// </summary>
    public class WidgetScreen : GameScreen
    {
        private ScreenWidget baseWidget = null;
        private Widget[] mouseDownWidgets = new Widget[]{null,null,null,null,null};
        private Widget mouseHoverWidget = null;
        private Widget selectedWidget = null;
        private Dictionary<String, List<Widget>> cssStyleLinks;
        private Keys[] watchedKeys = new Keys[] {
            Keys.Back, Keys.Tab, Keys.Enter,
            Keys.Pause, Keys.CapsLock, Keys.Escape,
            Keys.Space, Keys.PageUp, Keys.PageDown,
            Keys.End, Keys.Home, Keys.Left,
            Keys.Up, Keys.Right, Keys.Down,
            Keys.Select, Keys.Insert, Keys.Delete,
            Keys.Help, Keys.D0, Keys.D1,
            Keys.D2, Keys.D3, Keys.D4,
            Keys.D5, Keys.D6, Keys.D7,
            Keys.D8, Keys.D9, Keys.A,
            Keys.B, Keys.C, Keys.D,
            Keys.E, Keys.F, Keys.G,
            Keys.H, Keys.I, Keys.J, Keys.K,
            Keys.L, Keys.M, Keys.N, Keys.O,
            Keys.P, Keys.Q, Keys.R, Keys.S,
            Keys.T, Keys.U, Keys.V, Keys.W,
            Keys.X, Keys.Y, Keys.Z, Keys.LeftWindows,
            Keys.RightWindows, Keys.NumPad0, Keys.NumPad1, Keys.NumPad2,
            Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6,
            Keys.NumPad7, Keys.NumPad8, Keys.NumPad9, Keys.Multiply,
            Keys.Add, Keys.Separator, Keys.Subtract, Keys.Decimal, Keys.Divide,
            Keys.F1, Keys.F2, Keys.F3, Keys.F4,
            Keys.F5, Keys.F6, Keys.F7, Keys.F8,
            Keys.F9, Keys.F10, Keys.F11, Keys.F12, Keys.F13,
            Keys.F14, Keys.F15, Keys.F16, Keys.F17, Keys.F18, Keys.F19,
            Keys.F20, Keys.F21, Keys.F22, Keys.F23, Keys.F24, Keys.NumLock,
            Keys.Scroll, Keys.LeftShift, Keys.RightShift, Keys.LeftControl,
            Keys.RightControl, Keys.LeftAlt, Keys.RightAlt
        };

        /// <summary>
        /// The base widget object. Add your child widgets to this via their constructors or by setting their Parent property.
        /// </summary>
        public ScreenWidget Base
        {
            get { return baseWidget; }
        }

        /// <summary>
        /// The static CSSParser object shared by all WidgetScreens.
        /// </summary>
        public BluCSSParser CSSParser
        {
            get
            {
                if (cssParser == null)
                    cssParser = new BluCSSParser();
                return cssParser;
            }
        }
        private static BluCSSParser cssParser = null;

        /// <summary>
        /// All cascading Widget styles used by this screen.
        /// </summary>
        public StyleSheet Styles
        {
            get { return styles; }
        }
        private StyleSheet styles;

        /// <summary>
        /// The path to the file this screen will load as it's stylesheet.
        /// By default, it will simply return the name of the runtime Type, appended with ".css",
        /// but you may override this if necessary.
        /// </summary>
        public virtual String StyleSheetPath
        {
            get { return GetType().Name + ".css"; }
        }

        public WidgetScreen()
            : base()
        {
            
            styles = new StyleSheet(this);
            cssStyleLinks = new Dictionary<String, List<Widget>>();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            //create widgets
            baseWidget = new ScreenWidget(this);
            LoadWidgets();

            //load CSS
            LoadCSS(StyleSheetPath);
        }

        /// <summary>
        /// Loads a CSS document and applies the rules found within. This is automatically called during this screen's
        /// loading routines to load the CSS file matching the type name, but you may use it to also apply other stylesheets
        /// to this screen.
        /// </summary>
        /// <param name="file">The filename of the css file found in BluGame.StylesPath.</param>
        protected void LoadCSS(string file)
        {
            String fullPath = ScreenManager.Game.ContentRoot + "\\" + ScreenManager.Game.StylesPath + file;
            if (!File.Exists(fullPath))
            {
                Console.WriteLine("Styles warning: CSS file \"" + fullPath + "\" does not exist.");
                return;
            }
            
            //load CSS document
            CSSParser.ActiveScreen = this;
            CSSDocument document = CSSParser.ParseFile(fullPath);

            //loop through CSS document
            foreach (KeyValuePair<String, Ruleset> ruleset in document.Rulesets)
            {
                foreach (KeyValuePair<String, State> state in ruleset.Value.States)
                {
                    string stateKey = state.Key.Equals("") ? "normal" : state.Key;
                    if (ruleset.Value.Prefix.Equals("#")) //it's an implicit Type style, pass it to the stylesheet
                    {
                        String typeName = ruleset.Value.Name + ", " + (ruleset.Value.Suffix.Equals("") ? "BluEngine" : ruleset.Value.Suffix);
                        try
                        {
                            Type type = Type.GetType(typeName,true,true);
                            if (type != null)
                                Styles.ApplyCSSStylesToType(type, stateKey, state.Value);
                            else
                                Console.WriteLine("Reflection error: Cannot create Type from string \"" + typeName + "\"");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Reflection error: " + e.GetType().Name + " thrown while creating Type from string \"" + typeName + "\"");
                        }
                    }
                    else if (ruleset.Value.Prefix.Equals(".")) //it's an explicit instance style
                    {
                        List<Widget> explicitWidgets = null;
                        if (cssStyleLinks.TryGetValue(ruleset.Value.Name, out explicitWidgets))
                        {
                            foreach (Widget widget in explicitWidgets)
                                Styles.ApplyCSSStylesToWidget(widget, stateKey, state.Value);
                        }
                    }
                }
            }
            //cleanup
            CSSParser.ActiveScreen = null;
        }

        /// <summary>
        /// Override this to load widgets before the screen loads it's CSS. You can still use LoadContent() as normal, but using this method specifically allows you to use RegisterCSSClass() on widgets that have already been loaded to use CSS styling on them.
        /// </summary>
        protected virtual void LoadWidgets()
        {
            
        }

        /// <summary>
        /// Registers a widget for use with a CSS class-based ruleset (eg .SomeClass).
        /// </summary>
        /// <param name="cssClassName">The name of the class (without the preceding period character)</param>
        /// <param name="widget">The widget object to which the class style will be applied.</param>
        protected void RegisterCSSClass(String cssClassName, Widget widget)
        {
            if (cssClassName == null || cssClassName.Length == 0 || widget == null)
                return;
            
            List<Widget> explicitWidgets = null;
            if (!cssStyleLinks.TryGetValue(cssClassName, out explicitWidgets))
                cssStyleLinks[cssClassName] = (explicitWidgets = new List<Widget>());

            if (!explicitWidgets.Contains(widget))
                explicitWidgets.Add(widget);
        }

        /// <summary>
        /// Registers a list of widgets for use with a CSS class-based ruleset (eg .SomeClass).
        /// </summary>
        /// <param name="cssClassName">The name of the class (without the preceding period character)</param>
        /// <param name="widgets">The list of widget objects to which the class style will be applied.</param>
        protected void RegisterCSSClass(String cssClassName, List<Widget> widgets)
        {
            if (cssClassName == null || cssClassName.Length == 0 || widgets == null || widgets.Count == 0)
                return;

            foreach (Widget widget in widgets)
                RegisterCSSClass(cssClassName,widget);
        }

        /// <summary>
        /// If a widget has been registered as a CSS class target, unregister it.
        /// </summary>
        /// <param name="widget">The widget to unregister.</param>
        public void UnregisterCSSClass(Widget widget)
        {
            if (widget == null || widget.Screen != this)
                return;

            foreach (KeyValuePair<String, List<Widget>> kvp in cssStyleLinks)
            {
                while (kvp.Value.Contains(widget))
                    kvp.Value.Remove(widget);
            }
        }

        /// <summary>
        /// Update any "world" layer information, then all widgets. You should not need to override this in children; use UpdateWorld and UpdateUI for specialization.
        /// </summary>
        /// <param name="gameTime">The gametime object passed in from Update().</param>
        /// <param name="otherScreenHasFocus"></param>
        /// <param name="coveredByOtherScreen"></param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            UpdateWorld(gameTime);
            baseWidget.UpdateAll(gameTime);
            UpdateUI(gameTime);
        }

        /// <summary>
        /// If this WidgetScreen has a "world" layer to it, perform it's updates here.
        /// </summary>
        /// <param name="gameTime">The gametime object passed in from Update().</param>
        protected virtual void UpdateWorld(GameTime gameTime) { }

        /// <summary>
        /// Update the UI layer of this WidgetScreen. This does not include widgets; this function is for doing your own custom UI-layer updating SEPARATE to the widget layer.
        /// </summary>
        /// <param name="gameTime">The gametime object passed in from Update().</param>
        protected virtual void UpdateUI(GameTime gameTime) { }

        /// <summary>
        /// Handles the input provided by the ScreenManager's InputControl. You should not need to override this - use the specialized input methods instead!
        /// </summary>
        /// <param name="input">The InputControl object passed in from the ScreenManager.</param>
        public override void HandleInput(InputControl input)
        {
            base.HandleInput(input);

            //store mouse pos
            Point mousePos = input.CurrentMouse;

            //check for a  widget at mouse point
            Widget widgetAtPoint = baseWidget.ChildAtPoint(mousePos, HitFlags.Mouse);
            
            //handle mouse movement
            bool cascade = true;
            if (input.MouseMoved())
            {
                //check for mouse enter/leave events
                if (widgetAtPoint != mouseHoverWidget)
                {
                    if (mouseHoverWidget != null)
                        cascade = !mouseHoverWidget.MouseLeave(mousePos);

                    mouseHoverWidget = widgetAtPoint;

                    if (mouseHoverWidget != null)
                        cascade &= !mouseHoverWidget.MouseEnter(mousePos);
                }

                if (cascade)
                    MouseMove(mousePos,input.PreviousMouse);
            }

            //handle mouse clicks
            for (int i = 1; i <= 5; i++)
            {
                ButtonState current;
                if (input.MouseChanged(i,out current))
                {
                    cascade = true;
                    if (current == ButtonState.Pressed)
                    {
                        mouseDownWidgets[i-1] = widgetAtPoint;
                        if (widgetAtPoint != null)
                            cascade = !widgetAtPoint.MouseDown(mousePos, i);

                        if (cascade)
                        {
                            MouseDown(mousePos, i);
                            selectedWidget = null;
                        }
                        else
                        {
                            if(widgetAtPoint.CanSelect)
                                selectedWidget = widgetAtPoint;
                        }
                    }
                    else
                    {
                        if (mouseDownWidgets[i-1] != null)
                            cascade = !mouseDownWidgets[i - 1].MouseUp(mousePos, i);
                        mouseDownWidgets[i-1] = null;

                        if (cascade)
                            MouseUp(mousePos, i);
                    }
                }
            }

            //check keyboard events
            for (int i = 0; i < watchedKeys.Length; i++)
            {
                if (input.KeyReleased(watchedKeys[i]))
                {
                    cascade = true;
                    if (selectedWidget != null)
                        cascade = !selectedWidget.KeyUp(watchedKeys[i]);
                    if (cascade)
                        KeyUp(watchedKeys[i]);
                }

                if (input.KeyPressed(watchedKeys[i]))
                {
                    cascade = true;
                    if (selectedWidget != null)
                        cascade = !selectedWidget.KeyDown(watchedKeys[i]);
                    if (cascade)
                        KeyDown(watchedKeys[i]);
                }
            }
        }

        /// <summary>
        /// MouseMove events passed down to the "world" layer. This may not fire - the UI layer can cancel these events before they cascade down to the world layer.
        /// </summary>
        /// <param name="mousePos">The new mouse position.</param>
        /// <param name="prevPos">The old mouse position.</param>
        protected virtual void MouseMove(Point mousePos, Point prevPos) {}

        /// <summary>
        /// MouseDown events passed down to the "world" layer. This may not fire - the UI layer can cancel these events before they cascade down to the world layer.
        /// </summary>
        /// <param name="pt">The screen coords of the event.</param>
        /// <param name="button">The 1-indexed number of the mouse button according to the constants defined in InputControl.</param>
        protected virtual void MouseDown(Point mousePos, int button) { }

        /// <summary>
        /// MouseUp events passed down to the "world" layer. This may not fire - the UI layer can cancel these events before they cascade down to the world layer.
        /// </summary>
        /// <param name="pt">The screen coords of the event.</param>
        /// <param name="button">The 1-indexed number of the mouse button according to the constants defined in InputControl.</param>
        protected virtual void MouseUp(Point mousePos, int button) { }

        /// <summary>
        /// KeyDown events passed down to the "world" layer. This may not fire - the UI layer can cancel these events before they cascade down to the world layer.
        /// </summary>
        /// <param name="key">The key that was pressed.</param>
        protected virtual void KeyDown(Keys key)
        {
            switch (key)
            {
                case Keys.F5:
                    ScreenManager.DrawingDebug = !ScreenManager.DrawingDebug;
                    break;
            }
        }

        /// <summary>
        /// KeyDown events passed down to the "world" layer. This may not fire - the UI layer can cancel these events before they cascade down to the world layer.
        /// </summary>
        /// <param name="key">The key that was released.</param>
        protected virtual void KeyUp(Keys key) { }

        /// <summary>
        /// Draw any base layer then all widgets. You should not need to override this in children; use DrawWorld and DrawUI for specialization.
        /// </summary>
        /// <param name="gameTime">The gametime object passed in from Update().</param>
        /// <param name="otherScreenHasFocus"></param>
        /// <param name="coveredByOtherScreen"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            DrawWorld(gameTime, spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            baseWidget.DrawAll(spriteBatch);
            DrawUI(gameTime, spriteBatch);
            spriteBatch.End();
        }

        /// <summary>
        /// If this WidgetScreen has a "world" layer to it, perform it's drawing here.
        /// </summary>
        /// <param name="gameTime">The gametime object passed in from Draw().</param>
        /// <param name="spriteBatch">The SpriteBatch object passed in from the ScreenManager.</param>
        protected virtual void DrawWorld(GameTime gameTime, SpriteBatch spriteBatch) {}

        /// <summary>
        /// If this WidgetScreen has a "UI" layer to it, perform it's drawing here. This does not include widgets; this function is for doing your own custom rendering ON TOP of the widget layer.
        /// </summary>
        /// <param name="gameTime">The gametime object passed in from Draw().</param>
        /// <param name="spriteBatch">The SpriteBatch object passed in from the ScreenManager.</param>
        protected virtual void DrawUI(GameTime gameTime, SpriteBatch spriteBatch) { }

    }


}
