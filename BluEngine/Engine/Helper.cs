using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace BluEngine.Engine
{
    public static class Helper
    {
        /// <summary>
        /// Takes a Vector2 direction and will convert to angle measured in radians.
        /// </summary>
        /// <returns>The direction in radians</returns>
        public static float VectorToRadians(Vector2 vector)
        {
            return (float)Math.Atan2(vector.X, -vector.Y);
        }
    }
}
