using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
