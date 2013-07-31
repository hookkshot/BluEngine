using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace BluEngine.GameObjects
{
    public class Transform : GameObjectComponent
    {
        #region Fields

        protected Vector2 position = Vector2.Zero;
        protected float rotation = 0.0f;
        protected float scale = 1.0f;

        #endregion

        #region Properties

        public virtual Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public virtual float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public virtual float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        #endregion
    }
}
