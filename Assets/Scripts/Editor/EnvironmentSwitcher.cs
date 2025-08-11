      
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

namespace CesiPlayground.EditorTools
{
    public static class EnvironmentSwitcher
    {
        private const string c_ConfigRootPath = "Assets/Config";
        private const string c_ResourcesPath = "Assets/Resources";
        private const string c_ActiveConfigFileName = "environment.json";

        [MenuItem("Environnement/Switch to Development")]
        public static void SetDevelopment()
        {
            SwitchEnvironment("dev");
        }

        [MenuItem("Environnement/Switch to Production")]
        public static void SetProduction()
        {
            SwitchEnvironment("prod");
        }

        private static void SwitchEnvironment(string env)
        {
            string sourceFileName = $"environment.{env}.json";
            string sourcePath = Path.Combine(c_ConfigRootPath, sourceFileName);
            
            if (!File.Exists(sourcePath))
            {
                Debug.LogError($"Configuration file not found at '{sourcePath}'. Please ensure it exists.");
                return;
            }

            // Ensure the Resources directory exists.
            if (!Directory.Exists(c_ResourcesPath))
            {
                Directory.CreateDirectory(c_ResourcesPath);
            }

            string destinationPath = Path.Combine(c_ResourcesPath, c_ActiveConfigFileName);
            
            try
            {
                // Copy the selected environment file into Resources, overwriting if it exists.
                File.Copy(sourcePath, destinationPath, true);
                
                // Re-import the asset so Unity's cache and internal references are updated immediately.
                AssetDatabase.ImportAsset(destinationPath);
                
                Debug.Log($"✅ Environment successfully switched to '{env.ToUpper()}'. The active config is now based on '{sourceFileName}'.");
            }
            catch (IOException e)
            {
                Debug.LogError($"Failed to switch environment: {e.Message}");
            }
        }
    }
}
#endif

    