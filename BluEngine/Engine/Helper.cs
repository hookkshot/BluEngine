using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        /// <summary>
        /// Allows for information to posted and retrieved from PHP pages
        /// </summary>
        /// <param name="_URL">url to post too.</param>
        /// <param name="_postString">information to post.</param>
        /// <returns>Content of page written.</returns>
        public static string WebPost(string _URL, string _postString)
        {
            const string REQUEST_METHOD_POST = "POST";
            const string CONTENT_TYPE = "application/x-www-form-urlencoded";

            Stream dataStream = null;
            StreamReader reader = null;
            WebResponse response = null;
            string responseString = null;

            WebRequest request = WebRequest.Create(_URL);

            request.Method = REQUEST_METHOD_POST;

            string postData = _postString;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = CONTENT_TYPE;
            request.ContentLength = byteArray.Length;

            dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            response = request.GetResponse();

            dataStream = response.GetResponseStream();

            reader = new StreamReader(dataStream);

            responseString = reader.ReadToEnd();

            if (reader != null) reader.Close();
            if (dataStream != null) dataStream.Close();
            if (response != null) response.Close();

            return responseString;
        }


        /// <summary>
        /// Allows text to be absolute centered.
        /// </summary>
        /// <param name="font"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Vector2 CenterTextPosition(SpriteFont font, string text)
        {
            return new Vector2(-font.MeasureString(text).X / 2,-font.MeasureString(text).Y / 2);
        }
    }
}
