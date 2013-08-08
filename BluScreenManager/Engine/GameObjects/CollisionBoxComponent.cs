using System;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;

namespace BluEngine.Engine.GameObjects
{
    public class CollisionBoxComponent : Transform
    {

        #region Fields

        public bool Active = true;

        public int Width = 1;
        public int Height = 1;

        public CollisionType CollisionType;

        [XmlIgnore()]
        protected bool dirtyMatrix = true;

        protected Vector2 origin = Vector2.Zero;

        [XmlIgnore()]
        protected Matrix positionMatrix = Matrix.CreateTranslation(new Vector3(0,0,0));
        [XmlIgnore()]
        protected Matrix rotationMatrix = Matrix.CreateRotationZ(0);
        [XmlIgnore()]
        protected Matrix scaleMatrix = Matrix.CreateScale(0);
        [XmlIgnore()]
        protected Matrix originMatrix = Matrix.CreateTranslation(new Vector3(0,0,0));

        [XmlIgnore()]
        protected Matrix translationMatrix;

        #endregion

        #region Properties

        public override Vector2 Position
        {
            get { return base.Position; }
            set
            {
                dirtyMatrix = true;
                base.Position = value;
                positionMatrix = Matrix.CreateTranslation(base.Position.X, base.Position.Y, 0);
            }
        }

        public override float Rotation
        {
            get
            {
                return base.Rotation;
            }
            set
            {
                dirtyMatrix = true;
                base.Rotation = MathHelper.WrapAngle(value);
                rotationMatrix = Matrix.CreateRotationZ(base.Rotation);
            }
        }

        public override float Scale
        {
            get
            {
                return base.Scale;
            }
            set
            {
                dirtyMatrix = true;
                base.Scale = value;
                scaleMatrix = Matrix.CreateScale(base.Scale);
            }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set
            {
                dirtyMatrix = true;
                origin = value;
                originMatrix = Matrix.CreateTranslation(origin.X * -1, origin.Y * -1, 0);
            }
        }

        /// <summary>
        /// Retrieves the radius of the object by getting half of the distance from corner to corner diagonally
        /// </summary>
        [XmlIgnore()]
        public float CollisionRadius
        {
            get { return (float)Math.Sqrt((Width * Width) + (Height * Height)) / 2; }
        }

        /// <summary>
        /// Retrieves the radius of the object by getting half of the distance from corner to corner diagonally but with 
        /// the exception of not square rooting the end result.
        /// </summary>
        [XmlIgnore()]
        public float RadiusNotSquared
        {
            get { return (float)((Width * Width) + (Height * Height)) / 2; }
        }

        /// <summary>
        /// Retrieves the dead center of the entity
        /// </summary>
        [XmlIgnore()]
        public Vector2 Center
        {
            get { return new Vector2(Width / 2, Height / 2); }
        }

        [XmlIgnore()]
        public Matrix Transform
        {
            get
            {
                if (dirtyMatrix)
                    recalculateTranslationMatrix();
                return translationMatrix;
            }
        }

        public Vector2[] BoundingCorners
        {
            get
            {
                Vector2[] corners = new Vector2[4];
                corners[0] = Vector2.Transform(Vector2.Zero, Transform);
                corners[1] = Vector2.Transform(new Vector2(Width, 0), Transform);
                corners[2] = Vector2.Transform(new Vector2(0, Height), Transform);
                corners[3] = Vector2.Transform(new Vector2(Width, Height), Transform);
                return corners;
            }
        }

        public Vector2 GetCenter
        {
            get
            {
                return Vector2.Transform(new Vector2(Width / 2, Height / 2), Transform);
            }
        }

        #endregion

        public override void Initialize(Microsoft.Xna.Framework.Content.ContentManager content, string path)
        {
            if (CollisionType != null)
            {
                CollisionSimulator.CollisionLists[CollisionType.ID].Add(this);
            }
        }

        #region Methods

        protected void recalculateTranslationMatrix()
        {
            if (origin == Vector2.Zero)
                translationMatrix = rotationMatrix * scaleMatrix * positionMatrix;
            else
                translationMatrix = originMatrix * rotationMatrix * scaleMatrix * positionMatrix;
            dirtyMatrix = false;
        }

        public bool PointIsInBoundingBox(Vector2 point)
        {
            Matrix screenToTexture = Matrix.Invert(Transform);
            Vector2 p = Vector2.Transform(point, screenToTexture);
            return (p.X >= 0 && p.X <= Width && p.Y >= 0 && p.Y <= Height);
        }

        public Vector2 PositionFromTransform(Vector2 offset)
        {
            return Vector2.Transform(offset, Transform);
        }

        /// <summary>
        /// Clone object
        /// </summary>
        public object Clone()
        {
            CollisionBoxComponent clone = new CollisionBoxComponent();

            clone.connectedGameObject = connectedGameObject;
            clone.Position = Position;
            clone.Rotation = Rotation;
            clone.Scale = Scale;
            clone.Origin = origin;
            clone.Width = Width;
            clone.Height = Height;

            return clone;
        }

        /// <summary>
        /// Check to see whether the object intersects another
        /// </summary>
        public bool Intersects(CollisionBoxComponent value)
        {
            Vector2[] itsCorners = value.BoundingCorners;
            Vector2[] myCorners = BoundingCorners;
            for (int i = 0; i < 4; i++)
            {
                if (PointIsInBoundingBox(itsCorners[i]))
                    return true;
                if (value.PointIsInBoundingBox(myCorners[i]))
                    return true;
            }
            return false;
        }

        #endregion

        #region Static

        /// <summary>
        /// Retrives are drawable rectangle from the bounds of the collision box
        /// </summary>
        public static Rectangle CollisionEntityToRect(CollisionBoxComponent entity)
        {
            Rectangle rect = new Rectangle();
            rect.X = (int)entity.Position.X;
            rect.Y = (int)entity.Position.Y;
            rect.Width = (int)entity.Width;
            rect.Height = (int)entity.Height;

            return rect;
        }

        /// <summary>
        /// Retrives are drawable rectangle from the bounds of the collision box with scaling applied
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Rectangle</returns>
        public static Rectangle CollisionEntityToRectScaled(CollisionBoxComponent entity)
        {
            Rectangle rect = new Rectangle();
            rect.X = (int)entity.Position.X;
            rect.Y = (int)entity.Position.Y;
            rect.Width = (int)(entity.Width * entity.Scale);
            rect.Height = (int)(entity.Height * entity.Scale);

            return rect;
        }

        #endregion


    }
}
