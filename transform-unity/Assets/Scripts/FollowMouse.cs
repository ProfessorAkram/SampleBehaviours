/************************************************************
* COPYRIGHT:  Year
* PROJECT: Name of Project or Assignment
* FILE NAME: FollowMouse.cs
* DESCRIPTION: Short Description of script.
*                   
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2000/01/01 | Your Name | Created class
*
*
************************************************************/

using System;
using UnityEngine;
 using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    // ===== Hidden Fields =====
    
    // Reference to Main Camera
    private Camera _mainCamera;
    
    // Maximum speed allowed
    private const float MAX_SPEED = 10f;
    
    // Runtime movement flag
    private bool _isMoving;
    
    // Z-depth from camera
    private float _depth;

    //private Vector3 _offset;
    
    // ===== Inspector Fields =====
    [SerializeField]
    [Range(0f, MAX_SPEED)]
    [Tooltip("Speed of object (units per second). " +
             "Cannot exceed maximum speed.")]
    private float _speed = 5f;
    
    [SerializeField]
    [Tooltip("If enabled, the object keeps its current Z position when following the mouse." +
             "Disable to use camera depth instead.")]
    private bool _useObjectZ = true;
    
    [SerializeField]
    [Tooltip("Enable to have the object follow mouse on Start.")]
    private bool _moveOnStart = true;


    // Start is called once before the first Update
    private void Start()
    {
        // Get reference to main camera
        _mainCamera = Camera.main;
        
        // Calculate the offset based on where the object starts
       // _offset = transform.position - GetMouseWorldPosition();
        
        // Determine if the object should start moving
        _isMoving = _moveOnStart;
        
    } //end Start()
    
    
    // Update is called once per frame
    private void Update()
    {
        
        if(_isMoving)
        {
            MoveToMouse();

        }//end if(_isMoving)

    }//end Update()


    /// <summary>
    /// Moves the GameObject smoothly toward the current mouse position
    /// using linear interpolation based on the assigned speed.
    /// </summary>
    public void MoveToMouse()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        
        // Move toward the mouse
        transform.position = Vector3.Lerp(transform.position, mousePosition, _speed * Time.deltaTime);
    }
    

    /// <summary>
    /// Retrieves the mouse position in world space by converting the
    /// current screen position of the mouse using the main camera.
    /// </summary>
    /// <returns>
    /// A <see cref="Vector3"/> representing the mouse position in world space.
    /// </returns>
    private Vector3 GetMouseWorldPosition()
    {
      
        // Get the current mouse position in screen coordinates
        Vector2 screenMousePosition = Mouse.current.position.ReadValue();
        
        
        // Use the object's current Z position instead of the camera's
        if (_useObjectZ)
        {
            // Distance from the camera to the object
            _depth = Mathf.Abs(_mainCamera.transform.position.z - transform.position.z);
        }
        else
        {
            // Distance from the camera’s origin
            _depth = _mainCamera.transform.position.z;
        }


        // Add an offset to the depth (z-axis) to place the position further from the camera
        Vector3 worldMousePosition = _mainCamera.ScreenToWorldPoint(new Vector3(screenMousePosition.x, screenMousePosition.y, _depth));

        Debug.Log($"Screen Mouse Position: {screenMousePosition}, World Mouse Position with Offset: {worldMousePosition}");

        return worldMousePosition;

    }//end GetMouseWorldPosition()
 
 
}//end FollowMouse
