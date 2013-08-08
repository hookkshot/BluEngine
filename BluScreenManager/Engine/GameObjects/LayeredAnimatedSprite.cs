using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - lastUpdate > animationSpeed && playing == true && sourceImages.Count > 0)
            {
                currentFrame++;


                if (currentFrame >= sourceImages[0].Width / frameWidth)
                {
                    timesPlayed++;
                    currentFrame = 0;

                    if (timesPlayed >= Repeat && Repeat != 0)
                    {

                        //if (base.AnimationFinished != null)
                            //base.AnimationFinished(this, new EventArgs());
                        playing = false;
                    }
                }

                lastUpdate = gameTime.TotalGameTime;
            }
        }

        public override void Draw(SpriteBatch spriteBatch,Vector2 screenOffset)
        {
            foreach (Sprite sprite in sourceImages)
            {
                spriteBatch.Draw(sprite.SourceImage, ConnectedGameObject.Position - screenOffset, new Rectangle(frameWidth * currentFrame, 0, frameWidth, frameHeight), Color.White, ConnectedGameObject.Rotation, new Vector2(Width/2,Height/2), ConnectedGameObject.Scale, SpriteEffects.None, sprite.Layer);
            }
        }
    }
}
