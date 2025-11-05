/************************************************************
* COPYRIGHT:  2025
* PROJECT: Sandbox
* FILE NAME: RotateTowardsTarget.cs
* DESCRIPTION: Rotates an object toward a target each frame using Quaternion.LookRotation & Quaternion.Slerp.
*              Frame-rate independent (Time.deltaTime) and can optionally stop at the target.
*              Rotation can start automatically on Awake or via Rotate() calls.
* 
* USAGE: Attach to a GameObject, assign a target, and configure rotation in the Inspector.
*                   
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2025/10/18 | Akram Taghavi-Burris | Created class
*
*
************************************************************/

using JetBrains.Annotations;
using UnityEngine;
 

public class RotateTowardsTarget : MonoBehaviour
{
    
    // ===== Hidden Fields =====
    
    // Runtime rotation flag
    private bool _isRotating;
    
    // ===== Inspector Fields =====

    [SerializeField]
    [Tooltip("Target object to rotate towards")]
    private Transform Target;
    
    [SerializeField]
    [Range(0f, 360)]
    [Tooltip("Rotation speed in degrees per second.")]
    private float _speed = 5f;
    
    
    [SerializeField]
    [Tooltip("Enable to rotate object towards target on Start.")]
    private bool _rotateOnStart = true;
    
    [SerializeField]
    [Tooltip("Automatically stop moving when the target is reached.")]
    private bool _stopOnTarget = true;
    
    [SerializeField]
    [Tooltip("Keep the object's X rotations when rotating to look at target.")]
    private bool _lockX = true;
    
    
    // ===== Public Properties =====

    public float Speed
    {
        get => _speed; 
        // Validate that speed is not greater than MAX_SPEED
        set => _speed = Mathf.Clamp(value, 0f, 360);
    }

    
    // Start is called once before the first Update
    private void Start()
    {
        // Determine if the object should start moving
        _isRotating= _rotateOnStart;
        
    } //end Start()

 
    // Update is called once per frame
    private void Update()
    {
        
        if(_isRotating)
        {
            Rotate();

        }//end if(_isMoving)

    }//end Update()
    
    
    private bool IsTargetValid([CanBeNull] Transform target)
    {
        // Use the passed values or fall back to the default inspector-assigned values
        Transform currentTarget = target ?? Target;
        
        //Set target to current target
        Target = currentTarget;

        // If null target Return (exit Move)
        if (currentTarget == null)
        {
            Debug.LogWarning("Move called but target is null!");
            return false;
        }
        
        return true;
        
    }//end IsTargetValid()

    
    /// <summary>
    /// Rotates the object around a specified axis at the current rotation speed.
    /// </summary>
    /// <param name="target">The transform off the game object to move towards.</param>
    /// <param name="speed">The speed at which the object should move (optional).</param>
    public void Rotate([CanBeNull] Transform target = null,  float? speed = null)
    {
        // If not valid target found stop moving, and exit method
        if (!IsTargetValid(target))
        {
            _isRotating = false;
            return;
            
        }//end if (!IsTargetValid)

        // Resolve passed speed value
        //ResolveSpeed(speed);
        
        // Get target's position
        Vector3 targetPosition = Target.position;

        // Ignore the target's vertical difference
       // targetPosition.y = transform.position.y;

        // Calculate the direction vector from this object to the target
        Vector3 direction = targetPosition - transform.position;

        // Ignore the target's vertical difference
      //  direction .x = transform.position.x;
        
      // Look at target position (rotate)
      transform.LookAt(targetPosition);


        // Create a rotation that looks along the direction vector
       // Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly (spherical) interpolate the current rotation towards the target rotation
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


    } // end RotateObject()
    
    /// <summary>
    /// Stops the object's movement by updating the movement flag.
    /// </summary>
    public void Stop()
    {
        // Flags the object as stopped
        _isRotating = false;

    }//end Stop()
 
}//end RotateTowardsTarget
