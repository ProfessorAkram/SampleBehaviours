/************************************************************
* COPYRIGHT:  Year
* PROJECT: Name of Project or Assignment
* FILE NAME: Bomb.cs
* DESCRIPTION: Short Description of script.
*                   
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2000/01/01 | Your Name | Created class
*
*
************************************************************/
 
using UnityEngine;
 

public class Bomb : MonoBehaviour, IHazard
{
    private MoveRigidbody _moveRigidbody;
    
    [SerializeField] 
    [Tooltip("The amount of damage hazard deals")]
    private int damageAmount = 10;
    public int DamageAmount => damageAmount;
    
    
   
    // Start is called once before the first Update
    private void Start()
    {
        //Try and get the MoveRigidbody component
        if (TryGetComponent<MoveRigidbody>(out _moveRigidbody))
        {
            //Set the direction forward
            _moveRigidbody.Direction = transform.forward;
        }
        
    } //end Start()
    
    /// <summary>
    /// Called when the hazard is destroyed or triggered. 
    /// Implement this method to define what happens when the hazard is removed from the scene (e.g., explosion, animation, effects).
    /// </summary>
    public void HandleDestroy()
    {
        Explode();

    }//end HandleDestroy()


    private void Explode()
    {
        BasicSpawner _spawner; 
        if (TryGetComponent<BasicSpawner>(out _spawner))
        {
            _spawner.SpawnObject();
        }
    }
    
 
}//end Bomb
