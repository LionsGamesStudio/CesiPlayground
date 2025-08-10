using UnityEngine;

namespace CesiPlayground.Server.Gates
{
    /// <summary>
    /// Represents a logical gate/teleporter on the server.
    /// It's a pure data class, not a MonoBehaviour.
    /// </summary>
    public class GateNode
    {
        public string Name { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation => Quaternion.identity; // Gates are always aligned to world axes
        public string ExitGateName { get; private set; }
        public bool IsLocked { get; private set; }

        public GateNode(string name, Vector3 position, string exitGateName, bool isLocked = false)
        {
            Name = name;
            Position = position;
            ExitGateName = exitGateName;
            IsLocked = isLocked;
        }

        public void Lock() => IsLocked = true;
        public void Unlock() => IsLocked = false;
        public void SetExit(string newExitGateName) => ExitGateName = newExitGateName;
    }
}