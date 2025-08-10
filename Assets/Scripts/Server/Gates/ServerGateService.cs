using System.Collections.Generic;
using UnityEngine;
using CesiPlayground.Core;
using CesiPlayground.Core.Events;
using CesiPlayground.Server.Players;
using CesiPlayground.Shared.Events.World;

namespace CesiPlayground.Server.Gates
{
    public class ServerGateService
    {
        private readonly Dictionary<string, GateNode> _gates = new Dictionary<string, GateNode>();
        private ServerPlayerManager _playerManager;

        public ServerGateService()
        {
            ServiceLocator.Register(this);
            GameEventSystem.Server.Register<ServerPlayerUsedGateEvent>(OnPlayerUsedGate);
        }
        
        public void RegisterGate(GateNode gate)
        {
            if (gate != null && !_gates.ContainsKey(gate.Name))
            {
                _gates.Add(gate.Name, gate);
            }
        }
        
        private void OnPlayerUsedGate(ServerPlayerUsedGateEvent e)
        {
            if (_playerManager == null) _playerManager = ServiceLocator.Get<ServerPlayerManager>();
            if (string.IsNullOrEmpty(e.PlayerId) || _playerManager == null) return;

            if (_gates.TryGetValue(e.GateName, out GateNode entryGate) && !entryGate.IsLocked)
            {
                if (_gates.TryGetValue(entryGate.ExitGateName, out GateNode exitGate))
                {                    
                    var playerEntity = _playerManager.GetPlayer(e.PlayerId);
                    if (playerEntity != null)
                    {
                        playerEntity.SetPositionWithoutCorrection(exitGate.Position, exitGate.Rotation);
                    }
                }
            }
        }
    }
}