using UnityEngine;

namespace CesiPlayground.Client.LevelDesign
{
    /// <summary>
    /// A marker component to identify the single, primary spawn point for a player
    /// within a game instance scene (e.g., GunClub_Scene).
    /// </summary>
    public class PlayerSpawnPoint : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.1f, 0.8f, 0.2f, 0.75f); // Green
            Gizmos.DrawSphere(transform.position, 0.5f);
            Gizmos.DrawRay(transform.position, transform.forward * 1.5f);
        }
    }
}