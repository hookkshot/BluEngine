
namespace BluEngine.Engine.GameObjects
{
    public class CollisionGameObject : GameObject
    {
        protected bool active = true;

        public CollisionBoxComponent TransformBox
        {
            get { return Transform as CollisionBoxComponent; }
        }

        public CollisionGameObject()
            : base()
        {
            Transform = new CollisionBoxComponent();
        }
    }
}
