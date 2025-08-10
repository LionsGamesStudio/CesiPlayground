using UnityEngine;
using System.Collections.Generic;
using CesiPlayground.Shared.Events.Interfaces;

namespace CesiPlayground.Shared.Events.Game
{
    /// <summary>
    /// Fired on the SERVER BUS when a game session's logic has concluded.
    /// Listened to by the ScoreService to report final scores.
    /// </summary>
    public struct GameSessionFinishedEvent : IEvent
    {
        public string GameId;
        public Dictionary<string, float> FinalScores;
    }

    /// <summary>
    /// Fired on the SERVER BUS when a player successfully scores points in a game.
    /// Listened to by the GameSessionController to update its internal state.
    /// </summary>
    public struct PlayerScoredEvent : IEvent
    {
        public string GameId;
        public string PlayerId;
        public float Points;
    }


    // --- Événements du Bus Client (Réactions aux Ordres du Serveur) ---

    /// <summary>
    /// Fired on the CLIENT BUS when the UI needs to be updated with the current game state
    /// received from the server. Listened to by the GameView to update score text, etc.
    /// </summary>
    public struct GameStateUpdatedEvent : IEvent
    {
        public string GameId;
        public float LocalPlayerScore;
    }

    /// <summary>
    /// Fired on the CLIENT BUS to update a game's status text (e.g., "Wave 2", "Game Over").
    /// Listened to by a GameView.
    /// </summary>
    public struct GameStatusUpdatedEvent : IEvent
    {
        public string GameInstanceId;
        public string StatusText;
    }

    /// <summary>
    /// Fired on the CLIENT BUS to update a game's remaining lives display.
    /// Listened to by a GameView.
    /// </summary>
    public struct GameLivesUpdatedEvent : IEvent
    {
        public string GameInstanceId;
        public int RemainingLives;
    }
}