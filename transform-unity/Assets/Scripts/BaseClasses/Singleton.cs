/************************************************************
* COPYRIGHT:  2025
* PROJECT: Sandbox 
* FILE NAME: Singleton.cs
* DESCRIPTION: Provides a generic base class for creating singleton MonoBehaviour components.
* 
* USAGE: Inherit from this class to make a MonoBehaviour a singleton.
* 
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2025/10/22 | Akram Taghavi-Burris | Created class
*
*
************************************************************/
 

using UnityEngine; 


public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Static instance that holds the reference to the Singleton
    public static T Instance {get; private set;}

    // Unity's Awake method, called when the script instance is being loaded
    private void Awake()
    {
        // Check for singleton duplication
        CheckForSingleton();

    }//end Awake()

    // Ensures that only one instance of the Singleton exists
    void CheckForSingleton()
    {
        // If no instance exists, assign this instance
        if (Instance == null)
        {
            Instance = this as T;
        } 
        // If an instance already exists and it's not this one, destroy the new instance to maintain the Singleton
        else{
            // Ensure that only the original Singleton instance persists
            Destroy(gameObject); 
        }
               
        // Log the current instance for debugging purposes
        Debug.Log(Instance);

    }//end CheckForSingleton()


}//end Singleton