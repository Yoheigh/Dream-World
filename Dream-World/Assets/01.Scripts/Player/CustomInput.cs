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

    public bool zoomIn;
    public bool zoomOut;

    public bool cursorLocked = true;
    public bool cursorInputForLook = true;
    private bool moveInputFlag = true;
    private bool lookInputFlag = true;
    private bool interactInputFlag = true;

    public PlayerInputActions playerInputActions;

    public void Setup()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.UI.Enable();

        playerInputActions.Player.Jump.performed += OpenInventory;
        playerInputActions.UI.Cancel.performed += CloseUI;

        //playerInputActions.Player.CameraZoomIn.performed += HandleCameraScroll;
        //playerInputActions.Player.CameraZoomOut.performed += HandleCameraScroll;

        Debug.Log($"5. Setup - {this}");
    }

    private void Update()
    {
        UpdateMoveInput();
        UpdateLookInput();
    }

    private void UpdateMoveInput()
    {
        switch (moveInputFlag)
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

    private void UpdateLookInput()
    {
        switch (lookInputFlag)
        {
            case true:
                look = playerInputActions.Player.Look.ReadValue<Vector2>();
                zoomIn = playerInputActions.Player.CameraZoomIn.IsPressed();
                zoomOut = playerInputActions.Player.CameraZoomOut.IsPressed();
                break;

            case false:
                look = Vector2.zero;
                zoomIn = false;
                zoomOut = false;
                break;
        }
    }

    public void CanMove(bool _flag)
    {
        moveInputFlag = _flag;

        switch (moveInputFlag)
        {
            case true:
                playerInputActions.Player.Move.Enable();
                playerInputActions.Player.Jump.Enable();
                playerInputActions.Player.Sprint.Enable();
                break;

            case false:
                playerInputActions.Player.Move.Disable();
                playerInputActions.Player.Jump.Disable();
                playerInputActions.Player.Sprint.Disable();
                break;
        }
    }

    public void CanLook(bool _flag)
    {
        lookInputFlag = _flag;

        switch (lookInputFlag)
        {
            case true:
                playerInputActions.Player.Look.Enable();
                playerInputActions.Player.CameraZoomIn.Enable();
                playerInputActions.Player.CameraZoomOut.Enable();
                break;

            case false:
                playerInputActions.Player.Look.Disable();
                playerInputActions.Player.CameraZoomIn.Enable();
                playerInputActions.Player.CameraZoomOut.Enable();
                break;
        }
    }

    public void CanInteract(bool _flag)
    {
        interactInputFlag = _flag;

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

    // 아니 결국 이렇게 다시 매핑할거면... InputSystem을 쓴 이유가... 없지 않나...?
    //public void HandleCameraScroll(InputAction.CallbackContext context)
    //{
    //    Manager.Instance.Camera.HandleCameraScroll(zoomIn, zoomOut);
    //}

    public void CloseUI(InputAction.CallbackContext callback)
    {
        Manager.Instance.UI.Close();
    }
    public void OpenInventory(InputAction.CallbackContext callback)
    {
        // 메인 인벤토리 인덱스가 0임
        Manager.Instance.UI.ShowPanel(0);
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