      
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using CesiPlayground.Client.LevelDesign;
using CesiPlayground.Shared.Data;

namespace CesiPlayground.EditorTools
{
    public class SpawnPointBaker
    {
        [MenuItem("Tools/Bake Scene Spawn Points")]
        public static void BakeSpawnPoints()
        {
            var activeScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
            string sceneName = activeScene.name;
            Debug.Log($"Baking spawn points for scene: {sceneName}...");

            var allMarkers = Object.FindObjectsByType<SpawnPointMarker>(FindObjectsSortMode.None);
            if (allMarkers.Length == 0)
            {
                Debug.LogWarning("No SpawnPointMarker components found. Nothing to bake.");
                return;
            }

            string assetPath = $"Assets/Resources/ScriptableObjects/GameData/SpawnPoints/{sceneName}_SpawnPoints.asset";

            SpawnPointData data = AssetDatabase.LoadAssetAtPath<SpawnPointData>(assetPath);
            if (data == null)
            {
                data = ScriptableObject.CreateInstance<SpawnPointData>();
                Directory.CreateDirectory(Path.GetDirectoryName(assetPath));
                AssetDatabase.CreateAsset(data, assetPath);
            }
            
            data.GameId = sceneName;
            data.SpawnPositions.Clear();
            foreach (var marker in allMarkers)
            {
                data.SpawnPositions.Add(marker.transform.position);
            }
            
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
            Debug.Log($"Successfully baked {data.SpawnPositions.Count} spawn points into {assetPath}");
        }
    }
}
#endif

    