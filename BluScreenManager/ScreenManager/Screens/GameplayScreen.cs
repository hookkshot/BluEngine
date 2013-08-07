using System.Collections.Generic;
using BluEngine.Engine.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.ScreenManager.Screens
{
    public class GameplayScreen : WidgetScreen
    {
        private List<GameObject> gameObjects = new List<GameObject>();
        private GameObject viewScreen;

        protected override void DrawWorld(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(spriteBatch, viewScreen.Position);
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();
            viewScreen = new GameObject();
        }

        /*
        //functions you can directly
        //override in subclasses of this class:
        protected override void MouseMove(Point mousePos, Point prevPos) {}
        protected override void MouseDown(Point mousePos, int button) { }
        protected override void MouseUp(Point mousePos, int button) { }
        protected override void KeyDown(Keys key) { }
        protected override void KeyUp(Keys key) { }
        protected override void UpdateWorld(GameTime gameTime) { }
         
        //you can also override these, but ensure you call the base
        //versions otherwise the UI will be invisible or dead
        protected override void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.DrawUI(gameTime,spriteBatch);
        }

        protected override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime,spriteBatch);
        }
         */
    }
}
