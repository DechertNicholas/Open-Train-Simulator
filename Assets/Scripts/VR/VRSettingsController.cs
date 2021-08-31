using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Cinemachine;
using Mirror;

public class VRSettingsController : NetworkBehaviour
{
    [Header("XR Components")]
    public XRRig PlayerXRRig;
    public LocomotionSystem LocomotionProvider;

    [Header("XR Disabled Component Settings")]
    public Camera PlayerCamera;
    public Transform CameraParentOnVRDisable;

    [Header("XR Enabled Component Settings")]
    public Transform ObjectsParentOnVREnable;
    public List<Transform> ObjectsToReParent;
    public Transform PlayerVisuals;


    // Start is called before the first frame update
    void Start()
    {
        if (base.hasAuthority)
        {
            StartCoroutine(HandleXRComponents());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator HandleXRComponents()
    {
        yield return new WaitForSeconds(0.2f);
        if(!XRSettings.enabled)
        {
            PlayerCamera.transform.SetParent(CameraParentOnVRDisable);
            PlayerXRRig.gameObject.SetActive(false);
        }
        else
        {
            // foreach (var objectNeedingReparent in ObjectsToReParent)
            // {
            //     objectNeedingReparent.SetParent(ObjectsParentOnVREnable);
            // }
            PlayerXRRig.gameObject.SetActive(true);
            PlayerVisuals.Translate(new Vector3(0f, 1f, 0f));

            LocomotionProvider = GameObject.FindWithTag("VRTeleLoco").GetComponent<LocomotionSystem>();
            LocomotionProvider.xrRig = this.PlayerXRRig;
        }
    }
}
