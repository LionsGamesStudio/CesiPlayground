using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Games;
using Assets.Scripts.Core.Gates;
using Assets.Scripts.Core.Spawn.Spawners;
using Assets.Scripts.Core.Wave;
using Assets.Scripts.Process.Events;
using Assets.Scripts.Process.GunClub.Main;
using Assets.Scripts.Process.Object;
using Assets.Scripts.Utilities;
using Assets.Scripts.Utilities.Sound;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class GunClubWave : IWave
{
    private Game _game;    
    private DataGunClubWave _currentLevelData;

    private Spawner _spawner;


    public GunClubWave(Game game, DataGunClubWave _currentLevel)
    {
        _game = game;
        _currentLevelData = _currentLevel;
        _spawner = _game.Spawner;
    }

    /// <summary>
    /// Initialize the wave
    /// </summary>
    public void InitializeLevel()
    {
        if(_currentLevelData == null) return;

        Debug.Log("Launch ITERATOR");
        _game.StartCoroutine(LaunchSpawnIterator());
    }

    public void ResetLevel()
    {
        _game.Spawner.ClearSpawner();
    }


    /// <summary>
    /// Launch the spawn of targets
    /// </summary>
    /// <returns></returns>
    private IEnumerator LaunchSpawnIterator()
    {

        for (int i = 0; i < _currentLevelData.NumberOfTarget; i++)
        {
            GameObject newTarget = null;

            if (_game.Spawner.PositionsAvailable.Count <= 0) yield return new WaitUntil(() => _game.Spawner.PositionsAvailable.Count > 0);

            // Generate the position of spawn
            int randNumber = Random.Range(0, _game.Spawner.PositionsAvailable.Count);

            // Generate the type of target to spawn
            int randTarget = Random.Range(0, _currentLevelData.Prefab.Count);

            // Check if the target prefab has a Target script
            if (_currentLevelData.Prefab[randTarget].GetComponent<Target>() == null)
            {
                Debug.LogError("No Target script found on target prefab");
                yield break;
            }

            // Spawn the target
            newTarget = _spawner.Spawn<GameObject>(randNumber, _currentLevelData.Prefab[randTarget]);

            // Check if the target is spawned
            if(newTarget != null)
            {
                newTarget.GetComponent<Target>().game = _game;

                // Dispawn target if a certain number of time has passed
                OnDispawnRequestSend request = new OnDispawnRequestSend();
                request.ObjectToDispawn = newTarget;
                request.Spawner = _game.Spawner;
                request.TimeBeforeDispawn = _currentLevelData.CooldownAlive;

                EventBus<OnDispawnRequestSend>.Raise(request);
            }

            yield return new WaitForSeconds(_currentLevelData.CooldownSpawn);
        }

        // Level Finished
        Debug.Log("Condition finish level");

        yield return new WaitUntil(() => _game.Spawner.SpawnPosibilitiesAlreadyUsed.Count <= 0);
        
        // Send Event Wave Finished
        OnWaveFinished onWaveFinished = new OnWaveFinished();
        onWaveFinished.Game = _game;
        EventBus<OnWaveFinished>.Raise(onWaveFinished);

    }
}
