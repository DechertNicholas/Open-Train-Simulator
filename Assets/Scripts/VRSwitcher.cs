using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class VRSwitcher : MonoBehaviour
{
    [SerializeField] GameObject leftController;
    [SerializeField] GameObject rightController;
    [SerializeField] Canvas menuCanvas = null;
    private void Awake()
    {
        StartCoroutine(StartXRCoroutine());
    }

    // private void Start()
    // {
    //     if (XRSettings.enabled)
    //     {
    //         StartCoroutine(SetXRCanvasCamera());
    //     }
    // }

    // This function checks out startup arguments to see if we want VR
    // To do this, create a desktop shortcut and add the arg at the end.
    // Example: "C:\Path\To\Game.exe" --enable-vr
    private static bool GetArg(string name)
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            Debug.Log($"Arg {i}: {args[i]}");
            if (args[i] == name)
            {
                return true;
            }
        }
        return false;
    }
 
    // From unity docs
    // https://docs.unity3d.com/Packages/com.unity.xr.management@4.0/manual/EndUser.html
    public IEnumerator StartXRCoroutine()
    {
        var enableXRArg = "--enable-vr";
        var inEditor = "-useHub";
        Debug.Log("Checking for unity editor environment");
        if (GetArg(inEditor))
        {
            Debug.Log("Looks like we're in the editor");
            if (XRGeneralSettings.Instance.Manager.activeLoader == null)
            {
                Debug.Log("XRGeneralSettings.Instance.Manager.activeLoader is null, disabling XR components");
                DisableVRComponents();
            }
        }
        else
        {
            // Only run the code block when we want VR
            Debug.Log("Looking if XR should enable");
            if (GetArg(enableXRArg)) 
            {
                yield return EnableXR();
            } 
            else 
            {
                Debug.Log("Did not find XR arg, starting in 2D");
                DisableVRComponents();
            }
        }
    }

    IEnumerator EnableXR()
    {
        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
        }
        else
        {
            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
    }

    void DisableVRComponents()
    {
        // Remove VR components from the game, as they won't be used
        //Debug.Log("Disabling VR Controllers");
        // leftController.SetActive(false);
        // rightController.SetActive(false);

        Debug.Log("Configuring menu canvas for 2D");
        if (menuCanvas == null)
        {
            Debug.Log("menuCanvas is null, trying to find in world...");
            menuCanvas = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<Canvas>();
        }
        menuCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }

    IEnumerator SetXRCanvasCamera()
    {
        Debug.Log("Setting XR Canvas camera");
        yield return new WaitForEndOfFrame();
        var playerCameras = GameObject.FindGameObjectsWithTag("PlayerCamera");
        // need to set the WorldCamera
        foreach (var camera in playerCameras)
        {
            // for multiplayer, multiple cameras may be present
            // only one should be enabled
            if (camera.activeSelf)
            {
                Debug.Log("Camera found and set");
                menuCanvas.worldCamera = camera.GetComponent<Camera>();
                Debug.Log("Camera is " + menuCanvas.worldCamera);
                break;
            }
        }
    }
}
