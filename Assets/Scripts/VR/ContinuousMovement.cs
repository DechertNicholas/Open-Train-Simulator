using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] XRNode inputSource;
    private float additionalHeight = 0.2f;
    [SerializeField] XRRig rig;
    //[SerializeField] Transform characterVisuals;
    private Vector2 inputAxis;
    private CharacterController character;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponentInParent<CharacterController>();
        //rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    void FixedUpdate()
    {
        CapsuleFollowHeadset();

        // Head move
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);

        character.Move(direction * Time.fixedDeltaTime * speed);

        // Gravity
        // fallingSpeed += gravity * Time.fixedDeltaTime;
        // character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    void CapsuleFollowHeadset()
    {
        character.height = rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }
}
