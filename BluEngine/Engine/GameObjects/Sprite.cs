using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.Engine.GameObjects
{
    public class Sprite : GameObjectComponent
    {
        #region Fields

        [XmlIgnore()]
        protected Texture2D sourceImage;
        protected string sourceImageName = "";

        protected float layer = 0.5f;

        private Vector2 imageOffset = Vector2.Zero;

        #endregion

        #region Properties

        public Texture2D SourceImage
        {
            get { return sourceImage; }
            set { sourceImage = value; }
        }

        public string SourceImageName
        {
            get { return sourceImageName; }
            set { sourceImageName = value; }
        }

        public float Layer
        {
            get { return layer; }
            set { layer = value; }
        }

        public virtual int Width
        {
            get { return sourceImage.Width; }
        }

        public virtual int Height
        {
            get { return sourceImage.Height; }
        }

        public Vector2 ImageOffset
        {
            get { return imageOffset; }
            set { imageOffset = value; }
        }

        #endregion

        #region Initialize

        public override void Initialize(ContentManager content, string path)
        {
            if (content != null)
            {
                if (path == null)
                {
                    sourceImage = content.Load<Texture2D>(sourceImageName);
                }
                else
                {
                    sourceImage = content.Load<Texture2D>(Path.Combine(path, sourceImageName));
                }
            }
        }

        #endregion

        #region Update

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch, Vector2 screenOffset)
        {
            spriteBatch.Draw(sourceImage, ConnectedGameObject.Position - screenOffset, sourceImage.Bounds, Color.White, ConnectedGameObject.Rotation, imageOffset, ConnectedGameObject.Scale, SpriteEffects.None, layer);
        }

        #endregion

        #region Methods

        public virtual void MakeCenter()
        {
            imageOffset = new Vector2(Width / 2, Height / 2);
        }

        #endregion
    }
}
