using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    #region Player Components
    [Header("Player Components")]
    [SerializeField] Transform playerBody;
    [SerializeField] CharacterController characterController;
    [SerializeField] Camera playerCamera;
    #endregion
    #region Player Settings
    [Header("Player Settings")]
    [SerializeField] float speed = 2.5f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 2.5f;
    #endregion
    #region Ground Settings
    [Header("Ground Settings")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;
    #endregion
    #region Other Components
    [Header("Other Components")]
    [SerializeField] PauseMenu pauseMenu;
    public OTSControls otsControls;
    #endregion
    bool isGrounded;
    Vector3 velocity;
    float xRotation = 0f;

    private void Awake() 
    {
        otsControls = new OTSControls();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable() 
    {
        otsControls.Player.Movement.performed += DoMove;
        otsControls.Player.Jump.performed += DoJump;
        otsControls.UI.EscapeMenu.performed += pauseMenu.DoEscapeMenu;

        otsControls.Enable();
    }

    public OTSControls GetControls()
    {
        return this.otsControls;
    }

    private void DoMove(InputAction.CallbackContext obj)
    {
        Debug.Log("Movement Values: " + otsControls.Player.Movement.ReadValue<Vector2>());
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (isGrounded) 
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void OnDisable() 
    {
        otsControls.Disable();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f;
        }

        // Player look
        float mouseX = otsControls.Player.Look.ReadValue<Vector2>().x;
        float mouseY = otsControls.Player.Look.ReadValue<Vector2>().y;
        
        // xRotation and mouseY are not naming mistakes, camera up/down is the X axis, but on the mouse it is Y
        // use -= because positive on camera X is "down", so invert the value to get the expected behavior of "up"
        xRotation -= mouseY * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX * Time.deltaTime);

        // Player move (z and y are not mistakes, same as above)
        float moveX = otsControls.Player.Movement.ReadValue<Vector2>().x;
        float moveZ = otsControls.Player.Movement.ReadValue<Vector2>().y;

        // transform is local to the attached unity object (PlayerCharacter object)
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * speed * Time.deltaTime);

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity *Time.deltaTime); // Gravity squared
    }
}
