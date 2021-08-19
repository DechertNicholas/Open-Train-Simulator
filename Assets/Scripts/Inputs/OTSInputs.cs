using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public class OTSInputs : NetworkBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    public void OnMove(InputAction.CallbackContext ctx)
    {
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
