using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    //[SerializeField] PlayerControls playerControls;
    [SerializeField] XRRayInteractor controllerRayInteractor;
    public bool GameIsPaused = false;
    private Dictionary<string,float> referenceRayInteractorValues = new Dictionary<string,float>();
    private PlayerInput playerInput = null;

    public void DoEscapeMenu()
    {
        if (playerInput == null)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players)
            {
                if (player.GetComponent<PlayerInput>().enabled)
                {
                    playerInput = player.GetComponent<PlayerInput>();
                    break;
                }
            }
        }

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
        playerInput.DeactivateInput();
        playerInput.SwitchCurrentActionMap("Player");
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);

        if(XRSettings.enabled)
        {
            controllerRayInteractor.lineType = XRRayInteractor.LineType.ProjectileCurve;
            controllerRayInteractor.referenceFrame = null;
            controllerRayInteractor.velocity = referenceRayInteractorValues["Velocity"];
            controllerRayInteractor.acceleration = referenceRayInteractorValues["Acceleration"];
            controllerRayInteractor.additionalGroundHeight = referenceRayInteractorValues["AdditionalGroundHeight"];
            controllerRayInteractor.additionalFlightTime = referenceRayInteractorValues["AdditionalFlightTime"];
        }
        GameIsPaused = false;
        Debug.Log("Game resumed");
    }

    void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        playerInput.DeactivateInput();
        playerInput.SwitchCurrentActionMap("UI");
        pauseMenuUI.SetActive(true);
        
        if (XRSettings.enabled)
        {
            referenceRayInteractorValues["Velocity"] = controllerRayInteractor.velocity;
            referenceRayInteractorValues["Acceleration"] = controllerRayInteractor.acceleration;
            referenceRayInteractorValues["AdditionalGroundHeight"] = controllerRayInteractor.additionalGroundHeight;
            referenceRayInteractorValues["AdditionalFlightTime"] = controllerRayInteractor.additionalFlightTime;
            controllerRayInteractor.lineType = XRRayInteractor.LineType.StraightLine;
        }
        GameIsPaused = true;
        Debug.Log("Game paused");
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
