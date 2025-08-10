

namespace CesiPlayground.Server.Gameplay.Logic
{
    /// <summary>
    /// Represents the authoritative state of a target on the server.
    /// </summary>
    public class TargetLogic
    {
        public int NetworkId { get; }
        public float ScoreValue { get; }
        public bool IsActive { get; private set; } = true;

        public TargetLogic(int networkId, float scoreValue)
        {
            NetworkId = networkId;
            ScoreValue = scoreValue;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}