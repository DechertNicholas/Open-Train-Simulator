using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] PlayerControls playerControls;
    [SerializeField] XRRayInteractor controllerRayInteractor;
    public static bool GameIsPaused = false;
    Dictionary<string,float> referenceRayInteractorValues = new Dictionary<string,float>();

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
        playerControls.otsControls.Player.Enable();
        pauseMenuUI.SetActive(false);
        controllerRayInteractor.lineType = XRRayInteractor.LineType.ProjectileCurve;
        controllerRayInteractor.referenceFrame = null;
        controllerRayInteractor.velocity = referenceRayInteractorValues["Velocity"];
        controllerRayInteractor.acceleration = referenceRayInteractorValues["Acceleration"];
        controllerRayInteractor.additionalGroundHeight = referenceRayInteractorValues["AdditionalGroundHeight"];
        controllerRayInteractor.additionalFlightTime = referenceRayInteractorValues["AdditionalFlightTime"];
        GameIsPaused = false;
        Debug.Log("ResumeGame finished");
    }

    void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        playerControls.otsControls.Player.Disable();
        Debug.Log("Player Controls State: " + playerControls.otsControls.Player.enabled);
        Debug.Log("UI Controls State: " + playerControls.otsControls.UI.enabled);
        pauseMenuUI.SetActive(true);
        
        referenceRayInteractorValues["Velocity"] = controllerRayInteractor.velocity;
        referenceRayInteractorValues["Acceleration"] = controllerRayInteractor.acceleration;
        referenceRayInteractorValues["AdditionalGroundHeight"] = controllerRayInteractor.additionalGroundHeight;
        referenceRayInteractorValues["AdditionalFlightTime"] = controllerRayInteractor.additionalFlightTime;
        
        controllerRayInteractor.lineType = XRRayInteractor.LineType.StraightLine;
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
