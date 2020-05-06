using System;
using System.Linq;
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

        public static float IncreaseValue(this IDictionary<string, object> dictionary, string[] keys, float value)
        {
            float currentValue = dictionary.GetValue<float>(keys, default(float));
            dictionary.SetValue(keys, currentValue + value);
            return dictionary.GetValue<float>(keys, default(float));
        }

        public static float DecreaseValue(this IDictionary<string, object> dictionary, string[] keys, float value)
        {
            float currentValue = dictionary.GetValue<float>(keys, default(float));
            dictionary.SetValue(keys, currentValue - value);
            return dictionary.GetValue<float>(keys, default(float));
        }

        public static void SetValue(this IDictionary<string, object> dictionary, string[] keys, object newValue)
        {
            for (int i = 0; i < keys.Length - 1; i++)
            {
                if (!dictionary.ContainsKey(keys[i]))
                    dictionary.Add(keys[i], new Dictionary<string, object>());

                dictionary = (Dictionary<string, object>)dictionary[keys[i]];
            }

            if (dictionary.ContainsKey(keys.Last()))
                dictionary[keys.Last()] = newValue;
            else
                dictionary.Add(keys.Last(), newValue);
        }

        public static T GetValue<T>(this IDictionary<string, object> dictionary, string[] keys, object defaultValue)
        {
            defaultValue = defaultValue == null ? default(T) : defaultValue;
            for (int i = 0; i < keys.Length - 1; i++)
            {
                if (!dictionary.ContainsKey(keys[i]))
                    return (T)CastObject(defaultValue);

                if (dictionary[keys[i]].GetType() != typeof(Dictionary<string, object>))
                    return (T)CastObject(defaultValue);

                dictionary = dictionary[keys[i]] as Dictionary<string, object>;
            }

            if (!dictionary.ContainsKey(keys.Last()))
                return (T)CastObject(defaultValue);

            return (T)CastObject(dictionary[keys.Last()]);
        }

        public static List<T> GetValueList<T>(this IDictionary<string, object> dictionary, string[] keys, object defaultValue)
        {
            var list = dictionary.GetValue<object>(keys, defaultValue);
            if (list.GetType() == typeof(List<T>))
                return (List<T>)list;
            else
                return ConvertToList<T>(list);
        }

        public static void MergeValue(this IDictionary<string, object> dictionary, IDictionary<string, object> newDictionary)
        {
            dictionary.MergeWithValue(newDictionary, new List<string>());
        }

        public static bool HasValue(this IDictionary<string, object> dictionary, string[] keys)
        {
            for (int i = 0; i < keys.Length - 1; i++)
            {
                if (!dictionary.ContainsKey(keys[i]))
                    return false;

                if (dictionary[keys[i]].GetType() != typeof(Dictionary<string, object>))
                    return false;

                dictionary = dictionary[keys[i]] as Dictionary<string, object>;
            }

            return dictionary.ContainsKey(keys.Last());
        }

        private static void MergeWithValue(this IDictionary<string, object> dictionary, object newElement, List<string> keys)
        {
            if (newElement.GetType() == typeof(Dictionary<string, object>))
            {
                foreach (KeyValuePair<string, object> element in (Dictionary<string, object>)newElement)
                {
                    var newKeys = new List<string>(keys);
                    newKeys.Add(element.Key);
                    dictionary.MergeWithValue(element.Value, newKeys);
                }
            }
            else
            {
                dictionary.SetValue(keys.ToArray(), newElement);
            }
        }

        private static List<T> ConvertToList<T>(object originalList)
        {
            var newList = new List<T>();
            foreach (object element in (List<object>)originalList)
                newList.Add((T)Convert.ChangeType(element, typeof(T)));

            return newList;
        }

        private static object CastObject(object data)
        {
            if (data.GetType() == typeof(double))
                data = Convert.ToSingle(data);

            if (data.GetType() == typeof(Int64))
                data = Convert.ToInt32(data);

            return data;
        }

        #endregion
    }
}
