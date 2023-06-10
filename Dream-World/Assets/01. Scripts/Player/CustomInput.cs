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
    private bool interactInputFlag = true;

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
        switch (_flag)
        {
            case true:
                move = playerInputActions.Player.Move.ReadValue<Vector2>();
                jump = playerInputActions.Player.Jump.triggered;               // 누른 순간
                sprint = playerInputActions.Player.Sprint.IsPressed();         // 누르고 있는 동안
                playerInputActions.Player.Enable();
                break;

            case false:
                move = Vector2.zero;
                jump = false;
                sprint = false;
                playerInputActions.Player.Disable();
                break;
        }
    }

    private void UpdateLookInput(bool _flag)
    {
        switch (_flag)
        {
            case true:
                look = playerInputActions.Player.Look.ReadValue<Vector2>();
                break;

            case false:
                look = Vector2.zero;
                break;
        }
    }

    private void UpdateInteractInput()
    {
        switch (interactInputFlag)
        {
            case true:
                playerInputActions.Player.Interact.Enable();
                playerInputActions.Player.InteractWithEquipment.Enable();
                break;

            case false:
                playerInputActions.Player.Interact.Disable();
                playerInputActions.Player.InteractWithEquipment.Disable();
                break;
        }
    }

    public void CanMove(bool _flag)
    {
        moveInputFlag = _flag;
    }

    public void CanLook(bool _flag)
    {
        lookInputFlag = _flag;
    }

    public void CanInteract(bool _flag)
    {
        interactInputFlag = _flag;

        UpdateInteractInput();
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