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
using UnityEngine;
 

public class BasicSpawner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("GameObject to spawn")]
    private GameObject _spawnObject;
    
    [SerializeField]
    [Tooltip("Spawn Point or Points")]
    private List<Transform> _spawnPoints;
    
    private int _spawnPointsIndex = 0;
    
    [SerializeField]
    [Tooltip("Time delay between spawns")]
    private float _spawnDelay = 2f;

 
 
    // Start is called once before the first Update
    private void Start()
    {
        StartCoroutine(SpawnLoop());
        
    } //end Start()


    private IEnumerator SpawnLoop()
    {
        // Loop forever (or until you stop it manually)
        while (true)
        {
            SpawnObject();
            yield return new WaitForSeconds(_spawnDelay);
        }
    }
        
   private void  SpawnObject()
    {
        if (_spawnPoints == null || _spawnPoints.Count == 0 || _spawnObject == null)
        {
            Debug.LogWarning("Missing spawn points or spawn object.");
            return;
        }
        
        // Pick a spawn point
        Transform spawnPoint = _spawnPoints[_spawnPointsIndex];
        GameObject spawned = Instantiate(_spawnObject, spawnPoint.position, spawnPoint.rotation);

        // Cycle to next spawn point
        _spawnPointsIndex = (_spawnPointsIndex + 1) % _spawnPoints.Count;
    }

 
}//end BasicSpawner
