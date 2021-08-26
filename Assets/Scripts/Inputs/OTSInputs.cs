using UnityEngine;
using UnityEngine.InputSystem;

public class OTSInputs : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    [Header("Menu References")]
    public PauseMenu pauseMenu = null;

    [Header("Component References")]
    public PlayerInput playerInput = null;

    //private OTSControls otsControls;

    // private void Awake() 
    // {
    //     otsControls = new OTSControls();
    // }

    private void Start()
    {
        if (pauseMenu == null)
        {
            pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<PauseMenu>();
        }

        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }
    }

    // private void OnEnable()
    // {
    //     Debug.Log("OTSInputs enabled, setting callbacks");
    //     otsControls.Player.Look.performed += OnLook;
    //     otsControls.Player.Movement.performed += OnMove;
    //     otsControls.Player.Sprint.performed += OnSprint;
    //     otsControls.Player.Jump.performed += OnJump;
    //     otsControls.UI.EscapeMenu.performed += OnEscape;
    // }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        Debug.Log("OnMove called!");
        MoveInput(ctx.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        if(cursorInputForLook)
        {
            LookInput(ctx.ReadValue<Vector2>());
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        JumpInput(ctx.ReadValueAsButton());
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        SprintInput(ctx.ReadValueAsButton());
    }

    public void OnEscape(InputAction.CallbackContext ctx)
    {
        Debug.Log("OnEscape called");
        if (ctx.performed)
        {
            pauseMenu.DoEscapeMenu();
        }
    }

    /*
    ---- /\ Unity Events | State Change \/ ----
    */

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    } 

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }

    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }
}
