using System.Collections.Generic;

namespace CFLFramework.Utilities.Extensions
{
    public static partial class DictionaryExtensions
    {
        #region BEHAVIORS

        public static bool ContainsKeys(this IDictionary<string, object> dictionary, string[] keys)
        {
            try
            {
                for (int i = 0; i < keys.Length; i++)
                    dictionary = dictionary[keys[i]] as Dictionary<string, object>;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }

            return true;
        }

        public static void CreateKeys(this IDictionary<string, object> dictionary, string[] keys)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (!dictionary.ContainsKey(keys[i]))
                    dictionary.Add(keys[i], new Dictionary<string, object>());

                dictionary = dictionary[keys[i]] as Dictionary<string, object>;
            }
        }

        public static float IncreaseValue(this IDictionary<string, object> dictionary, string[] keys, string key, float value)
        {
            if (!dictionary.ContainsKeys(keys))
                dictionary.CreateKeys(keys);

            for (int i = 0; i < keys.Length; i++)
                dictionary = dictionary[keys[i]] as Dictionary<string, object>;

            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, default(float));

            float currentValue = (float)dictionary[key];
            dictionary[key] = currentValue + value;

            return (float)dictionary[key];
        }

        public static void SetValue(this IDictionary<string, object> dictionary, string[] keys, string key, object value)
        {
            if (!dictionary.ContainsKeys(keys))
                dictionary.CreateKeys(keys);

            for (int i = 0; i < keys.Length; i++)
                dictionary = dictionary[keys[i]] as Dictionary<string, object>;

            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);
        }

        public static T GetValue<T>(this IDictionary<string, object> dictionary, string[] keys, string key)
        {
            if (!dictionary.ContainsKeys(keys))
                dictionary.CreateKeys(keys);

            for (int i = 0; i < keys.Length; i++)
                dictionary = dictionary[keys[i]] as Dictionary<string, object>;

            if (!dictionary.ContainsKey(key))
                return default(T);

            return (T)dictionary[key];
        }

        #endregion
    }
}
