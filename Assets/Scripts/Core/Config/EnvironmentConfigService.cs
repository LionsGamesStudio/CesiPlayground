using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using CesiPlayground.Core;

namespace CesiPlayground.Core.Config
{
    /// <summary>
    /// A central service that loads and provides access to environment-specific
    /// configuration values from the 'environment.json' file in the Resources folder.
    /// This service is used by both the client and the server.
    /// </summary>
    public class EnvironmentConfigService
    {
        private Dictionary<string, string> _config;

        public EnvironmentConfigService()
        {
            ServiceLocator.Register(this);
            LoadConfig();
        }

        private void LoadConfig()
        {
            // At runtime, this will load the 'environment.json' file that
            // our editor script has placed in a 'Resources' folder.
            var configFile = Resources.Load<TextAsset>("environment");
            if (configFile != null && !string.IsNullOrEmpty(configFile.text))
            {
                _config = JsonConvert.DeserializeObject<Dictionary<string, string>>(configFile.text);
                Debug.Log($"[EnvironmentConfigService] Configuration loaded successfully for environment.");
            }
            else
            {
                Debug.LogWarning("[EnvironmentConfigService] No 'environment.json' file found in Resources, or the file is empty. The application will rely on fallback values in scripts.");
                _config = new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// Gets a configuration value for a given key.
        /// </summary>
        /// <param name="key">The key of the configuration value (e.g., "clientApiUrl").</param>
        /// <param name="defaultValue">A fallback value to use if the key is not found in the config file.</param>
        /// <returns>The configured value from the JSON file, or the provided default value if the key is missing.</returns>
        public string GetValue(string key, string defaultValue = "")
        {
            if (_config.TryGetValue(key, out var value))
            {
                return value;
            }
            
            // Log a warning if a key is requested but not found, as this might indicate a misconfiguration.
            Debug.LogWarning($"[EnvironmentConfigService] Key '{key}' not found in configuration. Using default value: '{defaultValue}'");
            return defaultValue;
        }
    }
}