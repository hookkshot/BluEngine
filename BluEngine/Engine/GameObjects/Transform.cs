
using Microsoft.Xna.Framework;

namespace BluEngine.Engine.GameObjects
{
    public class Transform : GameObjectComponent
    {
        #region Fields

        private Vector2 position = Vector2.Zero;
        private float rotation = 0.0f;
        private float scale = 1.0f;

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
