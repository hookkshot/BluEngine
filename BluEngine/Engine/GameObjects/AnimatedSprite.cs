using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.Engine.GameObjects
{

    public class AnimatedSprite : Sprite
    {
        #region Fields

        protected int frameWidth = 0;
        protected int frameHeight = 0;

        protected int currentFrameX = 0;
        protected int currentFrameY = 0;

        private int xframes = 0;
        private int yframes = 0;

        protected bool playing = true;

        protected TimeSpan animationSpeed = TimeSpan.Zero;
        protected TimeSpan lastUpdate = TimeSpan.Zero;

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

        public TimeSpan AnimationSpeed
        {
            get { return animationSpeed; }
            set { animationSpeed = value; }
        }

        #endregion

        public override void Initialize(Microsoft.Xna.Framework.Content.ContentManager content, string path)
        {
            base.Initialize(content, path);

            xframes = sourceImage.Width / frameWidth;
            yframes = sourceImage.Height / frameHeight;
        }

        #region Update

        public override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - lastUpdate > animationSpeed && playing == true)
            {
                currentFrameX++;


                if (currentFrameX >= xframes)
                {
                    timesPlayed++;
                    currentFrameX = 0;
                    currentFrameY++;

                    if (currentFrameY >= yframes)
                    {
                        timesPlayed++;
                        currentFrameY = 0;
                        if (timesPlayed >= Repeat && Repeat != 0)
                        {

                            if (AnimationFinished != null)
                                AnimationFinished(this, new EventArgs());
                            playing = false;
                        }
                    }
                    
                    
                }

                lastUpdate = gameTime.TotalGameTime;
            }
        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch, Vector2 screenOffset)
        {
            spriteBatch.Draw(sourceImage, 
                new Vector2((int)(ConnectedGameObject.Position.X - screenOffset.X),(int)(ConnectedGameObject.Position.Y - screenOffset.Y)),
                new Rectangle(frameWidth * currentFrameX, frameHeight * currentFrameY, frameWidth, frameHeight),
                Color.White, ConnectedGameObject.Rotation, ImageOffset,
                ConnectedGameObject.Scale, SpriteEffects.None, layer);
        }


        #endregion

        #region Methods

        public void Play()
        {
            playing = true;
            currentFrameX = 0;
            currentFrameY = 0;
            timesPlayed = 0;
        }

        public override void MakeCenter()
        {
            ImageOffset = new Vector2(frameWidth / 2, frameHeight / 2);
        }

        #endregion
    }
}
