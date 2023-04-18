using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum ControlStatus
{
    available = 0, unavailable = 1, UI = 2
}

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CustomInput))]
public class PlayerController : Singleton<PlayerController>
{
    #region 기본 설정
    
    [Header("시네머신 카메라")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("카메라 회전 속도를 가속합니다.")]
    public float cameraRotationSpeed = 1.0f;

    [Tooltip("카메라 각도 최상단을 cameraTopClamp 도로 설정합니다.")]
    public float cameraTopClamp = 70.0f;

    [Tooltip("카메라 각도 최하단을 cameraBottomClamp 도로 설정합니다.")]
    public float cameraBottomClamp = -30.0f;

    [Tooltip("카메라를 고정합니다.")]
    public bool cameraPositionLock = false;

    public bool isCurrentDeviceMouse = true;

    // 일단 붙여놓고 나중에 체크
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float cameraAngleOverride = 0.0f;

    #endregion

    private ControlStatus controlStatus;

    // 내부 변수
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    private float threshold = 0.01f;

    private bool isControl = true; // 플레이어 컨트롤 가능 여부

    //private Tool[] playerTools;
    //private Tool currentPlayerTool;

    public CustomInput input;
    private Animator animator;
    private PlayerMovement move;
    private PlayerInteraction interact;
    private GameObject MainCamera;
    private FOVSystem fov;

    // ★테스트용
    public Text currentTool; // 강교수님 보여드리려고 만든 텍스트

    protected override void Awake2()
    {
        if (MainCamera == null)
            MainCamera = Camera.main.gameObject;

    }

    void Start()
    {
        cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        animator = GetComponentInChildren<Animator>();
        input = GetComponent<CustomInput>();
        move = GetComponent<PlayerMovement>();
        interact = GetComponent<PlayerInteraction>();
        fov = GetComponent<FOVSystem>();

        input.RegisterInteractStarted(Interact);
        input.RegisterChangeToolStarted(ChangeTool);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;

        //currentTool.text = "현재 도구 : " + interactions.ToString();
    }

    private void Update()
    {

    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    //public void Jump(InputAction.CallbackContext context)
    //{
        
    //}

    public void ChangeTool(InputAction.CallbackContext context)
    {
        //    if ((int)interactions >= 4)
        //    {
        //        interactions = 0;
        //    }
        //    else
        //        interactions++;

        //    currentTool.text = "현재 도구 : " + interactions.ToString();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (isControl == false) return;

    }

    public void InteractWith(IInteractable interactable)
    {

    }

    public void ChangeTools()
    {
        if (isControl == false) return;
    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (input.look.sqrMagnitude >= threshold && !cameraPositionLock)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = isCurrentDeviceMouse ? cameraRotationSpeed : Time.deltaTime;

            cinemachineTargetYaw += input.look.x * deltaTimeMultiplier;
            cinemachineTargetPitch += -input.look.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, cameraBottomClamp, cameraTopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch,
            cinemachineTargetYaw, 0.0f);
    }

    

    private void SnapToObject()
    {

    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}