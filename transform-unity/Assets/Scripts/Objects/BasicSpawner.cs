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
using UnityEngine.Serialization;

public class BasicSpawner : MonoBehaviour
{
    [FormerlySerializedAs("_spawnedObject")]
    [SerializeField]
    [Tooltip("GameObject to spawn")]
    private GameObject _objectToSpawn;
    
    [FormerlySerializedAs("_objectScale")]
    [SerializeField]
    [Tooltip("Scale of the spawned object")]
    private Vector3 _spawnScale = Vector3.one;
    
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
        GameObject spawned = Instantiate(_objectToSpawn, position, rotation);
     
        // Apply scale if specified
        spawned.transform.localScale = _spawnScale;
     

    }//end SpawnObject()


    private Vector3 GetSpawnPosition(Vector3? spawnPosition = null)
    {
   
        //Check for spawn points
        if (_spawnPoints != null && _spawnPoints.Count > 0)
        {
            //If spawn points are random
            if (_useRandomPoint)
            {
                //Pick a random index
                _spawnPointsIndex = Random.Range(0, _spawnPoints.Count);
                
            }//end if (_useRandomPoint)
            
            //Get the spawn point position
            Vector3 pointPosition = _spawnPoints[_spawnPointsIndex].position;
            Debug.Log($"Spawn position: {pointPosition}");
            
            // If not random, increment the index AFTER spawning
            if (!_useRandomPoint)
            {
                _spawnPointsIndex = (_spawnPointsIndex + 1) % _spawnPoints.Count;
            }//end if (!_useRandomPoint)

            //Set spawn point + offset
            return pointPosition + _spawnOffset;
            
        }//end if(_spawnPoints)
        
        // Otherwise, default to spawner's own position + offset
        return transform.position + _spawnOffset;
        
    }//end GetSpawnPosition
    
 
}//end BasicSpawner
