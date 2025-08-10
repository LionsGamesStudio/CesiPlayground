
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using CesiPlayground.Client.LevelDesign;
using CesiPlayground.Client.UI;
using CesiPlayground.Shared.Data.HubLayoutData;

namespace CesiPlayground.EditorTools
{
    public class StandBaker
    {
        [MenuItem("Tools/Bake Hub Stand Layout")]
        public static void BakeStandLayout()
        {
            Debug.Log("Baking Hub stand layout...");
            var allStands = Object.FindObjectsByType<GameStand>(FindObjectsSortMode.None);

            if (allStands.Length == 0)
            {
                Debug.LogWarning("No GameStand components found in the scene. Nothing to bake.");
                return;
            }

            string assetPath = "Assets/Resources/ScriptableObjects/GameData/HubLayoutData.asset";
            HubLayoutData layoutData = AssetDatabase.LoadAssetAtPath<HubLayoutData>(assetPath);
            if (layoutData == null)
            {
                layoutData = ScriptableObject.CreateInstance<HubLayoutData>();
                Directory.CreateDirectory(Path.GetDirectoryName(assetPath));
                AssetDatabase.CreateAsset(layoutData, assetPath);
            }

            layoutData.StandConfigurations.Clear();

            foreach (var stand in allStands)
            {
                if (stand.GameData == null)
                {
                    Debug.LogWarning($"GameStand on '{stand.gameObject.name}' is missing a DataGame reference.", stand.gameObject);
                    continue;
                }

                var newConfig = new StandConfiguration
                {
                    GameDataId = stand.GameData.GameId,
                    StandPosition = stand.transform.position,
                    SpawnPoints = new List<Vector3>()
                };

                var spawnMarkers = stand.GetComponentsInChildren<SpawnPointMarker>();
                foreach (var marker in spawnMarkers)
                {
                    newConfig.SpawnPoints.Add(marker.transform.position);
                }
                
                layoutData.StandConfigurations.Add(newConfig);
            }

            EditorUtility.SetDirty(layoutData);
            AssetDatabase.SaveAssets();
            Debug.Log($"Successfully baked {layoutData.StandConfigurations.Count} stands into {assetPath}");
        }
    }
}
#endif

    