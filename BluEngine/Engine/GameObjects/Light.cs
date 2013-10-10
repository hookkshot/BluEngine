using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.Engine.GameObjects
{
    public class Light : GameObjectComponent
    {
        private Texture2D lightMask;
        private Color lightColor = Color.White;
        private string sourceName = "";
        private bool on = true;

        public Texture2D LightMask
        {
            get { return lightMask; }
            set { lightMask = value; }
        }

        public string SourceName
        {
            get { return sourceName; }
            set { sourceName = value; }
        }

        public bool On
        {
            get { return on; }
            set { on = value; }
        }

        public override void Initialize(Microsoft.Xna.Framework.Content.ContentManager content, string path)
        {
            try
            {
                if(lightMask == null)
                    lightMask = content.Load<Texture2D>(Path.Combine(path, sourceName));

            }
            catch (Exception e)
            {

            }

            if (lightMask != null)
            {
                LightEngine.Instance.Lights.Add(this);
            }
        }

        public void DrawLight(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (on)
            {
                Transform transform = (Transform)connectedGameObject[typeof(Transform)];
                spriteBatch.Draw(lightMask, transform.Position - offset, lightMask.Bounds, lightColor, transform.Rotation, new Vector2(lightMask.Width / 2, lightMask.Height), transform.Scale, SpriteEffects.None, 1f);
            }
        }
    }
}
