/************************************************************
* COPYRIGHT:  Year
* PROJECT: Name of Project or Assignment
* FILE NAME: BasicSpawner.cs
* DESCRIPTION: Short Description of script.
*                   
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2000/01/01 | Your Name | Created class
*
*
************************************************************/
using System.Collections.Generic;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;


public class BasicSpawner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("GameObject to spawn")]
    private GameObject _spawnObject;
    
    [SerializeField]
    [Tooltip("List of spawn points")]
    private List<Transform> _spawnPoints;
    
    [SerializeField]
    [Tooltip("Optional: Choose a random spawn point from list.")]
    private bool _useRandomPoint = false;
    
    private int _spawnPointsIndex = 0;
    
    [SerializeField]
    [Tooltip("Optional: Offset applied to the spawn position (relative to the spawn point).")]
    private Vector3 _spawnOffset = Vector3.zero;
    
    [SerializeField]
    [Tooltip("Time delay between spawns")]
    private float _spawnDelay = 2f;

    [SerializeField]
    [Tooltip("Spawn on Start")]
    private bool _spawnOnStart = true;
    
    [SerializeField]
    [Tooltip("If enabled, the spawner will repeatedly spawn objects at the specified interval. " +
             "If disabled, it will spawn only once at start.")]
    private bool _loopSpawn = true;
 
 
    // Start is called once before the first Update
    private void Start()
    {
        //Check for continuous spawning
        if (_loopSpawn)
        {
           StartCoroutine(SpawnLoop()); 
           
        }//end if(_loopSpawn)
        
    } //end Start()

    /// <summary>
    /// Continuously spawns objects at a set interval until stopped manually.
    /// </summary>
    /// <returns>
    /// Coroutine that yields between spawn cycles for the specified delay.
    /// </returns>
    private IEnumerator SpawnLoop()
    {
        // Loop forever (or until you stop it manually)
        while (true)
        {
            SpawnObject();
            yield return new WaitForSeconds(_spawnDelay);
        }
    }
        
    /// <summary>
    /// Spawns the assigned object at a given position and rotation.
    /// If no position or rotation is provided, uses the next spawn point or the spawner's transform.
    /// Optionally applies a custom scale.
    /// </summary>
    /// <param name="spawnPosition">Optional world position for the spawn. Uses spawn point if not set.</param>
    /// <param name="spawnRotation">Optional rotation for the spawn. Uses spawn point if not set.</param>
    /// <param name="spawnScale">Optional scale for the spawned object.</param>
    public void SpawnObject(Vector3? spawnPosition = null, Quaternion? spawnRotation = null, Vector3? spawnScale = null)
    {
        if (_spawnObject == null)
        {
            Debug.LogWarning("Missing spawn object.");
            return;
        }
        
        
        // Pick a spawn point
        
        spawnPosition = spawnPoint.position + _spawnOffset;
        spawnRotation = spawnPoint.rotation;
        GameObject spawned = Instantiate(_spawnObject, spawnPosition, spawnPoint.rotation);


    }

    private Vector3 GetSpawnPosition(Vector3? spawnPosition = null)
    {
        Transform spawnPoint;
        
        //Check for spawn points
        if (spawnPosition == null && _spawnPoints != null && _spawnPoints.Count > 0)
        {
            //If spawn points are random
            if (_useRandomPoint)
            {
                _spawnPointsIndex = Random.Range(0, _spawnPoints.Count);
            }
            else
            {
                // Cycle to next spawn point
                _spawnPointsIndex = (_spawnPointsIndex + 1) % _spawnPoints.Count;
            }
            
            spawnPoint = _spawnPoints[_spawnPointsIndex];

        }
        else if(spawnPosition != null)
        {
           spawnPoint.position = spawnPosition;
        }

        
    }//end GetSpawnPosition
    
 
}//end BasicSpawner
