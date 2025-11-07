/************************************************************
 * COPYRIGHT:  Year
 * PROJECT: Name of Project or Assignment
 * FILE NAME: DamageDealer.cs
 * DESCRIPTION: Short Description of script.
 *
 * REVISION HISTORY:
 * Date [YYYY/MM/DD] | Author | Comments
 * ------------------------------------------------------------
 * 2000/01/01 | Your Name | Created class
 *
 *
 ************************************************************/
 
using System;
using UnityEngine;
 

public class DamageDealer : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("The amount of damage hazard deals")]
    private int _damageAmount = 10;
    public int DamageAmount => _damageAmount;
    
    [SerializeField] 
    [Tooltip("Should this object be destroyed after dealing damage")]
    private bool _destroyOnImpact = true; 
    
    // Events for others to subscribe to
    public event Action OnDestroyed;
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        { 
            DealDamage(damageable);
            
        }//end if(IDamageable)
        
    }//end OnTriggerEnter()
    
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            DealDamage(damageable);
            
        }//end if(IDamageable)
        
    }//end OnCollisionEnter()


    private void DealDamage(IDamageable damageable)
    {
        damageable.TakeDamage(_damageAmount);
        
        // Optionally destroy this object if it doesn't have health
        if (_destroyOnImpact){}
            OnDestroyed?.Invoke();

    }//end DealDamage()
 
 
}//end DamageDealer
