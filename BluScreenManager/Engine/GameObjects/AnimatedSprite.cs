using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using BluHelper;

namespace BluEngine.Engine.GameObjects
{

    public class AnimatedSprite : Sprite
    {
        #region Fields

        protected int frameWidth = 0;
        protected int frameHeight = 0;

        protected int currentFrame = 0;

        protected bool playing = true;

        protected long animationSpeed = InstanceTime.Zero;
        protected long lastUpdate = InstanceTime.Zero;

        public int Repeat = 1;
        protected int timesPlayed = 0;

        public event EventHandler AnimationFinished;

        #endregion

        #region Properties

        public int FrameWidth
        {
            get { return frameWidth; }
            set { frameWidth = value; }
        }

        public int FrameHeight
        {
            get { return frameHeight; }
            set { frameHeight = value; }
        }

        public override int Width
        {
            get
            {
                return frameWidth;
            }
        }

        public override int Height
        {
            get
            {
                return frameHeight;
            }
        }

        public long AnimationSpeed
        {
            get { return animationSpeed; }
            set { animationSpeed = value; }
        }

        #endregion

        #region Update

        public override void Update(InstanceTime gameTime)
        {
            if (gameTime.TotalTime - lastUpdate > animationSpeed && playing == true)
            {
                currentFrame++;


                if (currentFrame >= sourceImage.Width / frameWidth)
                {
                    timesPlayed++;
                    currentFrame = 0;
                    
                    if (timesPlayed >= Repeat && Repeat != 0)
                    {
                        
                        if (AnimationFinished != null)
                            AnimationFinished(this, new EventArgs());
                        playing = false;
                    }
                }

                lastUpdate = gameTime.TotalTime;
            }
        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch, Vector2 screenOffset)
        {
            spriteBatch.Draw(sourceImage, ConnectedGameObject.Position - screenOffset,
                new Rectangle(frameWidth * currentFrame, 0, frameWidth, frameHeight),
                Color.White, ConnectedGameObject.Rotation, ImageOffset,
                ConnectedGameObject.Scale, SpriteEffects.None, layer);
        }


        #endregion

        #region Methods

        public void Play()
        {
            playing = true;
            currentFrame = 0;
            timesPlayed = 0;
        }

        #endregion
    }
}
