﻿using System.Collections.Generic;
using BluEngine.Engine.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private List<GameObject> gameObjects = new List<GameObject>();

        /// <summary>
        /// Represents the "camera" or "viewport" of the game world render layer.
        /// </summary>
        protected GameObject ViewScreen
        {
            get { return viewScreen; }
        }
        private GameObject viewScreen;

        public override void LoadContent()
        {
            base.LoadContent();
            viewScreen = new GameObject();
        }

        protected override void UpdateWorld(GameTime gameTime)
        {
            foreach (GameObject gameObject in gameObjects)
                gameObject.Update(gameTime);
        }
        
        protected override void DrawWorld(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (GameObject gameObject in gameObjects)
                gameObject.Draw(spriteBatch, viewScreen.Position);
        }


    }
}
