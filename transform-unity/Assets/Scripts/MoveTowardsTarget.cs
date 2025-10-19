/************************************************************
 * COPYRIGHT:  2025
 * PROJECT: Sandbox
 * FILE NAME: MoveTowardsTarget.cs
 * 
 * DESCRIPTION: Moves an object toward a target each frame using Vector3.MoveTowards.
 *              Frame-rate independent (Time.deltaTime) and can optionally stop at the target.
 *              Movement can start automatically on Awake or via Move() calls.
 * 
 * USAGE: Attach to a GameObject, assign a target, and configure movement in the Inspector.
 *
 * REVISION HISTORY:
 * Date [YYYY/MM/DD] | Author | Comments
 * ------------------------------------------------------------
 * 2025/10/17 | Akram Taghavi-Burris | Created class
 *
 *
 ************************************************************/


using JetBrains.Annotations;
using UnityEngine;

public class MoveTowardsTarget : MonoBehaviour
{
    // ===== Hidden Fields =====
    
    // Maximum speed allowed
    private const float MAX_SPEED = 10f;
    
    // Runtime movement flag
    private bool _isMoving;

        
    // ===== Inspector Fields =====
    
    [SerializeField]
    [Tooltip("Target object to move towards")]
    private Transform Target;
    
    [SerializeField]
    [Range(0f, MAX_SPEED)]
    [Tooltip("Speed of object (units per second). " +
             "Cannot exceed maximum speed.")]
    private float _speed = 5f;
    
    [SerializeField]
    [Tooltip("Enable to move object towards target on Start.")]
    private bool _moveOnStart = true;
    
    [SerializeField]
    [Tooltip("Automatically stop moving when the target is reached.")]
    private bool _stopOnTarget = true;
    
    [SerializeField]
    [Tooltip("Rotates object to look at (face) target while moving")]
    private bool _lookAtTarget = true;
    
    // ===== Public Properties =====
    
    public float Speed
    {
        get => _speed; 
        // Validate that speed is not greater than MAX_SPEED
        set => _speed = Mathf.Clamp(value, 0f, MAX_SPEED);
    }
    

    // Awake is called once on initialization         
    private void Awake()
    {
        // Validate initial speed  via properties
        Speed = _speed;


        
    }//end Awake()
    
    
    // Start is called once before the first Update
    private void Start()
    {
        // Determine if the object should start moving
        _isMoving = _moveOnStart;
        
    } //end Start()
    
    

    // Update is called once per frame
    private void Update()
    {
        
        if(_isMoving)
        {
            Move();

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
    /// Moves the object in a specified direction at a specified speed.
    /// </summary>
    /// <param name="target">The transform off the game object to move towards (optional).</param>
    /// <param name="speed">The speed at which the object should rotate (optional).</param>
    public void Move(Transform target = null, float? speed = null)
    {
        // If not valid target found stop moving, and exit method
        if (!IsTargetValid(target))
        {
            _isMoving = false;
            return;
            
        }//end if (!IsTargetValid)
        
        // Use the provided speed values if not null; otherwise keep the current Speed
        Speed = speed ?? Speed;
        
        // Reference to Target's position
        Vector3 targetPosition = Target.position;

        // Ignore vertical difference, to stay upright and only rotate around Y
        targetPosition.y = transform.position.y;

        // Flags the object as moving
        _isMoving = true;
        
        // If enabled, rotate the object to face the target while moving.
        // The rotation will snap instantly the object's forward vector toward the target position.
        if (_lookAtTarget)
        {
            transform.LookAt(targetPosition);
            
        }// end if (_lookAtTarget)
        
        // Move toward the target using MoveTowards
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
        
        //Debug.Log("Target's Position: " + targetPosition);
        //Debug.Log("Object's Positon " + transform.position);

        // Optional: stop moving when object reaches the target
        if (_stopOnTarget && (transform.position == targetPosition))
        {
            _isMoving = false;
            
        }//end if(_stopOnTarget)
        
    }//end Move()
    
    
    /// <summary>
    /// Stops the object's movement by updating the movement flag.
    /// </summary>
    public void Stop()
    {
        // Flags the object as stopped
        _isMoving = false;

    }//end Stop()

}//end MoveTowardsTarget
