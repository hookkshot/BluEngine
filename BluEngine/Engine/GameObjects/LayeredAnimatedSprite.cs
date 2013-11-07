using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.Engine.GameObjects
{
    public class LayeredAnimatedSprite : AnimatedSprite
    {
        #region Fields

        private List<Sprite> sourceImages = new List<Sprite>();

        #endregion

        #region Properties

        public List<Sprite> SourceImages
        {
            get { return sourceImages; }
        }

        #endregion

        #region Initialize

        public override void Initialize(Microsoft.Xna.Framework.Content.ContentManager content, string path)
        {
            foreach (Sprite sprite in sourceImages)
            {
                sprite.Initialize(content, path);
            }
        }

        #endregion

        public override void Draw(SpriteBatch spriteBatch,Vector2 screenOffset)
        {
            foreach (Sprite sprite in sourceImages)
            {
                spriteBatch.Draw(sprite.SourceImage, ConnectedGameObject.Position - screenOffset, new Rectangle(frameWidth * currentFrameX, frameHeight * currentFrameY, frameWidth, frameHeight), Color.White, ConnectedGameObject.Rotation, ImageOffset, ConnectedGameObject.Scale, SpriteEffects.None, sprite.Layer);
            }
        }
    }
}
