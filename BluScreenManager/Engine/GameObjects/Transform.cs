
using Microsoft.Xna.Framework;

namespace BluEngine.Engine.GameObjects
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
            protected set { position = value; }
        }

        public virtual float Rotation
        {
            get { return rotation; }
            protected set { rotation = value; }
        }

        public virtual float Scale
        {
            get { return scale; }
            protected set { scale = value; }
        }

        #endregion

        #region Methods

        public void SetTransform(Vector2 Position, float Rotation, float Scale)
        {
            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;
        }

        #endregion
    }
}
