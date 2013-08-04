using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BluEngine.ScreenManager
{
    /// <summary>
    /// Stores some 1x1 textures of solid colours.
    /// </summary>
    public abstract class SolidColours
    {
        private static Texture2D red, green, blue, cyan, magenta, yellow, black, white, darkgray, gray, lightgray, dimgray, darkdarkgray;

        private static Texture2D InitializeColour(Color color, out Texture2D tex)
        {
            tex = new Texture2D(ScreenManager.Instance.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            tex.SetData<Color>(new Color[] { color });
            return tex;
        }

        public static Texture2D Red { get { return red ?? InitializeColour(Color.Red, out red); } }
        public static Texture2D Green { get { return green ?? InitializeColour(Color.Green, out green); } }
        public static Texture2D Blue { get { return blue ?? InitializeColour(Color.Blue, out blue); } }
        public static Texture2D Cyan { get { return cyan ?? InitializeColour(Color.Cyan, out cyan); } }
        public static Texture2D Yellow { get { return yellow ?? InitializeColour(Color.Yellow, out yellow); } }
        public static Texture2D Magenta { get { return magenta ?? InitializeColour(Color.Magenta, out magenta); } }
        public static Texture2D Black { get { return black ?? InitializeColour(Color.Black, out black); } }
        public static Texture2D White { get { return white ?? InitializeColour(Color.White, out white); } }
        public static Texture2D DarkGray { get { return darkgray ?? InitializeColour(Color.DarkGray, out darkgray); } }
        public static Texture2D Gray { get { return gray ?? InitializeColour(Color.Gray, out gray); } }
        public static Texture2D LightGray { get { return lightgray ?? InitializeColour(Color.LightGray, out lightgray); } }
        public static Texture2D DimGray { get { return dimgray ?? InitializeColour(Color.DimGray, out dimgray); } }
        public static Texture2D DarkDarkGray { get { return darkdarkgray ?? InitializeColour(Color.FromNonPremultiplied(20,20,20,255), out darkdarkgray); } }
    }
}
