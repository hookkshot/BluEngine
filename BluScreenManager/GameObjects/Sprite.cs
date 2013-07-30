using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BluEngine.GameObjects
{
    public class Sprite : GameObjectComponent
    {
        #region Fields

        [XmlIgnore()]
        protected Texture2D sourceImage;
        protected string sourceImageName = "";

        protected float layer = 0.5f;

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

        #endregion

        #region Initialize

        public override void Initialize(ContentManager content, string path)
        {
            sourceImage = content.Load<Texture2D>(Path.Combine(path, sourceImageName));
        }

        #endregion

        #region Update

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch, Vector2 screenOffset)
        {
            spriteBatch.Draw(sourceImage, ConnectedGameObject.Position, sourceImage.Bounds, Color.White, 0.0f, new Vector2(sourceImage.Width/2,sourceImage.Height/2),1.0f, SpriteEffects.None, layer);
        }

        #endregion
    }
}
