using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CesiPlayground.Core.Storing
{
    public class Blackboard
    {
        private Dictionary<string, object> _data = new Dictionary<string, object>();

        /// <summary>
        /// Set value to blackboard. If key already exists, it will be overwritten.
        /// </summary>
        /// <typeparam name="T">Type of the value to store</typeparam>
        /// <param name="key">Identifier of the value</param>
        /// <param name="value">Value to store</param>
        public void Set<T>(string key, T value)
        {
            if (_data.ContainsKey(key))
            {
                _data[key] = value;
            }
            else
            {
                _data.Add(key, value);
            }
        }

        /// <summary>
        /// Get value from blackboard. If key does not exist, default value will be returned.
        /// </summary>
        /// <typeparam name="T">Type of the value stored</typeparam>
        /// <param name="key">Key representing the value to get</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (_data.ContainsKey(key))
            {
                return (T)_data[key];
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Remove value from blackboard.
        /// </summary>
        /// <param name="key">Identifier of the value to remove</param>
        public void Remove(string key)
        {
            if (_data.ContainsKey(key))
            {
                _data.Remove(key);
            }
        }

        /// <summary>
        /// Clear all values from blackboard.
        /// </summary>
        public void Clear()
        {
            _data.Clear();
        }

        /// <summary>
        /// Check if blackboard contains a value with the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return _data.ContainsKey(key);
        }

        /// <summary>
        /// Check if blackboard contains a specific value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ContainsValue<T>(T value)
        {
            return _data.ContainsValue(value);
        }
    }
}
