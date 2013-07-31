using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;

namespace BluEngine.GameObjects
{
    public class CollisionBox : Transform
    {

        #region Fields

        public bool Active = true;

        public int Width = 1;
        public int Height = 1;

        [XmlIgnore()]
        protected bool dirtyMatrix = true;

        protected Vector2 origin = Vector2.Zero;

        [XmlIgnore()]
        protected Matrix positionMatrix;
        [XmlIgnore()]
        protected Matrix rotationMatrix;
        [XmlIgnore()]
        protected Matrix scaleMatrix;
        [XmlIgnore()]
        protected Matrix originMatrix;

        [XmlIgnore()]
        protected Matrix translationMatrix;

        #endregion

        #region Properties

        public override Vector2 Position
        {
            get { return position; }
            set
            {
                dirtyMatrix = true;
                position = value;
                positionMatrix = Matrix.CreateTranslation(position.X, position.Y, 0);
            }
        }

        public override float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                dirtyMatrix = true;
                rotation = MathHelper.WrapAngle(value);
                rotationMatrix = Matrix.CreateRotationZ(rotation);
            }
        }

        #endregion
    }
}
