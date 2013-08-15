using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BluEngine.ScreenManager
{
    /// <summary>
    /// Stores some 1x1 textures of solid colours.
    /// </summary>
    public abstract class SolidColours
    {
        private static Texture2D red, green, blue, cyan, magenta, yellow, black, white;
        private static Texture2D black95, black98, black90, black85, black80, black70, black60, black50;

        public static Texture2D TexFromColor(Color color)
        {
            Texture2D tex = new Texture2D(ScreenManager.Instance.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            tex.SetData<Color>(new Color[] { color });
            return tex;
        }

        private static Texture2D InitializeColour(Color color, out Texture2D tex)
        {
            return tex = TexFromColor(color);
        }

        public static Texture2D Red { get { return red ?? InitializeColour(Color.Red, out red); } }
        public static Texture2D Green { get { return green ?? InitializeColour(Color.Green, out green); } }
        public static Texture2D Blue { get { return blue ?? InitializeColour(Color.Blue, out blue); } }
        public static Texture2D Cyan { get { return cyan ?? InitializeColour(Color.Cyan, out cyan); } }
        public static Texture2D Yellow { get { return yellow ?? InitializeColour(Color.Yellow, out yellow); } }
        public static Texture2D Magenta { get { return magenta ?? InitializeColour(Color.Magenta, out magenta); } }
        public static Texture2D White { get { return white ?? InitializeColour(Color.White, out white); } }

        /// <summary>
        /// 100% Black.
        /// </summary>
        public static Texture2D Black { get { return black ?? InitializeColour(Color.Black, out black); } }

        /// <summary>
        /// 98% Black.
        /// </summary>
        public static Texture2D Black98 { get { return black98 ?? InitializeColour(Color.FromNonPremultiplied(5, 5, 5, 255), out black98); } }

        /// <summary>
        /// 95% Black.
        /// </summary>
        public static Texture2D Black95 { get { return black95 ?? InitializeColour(Color.FromNonPremultiplied(13,13, 13, 255), out black95); } }

        /// <summary>
        /// 90% Black.
        /// </summary>
        public static Texture2D Black90 { get { return black90 ?? InitializeColour(Color.FromNonPremultiplied(25, 25, 25, 255), out black90); } }

        /// <summary>
        /// 85% Black.
        /// </summary>
        public static Texture2D Black85 { get { return black85 ?? InitializeColour(Color.FromNonPremultiplied(38, 38, 38, 255), out black85); } }

        /// <summary>
        /// 80% Black.
        /// </summary>
        public static Texture2D Black80 { get { return black80 ?? InitializeColour(Color.FromNonPremultiplied(51, 51, 51, 255), out black80); } }

        /// <summary>
        /// 70% Black.
        /// </summary>
        public static Texture2D Black70 { get { return black70 ?? InitializeColour(Color.FromNonPremultiplied(77, 77, 77, 255), out black70); } }

        /// <summary>
        /// 60% Black.
        /// </summary>
        public static Texture2D Black60 { get { return black60 ?? InitializeColour(Color.FromNonPremultiplied(102, 102, 102, 255), out black60); } }

        /// <summary>
        /// 50% Black.
        /// </summary>
        public static Texture2D Black50 { get { return black50 ?? InitializeColour(Color.FromNonPremultiplied(128, 128, 128, 255), out black50); } }
    }
}
