/************************************************************
* COPYRIGHT:  Year
* PROJECT: Name of Project or Assignment
* FILE NAME: ShrinkGrow.cs
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
 

public class ShrinkGrow : MonoBehaviour
{
    
    // ===== Hidden Fields =====
    
    // The scaling behavior the object performs
    private enum ScaleMode
    {
        Grow,       // Scale from current to maximum
        Shrink,     // Scale from current to minimum
        ShrinkGrow  // Scale back and forth between min and max
    }
    
    // The behavior preformed once scaling completes
    private enum EndBehavior
    {
        Stop,       // stops at target scale
        Loop,       // continuously back and forth
        Reset       // returns to original scale after finishing
    }
    
    // Maximum speed allowed
    private const float MAX_SPEED = 10f;
    
    // Flag to check if object is growing
    private bool _isGrowing = true;
    
    // The object's scale at the start of the scene, used as a reference for relative scaling
    private Vector3 _initialScale;
    
    // Calculated minimum scale the object can shrink to, considering initial scale and multipliers
    private Vector3 _targetMinScale;

    // Calculated maximum scale the object can grow to, considering initial scale and multipliers
    private Vector3 _targetMaxScale;
    
    
    // ===== Inspector Fields =====
    
    [SerializeField]
    [Tooltip("Choose the scale mode to use.\n"+
             "Grow: Scale from current to maximum.\n"+
             "Shrink: Scale from current to minimum.\n"+
             "ShrinkGrow: Scale back and forth between min and max.")]
    private ScaleMode _scaleMode = ScaleMode.Grow;
    
    [SerializeField]
    [Tooltip("Minimum scale multiplier relative to the object's initial scale")]
    private float _scaleMultiplierMin = 0.5f;

    [SerializeField]
    [Tooltip("Maximum scale multiplier relative to the object's initial scale")]
    private float _scaleMultiplierMax = 2f;

    [SerializeField]
    [Tooltip("Absolute minimum scale to clamp to")]
    private Vector3 _absoluteMinScale = new Vector3(0.1f, 0.1f, 0.1f);

    [SerializeField]
    [Tooltip("Absolute maximum scale to clamp to")]
    private Vector3 _absoluteMaxScale = new Vector3(5f, 5f, 5f);
    
    [SerializeField]
    [Range(0f, MAX_SPEED)]
    [Tooltip("Speed scale transition (units per second).\n " +
             "Cannot exceed maximum speed.")]
    private float _speed = 5f;
    
    [SerializeField]
    [Tooltip("Enable to shrink/grow on Start.")]
    private bool _scaleOnStart = true;
    
    [SerializeField]
    [Tooltip("Choose the end behavior to use.\n"+
             "Stop: stops scaling once the target scale is reached.\n" +
             "Loop: continuously repeats the scaling sequence (ping-pong).\n"+
             "Reset: returns the object to its initial scale after completing the scaling action.")]
    
    private EndBehavior _endBehavior = EndBehavior.Stop;
    
    
    // Start is called once before the first Update
    private void Start()
    {
        // Record the initial scale
        _initialScale = transform.localScale;
        
        // Calculate the target min/max scale by applying the relative multiplier and clamping to the absolute min/max
        _targetMinScale = Vector3.Max(_initialScale * _scaleMultiplierMin, _absoluteMinScale);
        _targetMaxScale = Vector3.Min(_initialScale * _scaleMultiplierMax, _absoluteMaxScale);
        
        
        CheckScaleMode();

        if (_scaleOnStart)
        {
            if 
        }

    } //end Start()
 
 
    // Update is called once per frame
    private void Update()
    {

    } //end Update()
 
    /// <summary>
    /// A custom method example.
    /// </summary>
    private void CheckScaleMode()
    {
        switch (_scaleMode)
        {
            case ScaleMode.Grow:
                _isGrowing = true;
                break;
            
            case ScaleMode.Shrink:
                _isGrowing = false;
                break;
            
            case ScaleMode.ShrinkGrow:
                _isGrowing = true;
                break;
            
            default:
                Debug.LogWarning("Unexpected ScaleMode value: " + _scaleMode + ". Defaulting to Grow.");
                _isGrowing = true;
                break;
            
        }//end switch (_scaleMode)
        
    }//end CheckScaleMode()
    
    /// <summary>
    /// A custom method example.
    /// </summary>
    private void CheckEndBehavior()
    {
        switch (_endBehavior)
        {
            case EndBehavior.Stop:
                Debug.Log("Stop");
                break;
            
            case EndBehavior.Loop:
                _isGrowing = false;
                break;
            
            case EndBehavior.Reset:
                _isGrowing = true;
                break;
            
            default:
                Debug.LogWarning("Unexpected ScaleMode value: " + _scaleMode + ". Defaulting to Grow.");
                _isGrowing = true;
                break;
            
        }//end switch (_scaleMode)
        
    }//end CheckScaleMode()
    
    
    
 
    /// <summary>
    /// A custom method example.
    /// </summary>
    /// <param name="exampleParameter"> A parameter that demonstrates passing data to the method.
    ///</param>
    private void Shrink(int exampleParameter)
    {
        
        
    }//end CustomMethod(int)
    
    /// <summary>
    /// A custom method example.
    /// </summary>
    /// <param name="exampleParameter"> A parameter that demonstrates passing data to the method.
    ///</param>
    private void Grow(int exampleParameter)
    {
        transform.localScale = Vector3.Lerp(transform.localScale, _targetMaxScale, _speed * Time.deltaTime);

        CheckEndBehavior();

    }//end Grow
 
 
}//end ShrinkGrow
