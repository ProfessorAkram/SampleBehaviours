/************************************************************
 * COPYRIGHT:  2025
 * PROJECT: Sandbox
 * FILE NAME: MoveTransform.cs
 * 
 * DESCRIPTION: Moves an object each frame using either Transform.position 
 *              or Transform.Translate, based on user preference.
 *              Movement is frame-rate independent (multiplied by Time.deltaTime)
 *              and uses world coordinates by default. 
 *              Speed and direction are validated via public properties.
 * 
 * USAGE: Attach to any GameObject to move it automatically on Awake or via Move() calls.
 *
 * REVISION HISTORY:
 * Date [YYYY/MM/DD] | Author | Comments
 * ------------------------------------------------------------
 * 2025/09/01 | Akram Taghavi-Burris | Created class
 *
 *
 ************************************************************/
 

using UnityEngine;

public class MoveTransform : MonoBehaviour
{
    // ===== Hidden Fields =====
    
    // Maximum speed allowed
    private const float MAX_SPEED = 10f;
    
    // Runtime movement flag
    private bool _isMoving;

        
    // ===== Inspector Fields =====
    
    [SerializeField]
    [Tooltip("Use Translate to move along the object's local axes.")]
    private bool _useTranslate = false;
    
    [SerializeField]
    [Range(0f, MAX_SPEED)]
    [Tooltip("Speed of object (units per second). " +
             "Cannot exceed maximum speed.")]
    private float _speed = 5f;
    
    [SerializeField]
    [Tooltip("Direction of movement. " +
             "Auto normalized for consistent movement.")]
    private Vector3 _direction = Vector3.right; 
    
    
    [SerializeField]
    [Tooltip("Enable to move the object on Awake.")]
    private bool _moveOnAwake = true;
    
    // ===== Public Properties =====
    
    public float Speed
    {
        get => _speed; 
        // Validate that speed is not greater than MAX_SPEED
        set => _speed = Mathf.Clamp(value, 0f, MAX_SPEED);
    }
    
    public Vector3 Direction
    {
        get => _direction;
        // Validate by normalizing the direction value
        set => _direction = value.normalized;
    }
    

    // Awake is called once on initialization         
    private void Awake()
    {
        // Validate initial speed and direction via properties
        Speed = _speed;
        Direction = _direction;
        
        // Determine if the object should start moving
        _isMoving = _moveOnAwake;
        
    }//end Awake()
    
    

    // Update is called once per frame
    private void Update()
    {
        
        if(_isMoving)
        {
            Move();

        }//end if(_isMoving)

    }//end Update()
     
    /// <summary>
    /// Moves the object in a specified direction at a specified speed.
    /// </summary>
    /// <param name="direction">The direction to move the object (optional).</param>
    /// <param name="speed">The speed at which the object should move (optional).</param>
    public void Move(Vector3? direction = null, float? speed = null)
    {
        // Resolve the effective values for this frame
        Vector3 moveDirection = direction ?? Direction;
        float moveSpeed = speed ?? Speed;

        // Update properties to ensure validation and internal consistency
        Direction = moveDirection;
        Speed = moveSpeed;

        //Debug.Log("Direction: " + Direction);
        //Debug.Log("Speed " + Speed);
        
        // Flags the object as moving
        _isMoving = true;


       // Move with Translate or Position
        if (_useTranslate)
        {
            
            // This moves relative to the object's local axes by default
            transform.Translate(Speed * Time.deltaTime * Direction);
        }
        else
        {
            // Move the object by directly updating its world position
            transform.position += Speed * Time.deltaTime * Direction;
            
        }//end if (_useTranslate)
            
        
 


    }//end Move()
    
    /// <summary>
    /// Stops the object's movement by updating the movement flag.
    /// </summary>
    public void Stop()
    {
        // Flags the object as stopped
        _isMoving = false;

    }//end Stop()

}//end MoveTransform
