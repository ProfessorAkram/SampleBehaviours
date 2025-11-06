/************************************************************
* COPYRIGHT:  2025
* PROJECT: Sandbox
* FILE NAME: GameManager.cs
* DESCRIPTION: The GameManager class is derived from the Singleton pattern
*              to ensure there is only one instance of it in the game.
*
* 
* REVISION HISTORY:
* Date [YYYY/MM/DD] | Author | Comments
* ------------------------------------------------------------
* 2025/10/22 | Akram Taghavi-Burris | Created class
*
*
************************************************************/
 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager: Singleton<GameManager>
{
    
    // Reference to the current state of the game
    public GameState CurrentState { get; private set; } = GameState.BootStrap;
    
    // Read-only property that returns true if the game is currently paused
    public bool IsPaused => Time.timeScale == 0;

    [Header("GAME STATES")] 
    [SerializeField] 
    [Tooltip("Set the game START state")]
    private GameState _startingGameState;
    
    [Header("INPUT")]
    [SerializeField]
    [Tooltip("Reference to the Input Action Asset used to handle pause and other input.")]
    private InputActionAsset _inputActions;

    [Header("SCENE MANAGEMENT")]
    
    [SerializeField]
    [Tooltip("The main menu scene that loads when the game starts.")]
    private string _mainMenuScene;
   
    [SerializeField]
    [Tooltip("The HUD overlay that appears during gameplay.")]
    private string _hudScene;
   
    [SerializeField]
    [Tooltip("The pause menu overlay that appears when the game is paused.")]
    private string _pauseMenuScene;
   
    [SerializeField]
    [Tooltip("The Game Over scene that loads when the player loses or finishes the game.")]
    private string _gameOverScene;
   
    [SerializeField]
    [Tooltip("All the level scenes in the game, in the order they should be played.")]
    private List<string> _gameLevels = new List<string>();
   
    // Index of the currently active level in the levelScenes list
    private int _currentLevelIndex = 0;
   
    // Tracks the currently loaded primary scene (menu or level)
    private string _currentScene;
   
    //List of all loaded scenes
    private List<string> _loadedScenes = new List<string>();
    

    
    private void OnEnable()
    {
        // Enable the Player action map so input events can be triggered
        _inputActions.FindActionMap("Player").Enable();

        // Subscribe to the Pause action event
        _inputActions.FindAction("Pause").performed += OnPausePressed;

    }//end OnEnable()

    private void OnDisable()
    {
        // Unsubscribe from the event
        _inputActions.FindAction("Pause").performed -= OnPausePressed;

        // Disable the Player action map to stop listening
        _inputActions.FindActionMap("Player").Disable();

    }//end OnDisable()
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Set the initial game state to Main Menu
        ChangeGameState(_startingGameState);
        
    }//end Start()
    
    /// <summary>
    /// Executes the logic associated with the current game state.
    /// This method is called whenever ChangeGameState() updates the state.
    /// </summary>
    private void ManageGameState()
    {
        // Unload all previously loaded scenes
       UnloadAllScenes();
        
        switch (CurrentState)
        {
            case GameState.MainMenu:
                Debug.Log("Game State: MainMenu");
                LoadScene(_mainMenuScene);
                break;

            case GameState.GamePlay:
                Debug.Log("Game State: GamePlay");
                
                //Load game level
                LoadScene(_gameLevels[_currentLevelIndex]);
   
                // Load the HUD as an overlay without setting it as the current scene
                LoadScene(_hudScene, false); 
                break;

            case GameState.GameOver:
                Debug.Log("Game State: GameOver");
                LoadScene(_gameOverScene);
                break;

            default:
                Debug.LogError($"[GameManager] Unknown GameState: {CurrentState}. No scenes loaded.");
                break;

        }//end switch(CurrentState)

    }//end ManageGameState()
    
    /// <summary>
    /// Changes the current game state and triggers corresponding logic
    /// only if the new state is different from the current state.
    /// </summary>
    /// <param name="newState">The new game state to switch to.</param>
    public void ChangeGameState(GameState newState)
    {
        // Early exit if already in this state
        if (newState == CurrentState)
            return;
   
        // Update the current state and manage scenes
        CurrentState = newState;

        // Short delay before managing game state
        Invoke("ManageGameState", 1f);
        
    }//end ChangeGameState
    
    /// <summary>
    /// Loads a scene additively and tracks it in the _loadedScenes list.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    /// <param name="setAsCurrent">
    /// If true, sets this scene as the current primary scene (e.g., main level or menu).
    /// If false, the scene is treated as an overlay (e.g., HUD, pause menu) and does not become the current scene.
    /// </param>
    private void LoadScene(string sceneName, bool setAsCurrent = true)
    {
        // Load the scene additively so existing scenes are preserved
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    
        // Track the loaded scene
        _loadedScenes.Add(sceneName);

        // Optionally set as current scene
        if (setAsCurrent)
        {
            _currentScene = sceneName;
        }

    } // end LoadScene()
    
    /// <summary>
    /// Unloads a single scene and removes it from the _loadedScenes list if present.
    /// </summary>
    /// <param name="sceneName">The name of the scene to unload.</param>
    private void UnloadScene(string sceneName)
    {
        //Reference to "this" scene being passed
        Scene thisScene = SceneManager.GetSceneByName(sceneName);

        // Checks if "this" scene is loaded and unloads
        if (thisScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);

        }//end if (scene.isLoaded)
        
        // Safely remove scene from list if it exists
        if (_loadedScenes.Contains(sceneName))
        {
            _loadedScenes.Remove(sceneName);
        
        }//end if(_loadedScenes.Contains(sceneName))
    
    }//end UnloadScene()
    
    /// <summary>
    /// Unloads all currently loaded scenes except persistent ones
    /// and clears the _loadedScenes list.
    /// </summary>
    private void UnloadAllScenes()
    {
        foreach (string sceneName in new List<string>(_loadedScenes))
        {
            // Unload each scene safely
            UnloadScene(sceneName);
        }

        // Clear the list to remove any remaining references
        _loadedScenes.Clear();
    
    }//end UnloadAllScenes()
    
    /// <summary>
    /// Loads the next level in the levelScenes list while keeping the player in the GamePlay state.
    /// Unloads the current level, updates the current level index, and tracks the newly loaded scene.
    /// </summary>
    public void LoadNextLevel()
    {
        // Unload the current level scene if one is loaded
        if (_currentScene != null && _loadedScenes.Contains(_currentScene))
        {
            UnloadScene(_currentScene);

        }//end if(_currentScene)

        // Increment level index
        _currentLevelIndex++;

        // If no more levels, reset index, switch to GameOver, and exit
        if (_currentLevelIndex >= _gameLevels.Count)
        {
            _currentLevelIndex = 0;
            ChangeGameState(GameState.GameOver);
            return;

        }//end

        // Load the next level and set it as the current scene
        LoadScene(_gameLevels[_currentLevelIndex]);

        Debug.Log($"Loaded Level: {_gameLevels[_currentLevelIndex]}");

    }//end LoadNextLevel()
    
    /// <summary>
    /// Called when the Pause input action is performed.
    /// Connects the input event to the GameManager's pause logic.
    /// </summary>
    /// <param name="context">Input callback info (not used here).</param>
    private void OnPausePressed(InputAction.CallbackContext context)
    {
        // Toggle the game's pause state
        TogglePause();
        
    }//end OnPausePressed()
    
    
    /// <summary>
    /// Toggles the pause state of the game.
    /// Freezes or resumes gameplay using Time.timeScale,
    /// and loads/unloads the PauseMenu scene.
    /// </summary>
    public void TogglePause()
    {
        // Only allow pausing/unpausing during gameplay
        if (CurrentState != GameState.GamePlay && !IsPaused)
            return;

        //If the game is not paused
        if (!IsPaused)
        {
            // Pause the game
            Time.timeScale = 0f;
            LoadScene(_pauseMenuScene);
        }
        else
        {
            // Resume the game
            Time.timeScale = 1f;
            UnloadScene(_pauseMenuScene);

        }//end if (!IsPaused)

    }//end TogglePause()

    
}//end GameManager
