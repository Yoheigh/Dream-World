using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CustomInput))]
public class PlayerController : PlayerMovement
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

    [Tooltip("카메라와 플레이어 간의 최소 거리를 정합니다.")]
    public float cameraOffsetMin = 5.0f;

    [Tooltip("카메라와 플레이어 간의 최대 거리를 정합니다.")]
    public float cameraOffsetMax = 15.0f;

    [Tooltip("카메라와 플레이어 간의 거리를 조절하는 감도를 정합니다.")]
    public float cameraScrollSensivity = 0.2f;

    [Tooltip("카메라를 고정합니다.")]
    public bool cameraPositionLock = false;

    public bool isCurrentDeviceMouse = true;

    // 일단 붙여놓고 나중에 체크
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float cameraAngleOverride = 0.0f;

    #endregion

    // 내부 변수
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    private float threshold = 0.01f;

    private bool isControl = true; // 플레이어 컨트롤 가능 여부

    private PlayerInteraction interact;
    private FOVSystem fov;

    void Awake()
    {
        if (MainCamera == null)
            MainCamera = Camera.main.gameObject;
    }

    void Start()
    {

        cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        interact = GetComponent<PlayerInteraction>();
        fov = GetComponent<FOVSystem>();

        input.RegisterInteractStarted(Interact);
        input.RegisterChangeToolStarted(ChangeTool);
        input.RegisterDoActionStarted(DoAction);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        base.Setup();

    }

    private void Update()
    {
        base.MovementExecute();
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
        interact.equipmentIndex++;

        if (interact.equipmentIndex >= 3)
        {
            interact.equipmentIndex = 0;
        }
            
        Debug.Log(interact.equipmentIndex);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        interact.Interact();

    }

    public void DoAction(InputAction.CallbackContext context)
    {
        interact.InteractWithEquipment();
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

    private void CameraZoom()
    {
        //if (input.scroll != 0.0f)
        //{
        //    //float distance = Vector3.Distance(MainCamera.transform.position, CinemachineCameraTarget.transform.position);
        //    //Vector3 direction = MainCamera.transform.forward;
        //    //Vector3 movement = direction * input.scroll * cameraScrollSensivity;
        //    //if (distance - movement.z < cameraOffsetMin)
        //    //{
        //    //    movement = direction * (distance - cameraOffsetMin);
        //    //}
        //    //else if (distance - movement.z > cameraOffsetMax)
        //    //{
        //    //    movement = direction * (distance - cameraOffsetMax);
        //    //}
        //    //MainCamera.transform.position += movement;
        //}
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}