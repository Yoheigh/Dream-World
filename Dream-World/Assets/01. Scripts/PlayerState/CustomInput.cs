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

    public void RegisterToStarted
        (Action<InputAction.CallbackContext> actionFunc)
    {
        playerInputActions.ActionMap.Move.started += actionFunc;
    }

    public void RegisterToPerformed
        (Action<InputAction.CallbackContext> actionFunc)
    {
        playerInputActions.ActionMap.Move.performed += actionFunc;
    }

    public void RegisterToCanceled
        (Action<InputAction.CallbackContext> actionFunc)
    {
        playerInputActions.ActionMap.Move.canceled += actionFunc;
    }
}