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
 

public class Bomb : MonoBehaviour
{
    private MoveRigidbody _moveRigidbody;
   
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
    
    
 
 

 
}//end Bomb
