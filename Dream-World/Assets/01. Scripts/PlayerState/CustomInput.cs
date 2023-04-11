using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomInput : MonoBehaviour
{
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;

    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.ActionMap.Enable();
    }

    private void Update()
    {
        move = playerInputActions.ActionMap.Move.ReadValue<Vector2>();
        look = playerInputActions.ActionMap.Look.ReadValue<Vector2>();
        jump = playerInputActions.ActionMap.Jump.triggered;         // 누른 순간
        sprint = playerInputActions.ActionMap.Sprint.IsPressed();   // 누르고 있는 동안
    }

    public void RegisterInteractStarted
        (Action<InputAction.CallbackContext> actionFunc)
    {
        playerInputActions.ActionMap.Interact.started += actionFunc;
    }

    public void RegisterInteractPerformed
        (Action<InputAction.CallbackContext> actionFunc)
    {
        playerInputActions.ActionMap.Interact.performed += actionFunc;
    }

    public void RegisterInteractCanceled
        (Action<InputAction.CallbackContext> actionFunc)
    {
        playerInputActions.ActionMap.Interact.canceled += actionFunc;
    }

    public void RegisterChangeToolStarted
    (Action<InputAction.CallbackContext> actionFunc)
    {
        playerInputActions.ActionMap.ChangeTool.started += actionFunc;
        Debug.Log("ChangeTool에 함수 등록됨");
    }
}