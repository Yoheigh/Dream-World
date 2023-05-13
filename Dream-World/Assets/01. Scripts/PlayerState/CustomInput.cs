using System;
using System.Reflection;
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
        playerInputActions.Player.Enable();
    }

    private void Update()
    {
        move = playerInputActions.Player.Move.ReadValue<Vector2>();
        look = playerInputActions.Player.Look.ReadValue<Vector2>();
        jump = playerInputActions.Player.Jump.triggered;         // 누른 순간
        sprint = playerInputActions.Player.Sprint.IsPressed();   // 누르고 있는 동안
    }

    public void RegisterInteractStarted(Action<InputAction.CallbackContext> actionFunc)
    {
        playerInputActions.Player.Interact.started += actionFunc;
        Debug.Log($"Interact - started : {actionFunc.GetMethodInfo()} 등록됨");
    }

    public void RegisterInteractPerformed(Action<InputAction.CallbackContext> actionFunc)
    {
        playerInputActions.Player.Interact.performed += actionFunc;
        Debug.Log($"Interact - performed : {actionFunc.GetMethodInfo()} 등록됨");
    }

    public void RegisterInteractCanceled(Action<InputAction.CallbackContext> actionFunc)
    {
        playerInputActions.Player.Interact.canceled += actionFunc;
        Debug.Log($"Interact - canceled : {actionFunc.GetMethodInfo()} 등록됨");
    }

    public void RegisterInteractWithEquipmentPerformed(Action<InputAction.CallbackContext> actionFunc)
    {
        playerInputActions.Player.InteractWithEquipment.performed += actionFunc;
        Debug.Log($"DoAction - started : {actionFunc.GetMethodInfo()} 등록됨");
    }

}