using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BluEngine.Engine.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using BluEngine.ScreenManager.Screens;

namespace BluEngine.Engine
{
    public class BaseEngine : GameScreen
    {
        #region Fields

        protected List<GameObject> gameObjects = new List<GameObject>();


        protected GameObject viewScreen;

        #endregion

        #region Initialize

        public override void LoadContent()
        {
            base.LoadContent();

            viewScreen = new GameObject();

        }

        #endregion

        #region Update

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
            }
        }

        #endregion

        #region HandleInput

        public override void HandleInput(InputControl input)
        {
            base.HandleInput(input);
        }

        #endregion

        #region Draw

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(spriteBatch, viewScreen.Position);
            }
        }

        #endregion
    }
}
