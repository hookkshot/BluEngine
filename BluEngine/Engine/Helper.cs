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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Vector2 RadiansToVector(float radian)
        {
            throw new NotImplementedException("This method is not implemented in this version of BluEngine ");
        }

        public static string HashString(string _value)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(_value);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++) ret += data[i].ToString("x2").ToLower();
            return ret;
        }
    }
}
