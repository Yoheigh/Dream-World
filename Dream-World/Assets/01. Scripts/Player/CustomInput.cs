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
    private bool moveInputFlag = true;
    private bool lookInputFlag = true;

    private PlayerInputActions playerInputActions;

    public void Setup()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        Debug.Log($"5. Setup - {this}");
    }

    private void Update()
    {
        UpdateMoveInput(moveInputFlag);
        UpdateLookInput(lookInputFlag);
    }

    private void UpdateMoveInput(bool _flag)
    {
        switch(_flag)
        {
            case true:
                move = playerInputActions.Player.Move.ReadValue<Vector2>();
                jump = playerInputActions.Player.Jump.triggered;               // 누른 순간
                sprint = playerInputActions.Player.Sprint.IsPressed();         // 누르고 있는 동안
                break;

            case false:
                move = Vector2.zero;
                jump = false;
                sprint = false;
                break;
        }
    }

    private void UpdateLookInput(bool _flag)
    {
        if( lookInputFlag )
            look = playerInputActions.Player.Look.ReadValue<Vector2>();

        else
            look = Vector2.zero;
    }

    public void CanMove(bool _flag)
    {
        moveInputFlag = _flag;
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