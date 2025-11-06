/************************************************************
* COPYRIGHT:  Year
* PROJECT: Name of Project or Assignment
* FILE NAME: ObjectSpawner.cs
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
using UnityEngine.Serialization;

public class ObjectSpawner : MonoBehaviour
{
    [Header("OBJECT TO SPAWN")]
    [SerializeField]
    [Tooltip("GameObject to spawn")]
    private GameObject _objectToSpawn;
    
    [FormerlySerializedAs("_scale")]
    [SerializeField]
    [Tooltip("Scale of the spawned object")]
    [Range(0,1)]
    private float _spawnScale = 1;
    
    [Header("SPAWN POINTS")]
    [SerializeField]
    [Tooltip("List of spawn points")]
    private List<Transform> _spawnPoints;
    
    [FormerlySerializedAs("_useRandomPoint")]
    [SerializeField]
    [Tooltip("Optional: Choose a random spawn point from list.")]
    private bool _useRandomSpawnPoint = false;
    
    private int _spawnPointIndex = 0;
    
    [SerializeField]
    [Tooltip("Optional: Offset applied to the spawn position (relative to the spawn point).")]
    private Vector3 _spawnOffset = Vector3.zero;
    
    
    [Header("SPAWN BEHAVIORS")]
    [SerializeField]
    [Tooltip("Spawn on Start")]
    private bool _spawnOnStart = true;
    
    [SerializeField]
    [Tooltip("If enabled, the spawner will repeatedly spawn objects at the specified interval. " +
             "If disabled, it will spawn only once at start.")]
    private bool _loopSpawning = true;
    
    [SerializeField]
    [Tooltip("Time delay between spawns")]
    private float _spawnDelay = 2f;
 
 
    // Start is called once before the first Update
    private void Start()
    {
        //Check for continuous spawning
        if (_loopSpawning)
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
    public void SpawnObject()
    {
        if (_objectToSpawn == null)
        {
            Debug.LogWarning("Missing spawn object.");
            return;
            
        }//end if(_spawnObject == null)
        
        // Determine spawn position
        Vector3 position = GetSpawnPosition();

        // Determine spawn rotation
        Quaternion rotation = Quaternion.identity;

        // Instantiate the object
        GameObject spawnedObject = Instantiate(_objectToSpawn, position, rotation);
     
        // Apply scale if specified
        Vector3 spawnedObjectScale = spawnedObject.transform.localScale * _spawnScale;
        spawnedObject.transform.localScale = spawnedObjectScale;
        
    }//end SpawnObject()


    private Vector3 GetSpawnPosition(Vector3? spawnPosition = null)
    {
   
        //Check for spawn points
        if (_spawnPoints != null && _spawnPoints.Count > 0)
        {
            //If spawn points are random
            if (_useRandomSpawnPoint)
            {
                //Pick a random index
                _spawnPointIndex = Random.Range(0, _spawnPoints.Count);
                
            }//end if (_useRandomPoint)
            
            //Get the spawn point position
            Vector3 pointPosition = _spawnPoints[_spawnPointIndex].position;
            Debug.Log($"Spawn position: {pointPosition}");
            
            // If not random, increment the index AFTER spawning
            if (!_useRandomSpawnPoint)
            {
                _spawnPointIndex = (_spawnPointIndex + 1) % _spawnPoints.Count;
            }//end if (!_useRandomPoint)

            //Set spawn point + offset
            return pointPosition + _spawnOffset;
            
        }//end if(_spawnPoints)
        
        // Otherwise, default to spawner's own position + offset
        return transform.position + _spawnOffset;
        
    }//end GetSpawnPosition
    
 
}//end ObjectSpawner
