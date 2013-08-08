using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BluEngine.Engine.GameObjects
{
    public class ViewScreen
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
    }
}
