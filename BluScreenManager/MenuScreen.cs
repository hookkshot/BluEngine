#region File Description
//File: MenuScreen.cs
//Date: 14/08/2012
//Version: 1.0
//
//Copyright (C) Blurift Entertainment
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BluEngine
{
    public class MenuScreen : GameScreen
    {
        #region Fields

        List<SpriteFont> fonts = new List<SpriteFont>();
        protected List<MenuItem> menuItems = new List<MenuItem>();

        protected MenuItem selectedMenuItem;

        protected ContentManager content;

        #endregion

        #region Initialization

        public MenuScreen()
        {
        }

        public override void LoadContent()
        {
            content = new ContentManager(ScreenManager.Game.Services);
            content.RootDirectory = "Content";
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
            {
                if (selectedMenuItem != null)
                {
                    if (selectedMenuItem == menuItems[i])
                    {
                        menuItems[i].HandleInput(input, true);
                    }
                    else
                    {
                        menuItems[i].HandleInput(input, false);
                    }
                }
                else
                {
                    menuItems[i].HandleInput(input, true);
                }
               
            }

            
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
