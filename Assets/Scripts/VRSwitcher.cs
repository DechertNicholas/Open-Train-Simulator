using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;

public class VRSwitcher : MonoBehaviour
{
    [SerializeField] GameObject leftController;
    [SerializeField] GameObject rightController;
    public void Awake()
    {
        StartCoroutine(StartXRCoroutine());
    }

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
        var enableVRArg = "--enable-vr";
        var forceVR = "-useHub";
        Debug.Log("Checking for unity editor environment");
        if (GetArg(forceVR))
        {
            Debug.Log("Looks like we're in the editor");
            if (XRGeneralSettings.Instance.Manager.activeLoader == null)
            {
                Debug.Log("XRGeneralSettings.Instance.Manager.activeLoader is null, disabling VR components");
                DisableVRComponents();
            }
        }
        else
        {
            // Only run the code block when we want VR
            Debug.Log("Looking if VR should enable");
            if (GetArg(enableVRArg)) 
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
            else 
            {
                Debug.Log("Did not find VR arg, starting in 2D");
                DisableVRComponents();
            }
        }
    }

    void DisableVRComponents()
    {
        // Remove VR components from the game, as they won't be used
        Debug.Log("Disabling VR Controllers");
        leftController.SetActive(false);
        rightController.SetActive(false);
    }
}
