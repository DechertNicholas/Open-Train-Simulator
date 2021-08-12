using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] PlayerControls playerControls;
    public static bool GameIsPaused = false;

    public void DoEscapeMenu(InputAction.CallbackContext obj)
    {
        Debug.Log("Escape pressed");
        if (GameIsPaused)
        {
            ResumeGame();
        }
        else 
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerControls.playerInputActions.Player.Enable();
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        Debug.Log("ResumeGame finished");
    }

    void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        playerControls.playerInputActions.Player.Disable();
        Debug.Log("Player Controls State: " + playerControls.playerInputActions.Player.enabled);
        Debug.Log("UI Controls State: " + playerControls.playerInputActions.UI.enabled);
        pauseMenuUI.SetActive(true);
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        ResumeGame();
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
