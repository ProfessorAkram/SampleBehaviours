/************************************************************
* COPYRIGHT:  Year
* PROJECT: Name of Project or Assignment
* FILE NAME: Hazard.cs
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

public abstract class Hazard : MonoBehaviour
{
    [Header("Base Hazard Settings")]
    [SerializeField] protected bool _hasLifeTime = true;
    [SerializeField] protected float _lifetime = 5f;
    [SerializeField] protected GameObject _effectPrefab;
    [SerializeField] protected AudioClip _soundEffect;

    protected float lifeTimer;
    protected bool isActive;
    public GameObject Owner { get; set; }

    protected virtual void OnEnable()
    {
        lifeTimer = lifetime;
        isActive = true;
    }

    protected virtual void Update()
    {
        if (!isActive) return;

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Deactivate();
        }
    }

    protected virtual void Deactivate()
    {
        isActive = false;
        gameObject.SetActive(false);
    }

  
}