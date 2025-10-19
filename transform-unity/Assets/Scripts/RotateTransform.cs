/************************************************************
* COPYRIGHT:  2025
* PROJECT: Sandbox
* FILE NAME: RotateTransform.cs
*
* DESCRIPTION: Rotates an object every frame using Transform.Rotate.
*              Rotation is frame-rate independent (multiplied by Time.deltaTime)
*              and applies around the Y-axis by default.
*             Rotation speed can be adjusted through a public property.
*
* USAGE: Attach to any GameObject to make it rotate continuously while active in the scene.
*                   
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2025/10/18 | Akram Taghavi-Burris | Created class
*
*
************************************************************/
 
using UnityEngine;
using UnityEngine.Serialization;


public class RotateTransform : MonoBehaviour
{
    // ===== Hidden Fields =====
    
    // Possible rotation axis
    private enum RotationAxis { X, Y, Z }
    
    // Axis to rotate around
    private Vector3 _axis;
    
    // Runtime rotation flag
    private bool _isRotating;

        
    // ===== Inspector Fields =====
    
    [SerializeField]
    [Tooltip("If checked, rotation is applied in world space; otherwise, uses the object's local axes.")]
    private bool _useWorldSpace = false;
    
    [SerializeField]
    [Tooltip("Select the axis to rotate around.")]
    private RotationAxis _rotationAxis = RotationAxis.Y;
    
    [FormerlySerializedAs("_rotationSpeed")]
    [SerializeField]
    [Range(0f, 360)]
    [Tooltip("Rotation speed in degrees per second.")]
    private float _speed = 5f;
    
    
    [SerializeField]
    [Tooltip("Enable to rotate the object on Awake.")]
    private bool _rotateOnAwake = true;
    
    
    // ===== Public Properties =====

    public float Speed
    {
        get => _speed; 
        // Validate that speed is not greater than MAX_SPEED
        set => _speed = Mathf.Clamp(value, 0f, 360);
    }
    

    // Awake is called once on initialization (before Start)
    private void Awake()
    {
        
        // Switch expression to evaluate 'axis' and returns the corresponding Vector3.
        _axis = _rotationAxis switch
        {
            RotationAxis.X => Vector3.right,
            RotationAxis.Y => Vector3.up,
            RotationAxis.Z => Vector3.forward,
            _ => Vector3.up
        };
        
        // Determine if the object should start moving
        _isRotating = _rotateOnAwake;

    } //end Awake()

 
    // Update is called once per frame
    private void Update()
    {
        if (_isRotating)
        {
            RotateObject();
            
        }//end if(_isRotating)
        
    } //end Update()
 
 
    /// <summary>
    /// Rotates the object around a specified axis at the current rotation speed.
    /// </summary>
    /// <param name="axis">The axis to rotate around. If null, the default _rotationAxis is used (optional).</param>
    /// <param name="speed">The speed at which the object should rotate (optional).</param>
    public void RotateObject(Vector3? axis  = null, float? speed = null)
    {
        // Use the passed values or fall back to the default inspector-assigned values
        Vector3 rotationAxis = axis ?? _axis;
        float rotationSpeed = speed ?? Speed;
        
        //Debug.Log(rotationAxis);
        
        // Update properties to ensure validation and internal consistency
        Speed = rotationSpeed;
        
        // Apply rotation
        transform.Rotate(rotationSpeed * Time.deltaTime * rotationAxis, _useWorldSpace ? Space.World : Space.Self);
        
    } // end RotateObject()

 
 
}//end RotateTransform
