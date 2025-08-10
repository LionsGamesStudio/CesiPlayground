      
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

namespace CesiPlayground.EditorTools
{
    /// <summary>
    /// Contains editor utility scripts to help configure XR setups.
    /// </summary>
    public static class XRSetupTools
    {
        /// <summary>
        /// Finds all TeleportationArea components in the active scene, checks for an adjacent
        /// TerrainCollider, and adds it to the area's collider list if it's missing.
        /// </summary>
        [MenuItem("Tools/XR/Setup Terrain Colliders for Teleport Areas")]
        public static void AutoSetupTerrainColliders()
        {
            // Find all instances of the TeleportationArea component in the currently open scene.
            var teleportAreas = Object.FindObjectsByType<TeleportationArea>(FindObjectsSortMode.None);

            if (teleportAreas.Length == 0)
            {
                Debug.Log("No Teleportation Area components found in this scene.");
                return;
            }

            int updatedCount = 0;
            Debug.Log($"Found {teleportAreas.Length} Teleportation Area(s). Checking for unlinked Terrain Colliders...");

            foreach (var area in teleportAreas)
            {
                // Check if the same GameObject also has a TerrainCollider component.
                if (area.TryGetComponent<TerrainCollider>(out var terrainCollider))
                {
                    if (!area.colliders.Contains(terrainCollider))
                    {
                        area.colliders.Add(terrainCollider);

                        // Mark the component as "dirty". This is crucial to tell Unity
                        // that a change has been made and needs to be saved.
                        EditorUtility.SetDirty(area);
                        
                        updatedCount++;
                        Debug.Log($"Updated TeleportationArea on GameObject '{area.gameObject.name}': Added missing TerrainCollider reference.", area.gameObject);
                    }
                }
            }

            if (updatedCount > 0)
            {
                Debug.Log($"Successfully updated {updatedCount} Teleportation Area(s). Please remember to save your scene to apply the changes.");
            }
            else
            {
                Debug.Log("All Teleportation Areas with TerrainColliders were already configured correctly. No changes needed.");
            }
        }
    }
}
#endif