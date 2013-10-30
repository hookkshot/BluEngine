#region File Description
//File: MenuItem.cs
//Date: 11/08/2012
//Version: 1.0
//
//Copyright (C) Blurift Entertainment
#endregion

using Microsoft.Xna.Framework;

namespace BluEngine.ScreenManager.MenuItems
{
    public enum Alignment
    {
        Left,
        Center,
        Right,
    }

    public class MenuItem : Container
    {
        #region Fields

        //event delegates
        public delegate void MenuItemEvent(MenuItem sender);

        //Misc
        protected bool active;
        protected bool isItemInUse;

        #endregion

        #region Properties

        public bool Active
        {
            get { return this.active; }
            set { this.active = value; }
        }

        public bool IsItemInUse
        {
            get { return isItemInUse; }
        }

        #endregion

        #region Initialize

        public MenuItem(Vector2 position) : base(position)
        {
            active = true;
            isItemInUse = false;
        }
        public MenuItem() : this (Vector2.Zero) {}

        #endregion

        /// <summary>
        /// Runs the update logic of the menu item for each frame
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Handles any input needed by the menu item from the user and lets the menu item know if it is in focus. (only needs to be called if the screen is active)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="inFocus">Lets the item know whthere its in focus and can be used or not</param>
        public virtual void HandleInput(InputControl input, bool inFocus) { }
    }
}
