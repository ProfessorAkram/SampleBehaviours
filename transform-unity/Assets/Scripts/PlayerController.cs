/************************************************************
* COPYRIGHT:  2025
* PROJECT: Name of Project or Assignment
* FILE NAME: PlayerController.cs
* 
* DESCRIPTION: 
*
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2025/10/22 | Akram Taghavi-Burris | Created class
*
*
************************************************************/
 
using UnityEngine;
using UnityEngine.InputSystem;
 
public class PlayerController : MonoBehaviour
{
    //Reference to the MoveTransform component 
    private MoveTransform _moveTransform;
    
    // Start is called once before the first Update
    private void Start()
    {
        // Check if MoveTransform component DOES NOT EXIST
        if (!TryGetComponent<MoveTransform>(out _moveTransform))
        {
            Debug.LogError("MoveTransform component missing!");

        }//end if(!TryGetComponent<MoveTransform>(out _moveTransform))
      
    }//end Start()
 
    
    /// <summary>
    /// Triggered by the Move input Action. Converts the 2D input from the player
    /// (keyboard, joystick, or gamepad) into a 3D direction and passes it to the 
    /// MoveTransform component to move the player.
    /// </summary>
    /// <param name="value">
    /// The InputValue wrapper passed automatically by the Player Input component. 
    /// </param>
    public void OnMove(InputValue value)
    {
        // Extract the Vector2 from the InputValue
        Vector2 inputVector = value.Get<Vector2>();
        Debug.Log("OnMove called: " + inputVector);

        // Convert the 2D input into a 3D direction on the XZ plane
        Vector3 direction = new Vector3(inputVector.x, 0f, inputVector.y);
        Debug.Log("OnMove direction: " + direction);
        

        // Call the movement component with the calculated direction
        _moveTransform.Move(direction);

    }//end OnMove()
    
 
}//end PlayerController