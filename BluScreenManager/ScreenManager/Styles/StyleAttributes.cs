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

        /// <summary>
        /// Gets the Nullable attribute from the attributes list.
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve.</typeparam>
        /// <param name="attribute">The name of the attribute.</param>
        /// <returns>The attribute value, or null if it was not found or was an incompatible type.</returns>
        public Nullable<T> ValueAttributeLookup<T>(String attribute) where T : struct
        {
            T? attVal = SafeCast<T>(this[attribute]);
            if (attVal.HasValue)
                return attVal.Value;
            return null;
        }

        /// <summary>
        /// Gets the Reference attribute from the attributes list.
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve.</typeparam>
        /// <param name="attribute">The name of the attribute.</param>
        /// <returns>The attribute value, or null if it was not found or was an incompatible type.</returns>
        public T ReferenceAttributeLookup<T>(String attribute) where T : class
        {
            T val = SafeRefCast<T>(this[attribute]);
            if (val != null)
                return val;
            return null;
        }

        /// <summary>
        /// Converts an object to a Nullable<T>, if possible.
        /// </summary>
        /// <typeparam name="T">The primitive type to convert.</typeparam>
        /// <param name="value">The candidate for conversion.</param>
        /// <returns>A nullable value</returns>
        public static Nullable<T> SafeCast<T>(object value) where T : struct
        {
            if (value != null)
            {
                try
                {
                    return (T)value;
                }
                catch
                {
                    ;
                }
            }
            return null;
        }

        /// <summary>
        /// Converts an object to a T, if possible.
        /// </summary>
        /// <typeparam name="T">The reference type to convert.</typeparam>
        /// <param name="value">The candidate for conversion.</param>
        /// <returns>A nullable value</returns>
        public static T SafeRefCast<T>(object value) where T : class
        {
            if (value != null)
            {
                T ret = (value as T);
                if (ret != null)
                    return ret;
            }
            return null;
        }
    }
}
