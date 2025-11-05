/************************************************************
* COPYRIGHT:  Year
* PROJECT: Name of Project or Assignment
* FILE NAME: Health.cs
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


public class Health : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("Maiximum Health")]
    [Range(0,100)]
    private int maxHealth = 100;
    
    [SerializeField]
    [Tooltip("Should the object be destroyed on death?")]
    private bool _destroyOnDeath = false;
    
    private int currentHealth;
    
    //Public Properties
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    
    //Flag that returns true if the object is dead (health <= 0)
    public bool IsDead => currentHealth <= 0;

    // Events for others to subscribe to
    public event Action OnDamaged;
    public event Action OnHealed;
    public event Action OnDied;

    private void Awake()
    {
        currentHealth = maxHealth;
        
    }//end Awake()

    /// <summary>
    /// Reduces health by a specified amount and triggers events if damaged or dead.
    /// </summary>
    /// <param name="amount">The amount of damage to apply.</param>
    public void TakeDamage(int amount)
    {

        // Prevent negative health
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        
        // Notify subscribers
        OnDamaged?.Invoke();

        //If out of health die
        if (IsDead)
        {
            Die();
            
        }//end if(IsDead)
        
        Debug.Log("Damage Taken! Current Health " + CurrentHealth);
        
    }//end TakeDamage
    
    // <summary>
    /// Increases health by a specified amount, without exceeding maxHealth.
    /// </summary>
    /// <param name="amount">The amount to heal.</param>
    public void Heal(int amount)
    {
        if (IsDead) return;

        // Prevent health from exceeding max health
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
       
        // Notify subscribers
        OnHealed?.Invoke();
        
    }//end Heal

    private void Die()
    {
        OnDied?.Invoke();

        //if destroy on death, destroy object
        if (_destroyOnDeath)
        {
            Destroy(gameObject);
        }
        
    }//end Die()
    
}//end Health
