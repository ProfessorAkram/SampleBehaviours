/************************************************************
* COPYRIGHT:  Year
* PROJECT: Name of Project or Assignment
* FILE NAME: ColorFlash.cs
* DESCRIPTION: Short Description of script.
*                   
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2000/01/01 | Your Name | Created class
*
*
************************************************************/
using System.Collections; 
using UnityEngine;


public class ColorFlash : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    
    [SerializeField] 
    [Tooltip("The color of the flash")]
    private Color _flashColor = Color.red;
    
    [SerializeField] 
    private float _flashDuration = 0.1f;

    //Material instance for flash color
    private Material _material;
    
    //Reference to original Color
    private Color _originalColor;

    private void Awake()
    {
        //Check and assign MeshRender component
        if (!TryGetComponent<MeshRenderer>(out _meshRenderer))
        {
            Debug.LogError($"{nameof(_meshRenderer)} is not attached");
        }


        // Use a unique instance of the material so other objects aren’t affected
        _material = _meshRenderer.material;
        
        //Record the original color
        _originalColor = _material.color;
    }

    /// <summary>
    /// Call this method to flash the object red briefly.
    /// </summary>
    public void FlashColor()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        _material.color = _flashColor;
        yield return new WaitForSeconds(_flashDuration);
        _material.color = _originalColor;
        
    }//Enumeration of Flashes
    
}//end ColorFlash
