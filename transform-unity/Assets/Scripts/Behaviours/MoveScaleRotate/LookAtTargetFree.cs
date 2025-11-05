/************************************************************
 * COPYRIGHT:  2025
 * PROJECT: Sandbox
 * FILE NAME: LookAtTargetFree.cs
 * 
 * DESCRIPTION: Rotates a GameObject to face a target instantly in all directions using Transform.LookAt.
 *              Can start rotating on Start and can be stopped via Stop().
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
using UnityEngine;

public class LookAtTargetFree : MonoBehaviour
{
    // ===== Hidden Fields =====
    
    // Runtime rotation flag
    private bool _isRotating;

        
    // ===== Inspector Fields =====
    [SerializeField]
    [Tooltip("Target object to rotate towards")]
    private Transform _target;
    
    
    [SerializeField]
    [Tooltip("Enable to rotate the object on Start.")]
    private bool _rotateOnStart = true;
    
    
    // ===== Public Properties =====

    public Transform Target 
    {   get => _target; 
        set => _target = value; 
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
            LookAtTarget();

        }//end if(_isMoving)

    }//end Update()
    
    private bool IsTargetValid(Transform target)
    {
        // Use the passed values or fall back to the default inspector-assigned values
        Transform currentTarget = target ?? Target;
        
        // Set target to current target
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

        // Look at Target
        transform.LookAt(targetPosition);

    } // end RotateObject()
    
    /// <summary>
    /// Stops the object's movement by updating the movement flag.
    /// </summary>
    public void Stop()
    {
        // Flags the object as stopped
        _isRotating = false;

    }//end Stop()
 
 
}//end LookAtTargetFree
