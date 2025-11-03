/************************************************************
* COPYRIGHT:  2025
* PROJECT: Sandbox
* FILE NAME: GameState.cs
* DESCRIPTION:  Defines the different states of the game used to 
*              manage transitions between gameplay phases.
*
* USAGE: This enumeration can be referenced by game management scripts 
*        (e.g., GameManager) to track and control the current state of 
*        the game.
* 
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2025/10/22 | Akram Taghavi-Burris | Created class
*
*
************************************************************/
 
using UnityEngine;
 

public enum GameState
{
    BootStrap,  // Initial boot state
    MainMenu,   // Main menu screen
    GamePlay,   // Active gameplay
    GameOver    // Game over screen
}
