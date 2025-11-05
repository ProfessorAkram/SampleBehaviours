/************************************************************
 * COPYRIGHT:  2025
 * PROJECT: Sandbox
 * FILE NAME: LookAtTargetClamped.cs
 * 
 * DESCRIPTION: Rotates a GameObject to face a target smoothly in all directions using Transform.LookAt.
 *              Clamps rotation to avoid clipping into the ground or walls.
 *
 * USAGE: Attach to a GameObject, assign a target, and configure rotation in the Inspector.
 * 
 * REVISION HISTORY:
 * Date [YYYY/MM/DD] | Author | Comments
 * ------------------------------------------------------------
 * 2025/10/18 | Akram Taghavi-Burris | Created class
 * 2025/10/19 | Updated for smooth clamped rotation
 *
 ************************************************************/
using UnityEngine;

public class LookAtTargetClamped : MonoBehaviour
{
    // ===== Hidden Fields =====
    
    // Record initial rotation
    private Quaternion _initialRotation;
    
    // Runtime rotation flag
    private bool _isRotating;
    
  
    // ===== Inspector Fields =====
    
    [SerializeField]
    [Tooltip("Target object to rotate towards")]
    private Transform _target;
    
    [SerializeField]
    [Range(0f, 360)]
    [Tooltip("Speed of object's rotation (units per second). ")]
    private float _speed = 5f;
    
    [SerializeField]
    [Tooltip("Enable to rotate the object on Start.")]
    private bool _rotateOnStart = true;
    
    [Header("CLAMP SETTINGS (in degrees)")]
    
    [SerializeField]
    [Tooltip("Minimum allowed rotation up/down (X-axis).")]
    private float minPitch = -45f;
    
    [SerializeField]
    [Tooltip("Maximum allowed rotation up/down (X-axis).")]
    private float maxPitch = 45f;

    [SerializeField]
    [Tooltip("Minimum allowed rotation left/right (y-axis).")]
    private float minYaw = -90f;
    
    [SerializeField]
    [Tooltip("Maximum allowed rotation left/right (y-axis).")]
    private float maxYaw = 90f;
    
    
    // ===== Public Properties =====

    public Transform Target 
    {   get => _target; 
        set => _target = value; 
    }
    
    public float Speed
    {
        get => _speed; 
        // Validate that speed is not greater than MAX_SPEED
        set => _speed = Mathf.Clamp(value, 0f, 360);
    }
    
    

    // Start is called once before the first Update
    private void Start()
    {
        // Set initial rotation
        _initialRotation = transform.localRotation;
        
        // Determine if the object should start moving
        _isRotating= _rotateOnStart;
        
    } //end Start()
 
    // Update is called once per frame
    private void Update()
    {
        
        if(_isRotating)
        {
            LookAtTarget();

        }//end if(_isMoving)

    }//end Update()
    
    private bool IsTargetValid(Transform target)
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
    public void LookAtTarget(Transform target = null)
    {
        // If not valid target found stop moving, and exit method
        if (!IsTargetValid(target))
        {
            _isRotating = false;
            return;
            
        }//end if (!IsTargetValid)

        // Reference to Target's position
        Vector3 targetPosition = Target.position;

        // Calculate direction to target
        Vector3 direction = targetPosition - transform.position;
        
        // Set target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Convert to *local* offset from initial rotation
        Quaternion relativeRotation = Quaternion.Inverse(_initialRotation) * targetRotation;
        
        // Work with relative Euler angles
        Vector3 euler = targetRotation.eulerAngles;
        
        // Convert angles >180 into negative range
        if (euler.x > 180f) euler.x -= 360f;
        if (euler.y > 180f) euler.y -= 360f;
        
        // Clamp pitch (X) and yaw (Y)
        euler.x = Mathf.Clamp(euler.x, minPitch, maxPitch);
        euler.y = Mathf.Clamp(euler.y, minYaw, maxYaw);
        euler.z = 0f; // Keep roll zero
        
        // Convert back to Quaternion
        Quaternion clampedRotation = Quaternion.Euler(euler);

        // Smoothly rotate using Slerp
        transform.rotation = Quaternion.Slerp(transform.rotation, clampedRotation, Speed * Time.deltaTime);

    } // end RotateObject()
    
    /// <summary>
    /// Stops the object's movement by updating the movement flag.
    /// </summary>
    public void Stop()
    {
        // Flags the object as stopped
        _isRotating = false;

    }//end Stop()
 
 
}//end LookAtTargetClamped
