using System;

namespace CesiPlayground.Shared.Data.Players
{
    /// <summary>
    /// Represents the persistent data for a player.
    /// This is a simple data container (DTO) used for serialization
    /// and communication between client and server.
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        // The unique ID is the most important field.
        public string PlayerId;
        public string PlayerName;
    }
}