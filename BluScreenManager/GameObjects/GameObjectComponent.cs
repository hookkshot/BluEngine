using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.GameObjects
{
    public class GameObjectComponent
    {
        #region Fields


        #endregion

        #region Initialize

        #endregion

        #region Update

        public virtual void Update(GameTime gameTime)
        {
        }

        #endregion

        #region Draw

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 screenOffset)
        {
        }

        #endregion

        #region Methods

        #endregion
    }
}
