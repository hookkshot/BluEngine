using System;
using System.Collections.Generic;

namespace BluEngine.ScreenManager.Styles
{
    public class StyleAttributes
    {
        private Dictionary<String, object> attributes;

        /// <summary>
        /// Access an attribute of a style. Example: someAttributesObject["background"] = ...; Do not use "null" to represent "no attribute"; call Delete() using the name of the attribute you'd like to remove instead.
        /// </summary>
        /// <param name="attribute">The string ID of the attribute to access.</param>
        /// <returns>The object value of the attribute, or null if it did not exist.</returns>
        public object this[String attribute]
        {
            get
            {
                if (attribute == null || attribute.Length == 0)
                    return null;

                object attr = null;
                attributes.TryGetValue(attribute, out attr);
                return attr;
            }
            set
            {
                if (value == null || attribute == null || attribute.Length == 0)
                    return;
                attributes[attribute] = value;
            }
        }

        public void Delete(String attribute)
        {
            attributes.Remove(attribute);
        }

        public StyleAttributes()
        {
            attributes = new Dictionary<string, object>();
        }
    }
}
