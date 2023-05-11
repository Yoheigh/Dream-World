using System;
using System.Reflection;
using System.Text;
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
#if ENABLE_INPUT_SYSTEM
        playerInputActions = new PlayerInputActions();
        playerInputActions.ActionMap.Enable();
#endif
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
        Debug.Log($"Interact - started : {actionFunc.GetMethodInfo()} 등록됨");
    }

    public void RegisterInteractPerformed
        (Action<InputAction.CallbackContext> actionFunc)
    {
        playerInputActions.ActionMap.Interact.performed += actionFunc;
        Debug.Log($"Interact - performed : {actionFunc.GetMethodInfo()} 등록됨");
    }

    public void RegisterInteractCanceled
        (Action<InputAction.CallbackContext> actionFunc)
    {
        playerInputActions.ActionMap.Interact.canceled += actionFunc;
        Debug.Log($"Interact - canceled : {actionFunc.GetMethodInfo()} 등록됨");
    }

    public void RegisterChangeToolStarted
    (Action<InputAction.CallbackContext> actionFunc)
    {
        playerInputActions.ActionMap.ChangeTool.started += actionFunc;
        Debug.Log($"ChangeTool - started : {actionFunc.GetMethodInfo()} 등록됨");
    }

    public void RegisterChangeToolPerformed
    (Action<InputAction.CallbackContext> actionFunc)
    {
        playerInputActions.ActionMap.ChangeTool.performed += actionFunc;
        Debug.Log($"ChangeTool - performed : {actionFunc.GetMethodInfo()} 등록됨");
    }

    public void RegisterChangeToolCanceled
    (Action<InputAction.CallbackContext> actionFunc)
    {
        playerInputActions.ActionMap.ChangeTool.canceled += actionFunc;
        Debug.Log($"ChangeTool - canceled : {actionFunc.GetMethodInfo()} 등록됨");
    }

    public void RegisterDoActionStarted
(Action<InputAction.CallbackContext> actionFunc)
    {
        playerInputActions.ActionMap.DoAction.started += actionFunc;
        Debug.Log($"DoAction - started : {actionFunc.GetMethodInfo()} 등록됨");
    }

}