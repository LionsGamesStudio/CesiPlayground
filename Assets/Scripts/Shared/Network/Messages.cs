using System;
using UnityEngine;
using CesiPlayground.Shared.Events.Input;
using CesiPlayground.Shared.Data.Players;
using CesiPlayground.Shared.Games;

namespace CesiPlayground.Shared.Network
{
    public interface INetworkMessage { }

    // --- Messages Client -> Serveur ---

    [Serializable]
    public class LoginRequestMessage : INetworkMessage { public string Username; public string Password; }
    [Serializable]
    public class RegisterRequestMessage : INetworkMessage { public string Username; public string Password; }
    [Serializable]
    public class PlayerActionMessage : INetworkMessage { public PlayerActionType actionType; public InputActionState state; }
    [Serializable]
    public class PlayerTransformUpdateMessage : INetworkMessage { public Vector3 Position; public Quaternion Rotation; }

    /// <summary>
    /// Sent by the client to the server to acknowledge that it has
    /// finished loading a scene and is ready at a specific spawn point.
    /// </summary>
    [Serializable]
    public class ClientReadyInSceneMessage : INetworkMessage { public Vector3 Position; public Quaternion Rotation; }

    [Serializable]
    public class QueueForGameMessage : INetworkMessage { public string GameId; public GameDifficulty Difficulty; }

    [Serializable]
    public class PlayerUsedGateMessage : INetworkMessage { public string gateName; }

    [Serializable]
    public class PlayerMovementMessage : INetworkMessage { public Vector2 moveValue; }

    // --- Messages Serveur -> Client ---

    [Serializable]
    public class LoginResponseMessage : INetworkMessage { public bool Success; public PlayerData PlayerData; public string ErrorMessage; }
    [Serializable]
    public class RegisterResponseMessage : INetworkMessage { public bool Success; public string ErrorMessage; }
    [Serializable]
    public class SpawnObjectMessage : INetworkMessage { public int networkId; public string prefabId; public Vector3 position; public Quaternion rotation; public Vector3 initialVelocity; }
    [Serializable]
    public class DestroyObjectMessage : INetworkMessage { public int networkId; }
    [Serializable]
    public class PlayerTransformCorrectionMessage : INetworkMessage { public Vector3 Position; public Quaternion Rotation; }
    [Serializable]
    public class InstantiateGameMessage : INetworkMessage { public string gameInstanceId; public string gameDataId; public Vector3 position; }
    [Serializable]
    public class ReturnToHubMessage : INetworkMessage { public string gameInstanceId; public Vector3 hubReturnPosition; }
    [Serializable]
    public class GameStateUpdateMessage : INetworkMessage { public string gameInstanceId; public float localPlayerScore; }
    [Serializable]
    public class GameStatusMessage : INetworkMessage { public string gameInstanceId; public string statusText; }
    [Serializable]
    public class GameLivesUpdatedMessage : INetworkMessage { public string gameInstanceId; public int remainingLives; }
}