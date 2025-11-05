/************************************************************
* COPYRIGHT:  Year
* PROJECT: Name of Project or Assignment
* FILE NAME: MoveRigidbody.cs
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
 

using UnityEngine;

[RequireComponent(typeof(Rigidbody))] 
public class MoveRigidbody : MonoBehaviour
{
    
    // Reference to the object's Rigidbody component
    private Rigidbody _rigidBody;
    
    //Reference to the object's Mass
    private float _mass;
    
    // Maximum speed allowed
    private const float MAX_SPEED = 50f;
    
    // Store the last applied values for optimization
    private float _currentSpeed;
    private Vector3 _currentDirection;
    
    // Flag to prevent multiple braking applications
    private bool _isBraking = false;

    
    // Serialized fields for initial values
    
    [Header("MOVEMENT SETTINGS")]
    
    [SerializeField]
    [Tooltip("Controls how movement forces are applied:\n" +
             "• Force – Gradual, realistic acceleration (affected by mass).\n" +
             "• Acceleration – Gradual but ignores mass (like wind).\n" +
             "• Impulse – Instant burst, affected by mass (like a jump or explosion).\n" +
             "• VelocityChange – Instant burst, ignores mass (arcade-style movement).")]
    private ForceMode _forceMode = ForceMode.Force;
        
    [SerializeField]
    [Tooltip("Should the object be moving on start?")]
    private bool _moveOnStart = true;
    
    [SerializeField]
    [Tooltip("Direction of movement. Will be normalized automatically to ensure consistent movement.")]
    private Vector3 _direction = Vector3.right;
    
    [SerializeField]
    [Range(0f, MAX_SPEED)]
    [Tooltip("Target speed for the object’s movement. Clamped to MAX_SPEED.")]
    private float _speed = 5f;
    
    [SerializeField]
    [Tooltip("Acceleration magnitude applied toward the target velocity.\n" +
             "• Controls how strong the applied force/acceleration is when using ForceMode Force Acceleration.\n" +
             "• Larger values = stronger acceleration (reaches target speed faster).\n" +
             "• Smaller values = weaker acceleration (reaches target speed more gradually).")]
    private float _accelerationMultiplier = 100f;
    
    [SerializeField]
    [Tooltip("Enable to calculate force based on acceleration time instead of using a fixed acceleration multiplier. \n" +
             "• Produces smoother, time-based ramp-up to the target speed.")]
    private bool _useAccelerationTime = false;
    
    [SerializeField]
    [Tooltip("Duration (in seconds) over which the object accelerates to its target speed.\n" +
             "• Unity spreads the acceleration across physics steps automatically.\n" +
             "• Smaller values = quicker, more abrupt acceleration.\n" +
             "• Larger values = slower, smoother acceleration.")]
    private float _accelerationTime = 0.5f;
    
    [SerializeField]
    [Tooltip(
        "Velocity below which the object is considered stopped.\n\n" +
        "Use Cases:\n" +
        "- Tiny objects / precise movement: 0.01 – 0.05 units/sec\n" +
        "- Player characters / standard objects: 0.05 – 0.1 units/sec\n" +
        "- Heavy objects / vehicles: 0.1 – 0.5 units/sec"
    )]
    private float _stopThreshold = 0.1f;

   
    
#if UNITY_EDITOR
    [Header("FOR TESTING ONLY")]
    [SerializeField] 
    private bool _enableEditorTesting = false;

    //Enum for list of possible actions to test 
    private enum TestAction
    {
        None,   // No action selected
        Move,   // Run Move() test
        Stop,   // Run Stop() test
        Brake  //Run ApplyBraking() test

    }

    [SerializeField]
    [Tooltip("Select which action to test in the Editor.")]
    private TestAction _testAction = TestAction.None;

#endif
    
    
    // Public properties with encapsulation
    
    public float Speed
    {
        get => _speed; 
        set => _speed = Mathf.Clamp(value, 0f, MAX_SPEED);
    }
    
    public Vector3 Direction
    {
        get => _direction;
        set => _direction = value.normalized;
    }
    
    

    // Awake is called once on initialization         
    private void Awake()
    {
        // Validate initial speed and direction via properties
        Speed = _speed;
        Direction = _direction;
        
        //Set reference to the Rigidbody component
        _rigidBody = GetComponent<Rigidbody>(); 
        
        //Ensure that Rigidbody is dynamic
        _rigidBody.isKinematic = false; 
        
        //Set reference to the object's mass
        _mass = _rigidBody.mass;

    }//end Awake()
    
    
    // Start is called once before the first Update
    private void Start()
    {
        //Store current speed and direction
        _currentSpeed = Speed;
        _currentDirection = Direction;
        
        //Check if the object moves on start
        if (_moveOnStart)
        {
            Move();

        }//end if(_moveOnStart)
        
    }//end Start()
    

    // Update is called once per frame
    private void Update()
    {
        
        //Debug.Log(this.name + " velocity: " + _rigidBody.linearVelocity);
        
#if UNITY_EDITOR
        if (_enableEditorTesting)
        {
            RunMovementTest();

        } //end if(_enableEditorTesting)
#endif
    }//end Update()
    
    
    //Called at fixed intervals (i.e., physic steps) 
    private void FixedUpdate()
    {
        // Updated movement if speed or direction changed
        if (Speed != _currentSpeed || Direction != _currentDirection)
        {
            Move(Direction, Speed);
        }
        
        
        //If braking, countinue until stopped
        if (_isBraking)
        {
            Brake();
        }

        
    }//end FixedUpdate()
    
    
    /// <summary>
    /// Clamps the Rigidbody's velocity so it never exceeds MAX_SPEED.
    /// </summary>
    private void ClampMaxSpeed()
    {

        if (_rigidBody.linearVelocity.magnitude > MAX_SPEED)
        {
            _rigidBody.linearVelocity = _rigidBody.linearVelocity.normalized * MAX_SPEED;
        }

    }//end ClampMaxSpeed() 
    
     
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

        //Record the current direction and speed
        _currentDirection = Direction;
        _currentSpeed = Speed;

        // Move the GameObject using Rigidbody velocity
        //_rigidBody.linearVelocity = Speed * Direction;
        
        
        // Desired velocity based on current speed and direction
        Vector3 targetVelocity = Speed * Direction;
        Debug.Log("target velocity: " + targetVelocity);
        
        // Velocity difference to reach target
        Vector3 deltaVelocity = targetVelocity - _rigidBody.linearVelocity; 
        Debug.Log("delta velocity: " + deltaVelocity);
        
        // Instant force (like a sudden push)
        Vector3 impulse = deltaVelocity * _mass; 

        Vector3 acceleration; 

        //If using accelerated time for more control
        if (_useAccelerationTime)
        {
            // Gradually reach target velocity over a specified acceleration time
            acceleration = deltaVelocity / _accelerationTime;
            Debug.Log("acceleration over time : " + acceleration);
        }
        else
        {
            // Scale delta velocity by multiplier for responsive movement
            acceleration = deltaVelocity * _accelerationMultiplier;
            Debug.Log("acceleration multipler : " + acceleration);
        }//end if (_useAccelerationTime)
        
        
        // Flags the object as moving
        //_isMoving = true;
        
        ApplyForce(deltaVelocity, impulse, acceleration);

    }//end Move()
    
    /// <summary>
    /// Returns true if the Rigidbody is currently moving above the stop threshold.
    /// </summary>
    public bool IsMoving()
    {
        return _rigidBody.linearVelocity.sqrMagnitude > _stopThreshold * _stopThreshold;
    }
    

    /// <summary>
    /// Applies force to the Rigidbody based on the selected ForceMode.
    /// </summary>
    /// <param name="deltaVelocity">
    /// The difference between the target velocity and the current Rigidbody velocity.
    /// Used for ForceMode.VelocityChange to instantly adjust velocity.
    /// </param>
    /// <param name="impulse">
    /// The instantaneous change in momentum (mass × delta velocity).
    /// Used for ForceMode.Impulse to apply a sudden push.
    /// </param>
    /// <param name="acceleration">
    /// The acceleration vector to apply, either scaled by a multiplier or calculated over an acceleration time.
    /// Used for ForceMode.Force and ForceMode.Acceleration.
    /// </param>
    private void ApplyForce(Vector3 deltaVelocity,  Vector3 impulse, Vector3 acceleration)
    {
        switch (_forceMode)
        {
            case ForceMode.Force:
                // Applies a continuous force based on acceleration and mass 
                _rigidBody.AddForce(acceleration * _mass, ForceMode.Force);
                break;

            case ForceMode.Acceleration:
                // Applies a continuous acceleration
                _rigidBody.AddForce(acceleration, ForceMode.Acceleration);
                break;

            case ForceMode.Impulse:
                // Applies an instant force (like a sudden push), mass affects velocity change
                _rigidBody.AddForce(impulse, ForceMode.Impulse);
                break;

            case ForceMode.VelocityChange:
                // Instantly changes velocity, ignores mass (like directly setting velocity)
                _rigidBody.AddForce(deltaVelocity, ForceMode.VelocityChange);
                break;

            default:
                Debug.LogWarning("Unhandled ForceMode: " + _forceMode);
                break;
        }//end switch(_forceMode)
        

    }//end ApplyForce()
    
    
    /// <summary>
    /// Gradually slows the object by applying a consistent braking force
    /// regardless of mass.
    /// </summary>
    public void Brake()
    {
        //Check if the object is not moving
        if (!IsMoving())
        {
            // Ensures object is fully stopped
            Stop();

            //Reset braking state
            _isBraking = false;

            return;  
        }
        
        // Mark braking as active
        _isBraking = true;
        
        Debug.Log("Braking rigid body is " + _isBraking);
        
        // Get the Rigidbody's current velocity
        Vector3 currentVelocity = _rigidBody.linearVelocity;

        // Calculate deceleration, opposite to current velocity
        Vector3 deceleration = -currentVelocity / _accelerationTime;
          
        // Use Acceleration for consistent braking regardless of mass
        _rigidBody.AddForce(deceleration, ForceMode.Acceleration);

    } //end Brake()
    

    /// <summary>
    /// Stops the object's movement immediately by zeroing its Rigidbody velocity.
    /// </summary>
    public void Stop()
    {
        // Immediately halts motion
        _rigidBody.linearVelocity = Vector3.zero;  
        
        Debug.Log("Stopping rigid body");
        
    }//end Stop()
    

#if UNITY_EDITOR    
    /// <summary>
    /// Runs the selected movement test in the Editor based on the _testAction enum.
    /// </summary>
    private void RunMovementTest()
    {
        switch (_testAction)
        {
            case TestAction.Move:
                Debug.Log("Testing Move");
                Move();
                break;

            case TestAction.Stop:
                Debug.Log("Testing Stop");  
                Stop();
                break;
            
            case TestAction.Brake:
                Debug.Log("Testing Brakes");  
                Brake();
                break;
            
            case TestAction.None:
                Debug.Log("Testing None");
                //Do nothing
                break;
                
            default:
                Debug.Log("Unhandled TestAction: " + _testAction); 
                break;

        }//end switch(_testAction)

    }//end RunMovementTest()
#endif
    
}//end MoveRigidbody
