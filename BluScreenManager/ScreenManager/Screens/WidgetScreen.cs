using BluEngine.Engine;
using BluEngine.ScreenManager.Styles;
using BluEngine.ScreenManager.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Text;
using Marzersoft.CSS;
using System.Collections.Generic;
using BluEngine.ScreenManager.Styles.CSS;


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
        public BluEngineCSSParser CSSParser
        {
            get
            {
                if (cssParser == null)
                {
                    List<CSSPropertyInterpreter> interpreters = new List<CSSPropertyInterpreter>();
                    interpreters.Add(new BorderLayerInterpreter());
                    interpreters.Add(new ImageLayerInterpreter());

                    CSSColorInterpreter colorInterpreter = new CSSColorInterpreter();
                    colorInterpreter.NameRegex = "tint|border-color";
                    interpreters.Add(colorInterpreter);

                    CSSDimensionInterpreter dimensionInterpreter = new CSSDimensionInterpreter();
                    dimensionInterpreter.NameRegex = "border-width|bottom|top|right|left|(?:ref-)?width|(?:ref-)?height|alpha|tint-strength|layer-[0-" + (StyleSheet.STYLE_LAYERS - 1) + "]-alpha";
                    interpreters.Add(dimensionInterpreter);

                    cssParser = new BluEngineCSSParser(interpreters);
                }
                return cssParser;
            }
        }
        private static BluEngineCSSParser cssParser = null;

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
            //load CSS document
            CSSParser.ActiveScreen = this;
            CSSParser.Parse(ScreenManager.Game.ContentRoot + "\\" + ScreenManager.Game.StylesPath + file, true);
            CSSParser.ActiveScreen = null;
            Dictionary<String, List<ICSSProperty>> rulesets = CSSParser.Rulesets;

            //loop through CSS document
            foreach (KeyValuePair<String, List<ICSSProperty>> kvp in rulesets)
            {
                String[] selector = kvp.Key.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                String name = selector[0];
                String state = selector.Length > 1 ? selector[1] : "normal";

                if (name[0] == '#') //it's an implicit Type style, pass it to the stylesheet
                {
                    selector = name.Substring(1).Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                    String typeName = selector[0].Trim() + ", " + (selector.Length > 1 ? selector[1].Trim() : "BluEngine");
                    try
                    {
                        Type type = Type.GetType(typeName);
                        if (type != null)
                            Styles.ApplyCSSStylesToType(type, state, kvp.Value);
                        else
                            Console.WriteLine("Reflection error: Cannot create Type from string \"" + typeName + "\"");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Reflection error: " + e.GetType().Name + " thrown while creating Type from string \"" + typeName + "\"");
                    }
                }
                else if (name[0] == '.') //it's an explicit instance style
                {
                    List<Widget> explicitWidgets = null;
                    if (cssStyleLinks.TryGetValue(name.Substring(1), out explicitWidgets))
                    {
                        foreach (Widget widget in explicitWidgets)
                            Styles.ApplyCSSStylesToWidget(widget, state, kvp.Value);
                    }
                }
            }
            
            CSSParser.Clear();
        }

        /// <summary>
        /// Override this to load widgets before the screen loads it's CSS. You can still use LoadContent() as normal, but using this method specifically allows you to use RegisterCSSClass() on widgets that have already been loaded to use CSS styling on them.
        /// </summary>
        protected virtual void LoadWidgets()
        {
            
        }

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
                            MouseDown(mousePos, i);
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

            spriteBatch.Begin();
            DrawWorld(gameTime, spriteBatch);
            spriteBatch.End();

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
