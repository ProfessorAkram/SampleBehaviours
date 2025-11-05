/************************************************************
* COPYRIGHT:  Year
* PROJECT: Name of Project or Assignment
* FILE NAME: Hazards.cs
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

public interface IHazard
{
    //Amount of damage the hazard deals
    int DamageAmount { get; }
    
    // Called when the hazard is destroyed or triggered.
    void HandleDestroy()
    {
        //Do Something
    }

}//end Hazard
