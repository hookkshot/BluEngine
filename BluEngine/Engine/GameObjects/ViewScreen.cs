
using Microsoft.Xna.Framework;

namespace BluEngine.Engine.GameObjects
{
    public class ViewScreen : GameObject
    {
        private int width = 0;
        private int height = 0;

        public ViewScreen(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get
            {
                return height;
            }
        }

        public override Microsoft.Xna.Framework.Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value - new Vector2(width / 2, height / 2); ;
            }
        }
    }
}
