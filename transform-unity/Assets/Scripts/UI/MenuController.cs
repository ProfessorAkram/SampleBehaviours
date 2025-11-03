/************************************************************
* COPYRIGHT:  Year
* PROJECT: Name of Project or Assignment
* FILE NAME: MainMenuController.cs
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
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    private VisualElement _ui;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _resumeButton;
    
    private GameManager _gameManager;

    private void Awake()
    {
        _ui = GetComponent<UIDocument>().rootVisualElement;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        TryBindButton("Play_Btn", OnPlayClicked);
        TryBindButton("Exit_Btn", OnExitClicked);
        TryBindButton("Resume_Btn", OnResumeClicked);

    }
    
    private void OnDisable()
    {
        // Always good practice to unbind to avoid memory leaks when reloading UI
        UnbindButton(_playButton, OnPlayClicked);
        UnbindButton(_exitButton, OnExitClicked);
        UnbindButton(_resumeButton, OnResumeClicked);
    }
    
    /// <summary>
    /// Tries to find a button and safely attach a click callback if it exists.
    /// </summary>
    private void TryBindButton(string buttonName, System.Action callback)
    {
        Button button = _ui.Q<Button>(buttonName);
        if (button != null)
        {
            button.clicked += callback;
        }
        else
        {
            // Optional: log for debugging during development
            Debug.Log($"[MenuUIController] Button '{buttonName}' not found in this menu.");
        }
    }
    
    
    private void UnbindButton(Button button, System.Action callback)
    {
        if (button != null)
            button.clicked -= callback;
    }
    

    private void OnPlayClicked()
    {
        _gameManager.ChangeGameState(GameState.GamePlay);
    }

    private void OnExitClicked()
    {
        _gameManager.ChangeGameState(GameState.GameOver);
    }

    private void OnResumeClicked()
    {
        _gameManager.TogglePause();
    }
    
    
}//end MainMenuController
