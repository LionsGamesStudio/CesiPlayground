using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Game;
using Assets.Scripts.Core.Spawn.Spawners;
using Assets.Scripts.Core.Wave;
using Assets.Scripts.Process.Events;
using Assets.Scripts.Process.GunClub.Game;
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
    

    private float _nextSpawnTime;
    private int _numberOfTargetSpawned;

    private List<GameObject> _objectsAlive = new List<GameObject>();


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

        _numberOfTargetSpawned = 0;
        Debug.Log("Launch ITERATOR");
        _game.StartCoroutine(LaunchSpawnIterator());
    }

    public void ResetLevel()
    {
        _numberOfTargetSpawned = 0;
    }

    private void OnTriggerEnterTarget(Collider other, GameObject obj)
    {
        if (!other.gameObject.CompareTag("Bullet")) return;

        GameObject.Destroy(other.gameObject);

        other.enabled = false;
        Target target = obj.GetComponent<Target>();

        if (target == null) return;

        target.IsHit(_game.CurrentPlayer.PlayerData);
        target.PlayEffect(target.effectOnDestroy);

        if (target.gameObject.GetComponent<OnDestroyPlaySound>() != null)
            target.gameObject.GetComponent<OnDestroyPlaySound>().OnBeforeDestroy(() =>
            {
                SendDispawnRequest(target.gameObject);
            });
        else
        {
            SendDispawnRequest(target.gameObject);
        }
    }


    /// <summary>
    /// Launch the spawn of targets
    /// </summary>
    /// <returns></returns>
    private IEnumerator LaunchSpawnIterator()
    {
        int numberOfTryTolerance = 10;

        for (int i = 0; i < _currentLevelData.NumberOfTarget; i++)
        {
            int numberOfTry = 0;
            bool isSpawned = false;
            GameObject newTarget = null;

            // Try to spawn a target until the number of try is reached or the target is spawned
            while (numberOfTry < numberOfTryTolerance && !isSpawned)
            {
                // Generate the position of spawn
                int randNumber = Random.Range(0, _currentLevelData.NumberOfTarget);

                // Generate the type of target to spawn
                int randTarget = Random.Range(0, _currentLevelData.Prefab.Count);

                // Check if the target prefab has a TargetController script
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
                    isSpawned = true;
                    newTarget.GetComponent<Target>().game = _game;

                    _objectsAlive.Add(newTarget);

                    // Dispawn target if a certain number of time has passed
                    SendDispawnRequest(newTarget, _currentLevelData.CooldownAlive);

                    // Add target hit logic
                    TriggerListener trigger = newTarget.AddComponent<TriggerListener>();
                    trigger.onTriggerEnter += OnTriggerEnterTarget;
                }

                numberOfTry++;
            }

            _numberOfTargetSpawned++;

            // Check if the number of target spawned is equal to the number of target to spawn
            if (_numberOfTargetSpawned == _currentLevelData.NumberOfTarget)
            {
                Debug.Log("Condition finish level");

                #region Wait for Target to Dispawn
                yield return new WaitUntil(() =>
                {
                    bool stillAlive = false;
                    foreach (GameObject target in _objectsAlive)
                    {
                        if (target != null)
                        {
                            stillAlive = true;
                        }
                    }
                    return stillAlive;
                });

                yield return new WaitForSeconds(_currentLevelData.CooldownAlive);
                yield return new WaitForSeconds(2);

                #endregion

                // Send Event Wave Finished
                OnWaveFinished onWaveFinished = new OnWaveFinished();
                onWaveFinished.Game = _game;
                EventBus<OnWaveFinished>.Raise(onWaveFinished);


                yield break;
            }

            yield return new WaitForSeconds(_currentLevelData.CooldownSpawn);
            
        }
    }

    private void SendDispawnRequest(GameObject target, float time = 0)
    {
        OnDispawnRequestSend request = new OnDispawnRequestSend();
        request.ObjectToDispawn = target;
        request.Spawner = _spawner;

        if(time > 0) 
            request.TimeBeforeDispawn = _currentLevelData.CooldownAlive;

        EventBus<OnDispawnRequestSend>.Raise(request);
    }

}
