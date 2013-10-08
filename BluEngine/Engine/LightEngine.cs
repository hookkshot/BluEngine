using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using BluEngine.Engine.GameObjects;

namespace BluEngine.Engine
{
    class LightEngine
    {
        #region Fields

        private static LightEngine instance;

        public static LightEngine Instance
        {
            get
            {
                if (instance == null)
                    return new LightEngine();
                return instance;
            }
        }

        private List<Light> lights = new List<Light>();
        public List<Light> Lights
        {
            get { return lights; }
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            for (int i = lights.Count-1; i >= 0; i--)
            {
                if (!lights[i].ConnectedGameObject.Active)
                    lights.RemoveAt(i);
            }
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            for (int i = 0; i < lights.Count; i++)
            {
                lights[i].DrawLight(spriteBatch, offset);
            }
        }

        #endregion
    }
}
