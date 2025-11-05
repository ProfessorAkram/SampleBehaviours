/************************************************************
* COPYRIGHT:  2025
* PROJECT: Sandbox
* FILE NAME: Player.cs
* DESCRIPTION: Behaviors of Player
*                   
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2025/10/31 | Akram Taghavi-Burris | Created class
*
*
************************************************************/

using System;
using UnityEngine;
 

public class Player : MonoBehaviour
{
   private Health _health;
   private ColorFlash _colorFlash;
   
   private void OnEnable()
   {
      //Subscribe to Heath
      if (_health != null)
         _health.OnDied += HandleDeath;
      
   }//end OnEnable()

   private void OnDisable()
   {
      //Unsubscribe to Health
      if (_health != null)
         _health.OnDied -= HandleDeath;
      
   }//end OnDisable()

   private void Awake()
   {
      //Check and assign Health component
      if (!TryGetComponent<Health>(out _health))
      {
         Debug.LogError($"{gameObject.name} does not have a Health component");
      }//end if(Health)

      //Check and assign ColorFlash component
      if (!TryGetComponent<ColorFlash>(out _colorFlash))
      {
         Debug.LogError($"{gameObject.name} does not have a ColorFlash component");
      }
      
   }//end Awake()

   private void OnCollisionEnter(Collision other)
   {
      if (other.gameObject.CompareTag("Hazard"))
      {
         //Destroy the other game object
         Destroy(other.gameObject);
         
         //Take damage on health
         _health.TakeDamage(10);
         
         //Flash damage color
         _colorFlash.FlashColor();
         
      }//end if ("Hazard")
      
   }//end OnCollision Enter
   
   private void HandleDeath()
   {
      Debug.LogWarning("Player has died!");
      
      // Tell GameManager to change state
      GameManager.Instance?.ChangeGameState(GameState.GameOver);
      
   }//end HandelDeath()

   
}//end Player
