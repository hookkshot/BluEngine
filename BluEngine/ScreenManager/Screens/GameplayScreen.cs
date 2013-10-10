using System.Collections.Generic;
using BluEngine.Engine.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using BluHelper;
using BluEngine.Engine;

namespace BluEngine.ScreenManager.Screens
{
    /// <summary>
    /// <para>A specialized WidgetScreen that maintains, updates and renders a list of Game World objects.</para>
    /// <para>Subclass this to implement your actual "gameplay" functionality. Overrides you may employ to achieve this are as follows:</para>
    /// <para>protected override void MouseMove(Point mousePos, Point prevPos)</para>
    /// <para>protected override void MouseDown(Point mousePos, int button)</para>
    /// <para>protected override void MouseUp(Point mousePos, int button)</para>
    /// <para>protected override void KeyDown(Keys key)</para>
    /// <para>protected override void KeyUp(Keys key)</para>
    /// <para>protected override void UpdateWorld(GameTime gameTime)</para>
    /// <para>protected override void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)</para>
    /// <para>protected override void UpdateUI(GameTime gameTime)</para>
    /// </summary>
    public class GameplayScreen : WidgetScreen
    {
        protected Engine.Engine Engine
        {
            get { return engine; }
            set { engine = value; }
        }
        private Engine.Engine engine;

        /// <summary>
        /// Represents the "camera" or "viewport" of the game world render layer.
        /// </summary>
        protected ViewScreen ViewScreen
        {
            get { return viewScreen; }
            set { if (value == null) return; viewScreen = value; }
        }
        private ViewScreen viewScreen;

        public override void LoadContent()
        {
            base.LoadContent();
            viewScreen = new ViewScreen(ScreenManager.GraphicsDevice.Viewport.Width,ScreenManager.GraphicsDevice.Viewport.Height);

            engine.Content = new Microsoft.Xna.Framework.Content.ContentManager(ScreenManager.Game.Services,"Content");
            engine.LoadContent();
        }

        protected override void UpdateWorld(GameTime gameTime)
        {
            engine.Update(gameTime);
        }
        
        protected override void DrawWorld(GameTime gameTime, SpriteBatch spriteBatch)
        {
            engine.Draw(gameTime, spriteBatch);
        }

        protected override void KeyDown(Keys key)
        {
            engine.KeyDown(key);
        }

        protected override void KeyUp(Keys key)
        {
            engine.KeyUp(key);
        }
    }
}
