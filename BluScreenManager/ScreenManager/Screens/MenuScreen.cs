#region File Description
//File: MenuScreen.cs
//Date: 14/08/2012
//Version: 1.0
//
//Copyright (C) Blurift Entertainment
#endregion

using System.Collections.Generic;
using System.Linq;
using BluEngine.ScreenManager.MenuItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.ScreenManager.Screens
{
    public class MenuScreen : GameScreen
    {
        #region Fields

        List<SpriteFont> fonts = new List<SpriteFont>();
        protected List<MenuItem> menuItems = new List<MenuItem>();

        protected MenuItem selectedMenuItem;

        #endregion

        #region Initialization

        public MenuScreen()
        {
        }

        #endregion



        #region Update

        /// <summary>
        /// Allows the screen to run logic, such as updating the transition position.
        /// Unlike HandleInput, this method is called regardless of whether the screen
        /// is active, hidden, or in the middle of a transition.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            foreach (MenuItem item in menuItems)
            {
                item.Update(gameTime);
            }
        }


        /// <summary>
        /// Allows the screen to handle user input. Unlike Update, this method
        /// is only called when the screen is active, and not when some other
        /// screen has taken the focus.
        /// </summary>
        public override void HandleInput(InputControl input)
        {
            for (int i = 0; i < menuItems.Count(); i++)
                menuItems[i].HandleInput(input, selectedMenuItem == null || selectedMenuItem == menuItems[i]);
        }

        #endregion

        #region Draw

        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {


            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            for (int i = 0; i < menuItems.Count(); i++)
            {
                menuItems[i].Draw(spriteBatch);
            }
            spriteBatch.End();
        }

        #endregion

        #region Methods

        protected void SelectItem()
        {
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (menuItems[i].IsItemInUse)
                {
                    selectedMenuItem = menuItems[i];
                    break;
                }
                else
                {
                    selectedMenuItem = null;
                }
            }
        }

        #endregion

    }
}
