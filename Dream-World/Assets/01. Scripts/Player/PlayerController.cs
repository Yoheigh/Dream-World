using Cinemachine;
using PlayerOwnedStates;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public enum PlayerStateType : sbyte
{
    Default = 0,
    Falling,
    Dragging,
    Climbing,
    Interaction,
    Damaged,
    Cinematic
}

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region ---- 카메라 기본 설정 ----

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
    //[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    //public float cameraAngleOverride = 0.0f;

    #endregion

    #region ---- 내부 변수----
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;
    private float threshold = 0.01f;

    private bool isControl = true; // 플레이어 컨트롤 가능 여부

    #endregion

    public PlayerInteraction interaction;
    public PlayerMovement movement;
    public PlayerAnimation anim;

    public FOVSystem fov;
    public CustomInput input;
    public GameObject MainCamera;
    public Animator animator;

    private StateMachine<PlayerController> PlayerFSM;
    private State<PlayerController>[] PlayerStates;

    private void Awake()
    {
        this.SetUp();
        movement.Setup();
        interaction.Setup();
        anim.Setup(animator);
        input.Setup();

        // 인풋에 함수 등록
        input.RegisterInteractPerformed(Interact);
        input.RegisterInteractWithEquipmentPerformed(InteractWithEquipment);
    }

    private void Start()
    {
        cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        PlayerFSM.Execute();
    }

    private void LateUpdate()
    {
        HandleCameraRotation();
    }

    private void SetUp()
    {
        // 변수 등록
        if (MainCamera == null)
            MainCamera = Camera.main.gameObject;

        movement = GetComponent<PlayerMovement>();
        interaction = GetComponent<PlayerInteraction>();
        anim = new PlayerAnimation();

        // input은 매니저에서 담당하게 할지 고민중
        input = FindObjectOfType<CustomInput>().GetComponent<CustomInput>();
        animator = GetComponentInChildren<Animator>();
        fov = GetComponent<FOVSystem>();

        // FSM 셋업
        PlayerStates = new State<PlayerController>[Enum.GetValues(typeof(PlayerStateType)).Length];
        PlayerStates[(int)PlayerStateType.Default] = new DefaultState();
        PlayerStates[(int)PlayerStateType.Falling] = new FallingState();
        PlayerStates[(int)PlayerStateType.Dragging] = new DraggingState();
        PlayerStates[(int)PlayerStateType.Climbing] = new ClimbingState();
        PlayerStates[(int)PlayerStateType.Interaction] = new InteractionState();
        PlayerStates[(int)PlayerStateType.Damaged] = new DamagedState();
        PlayerStates[(int)PlayerStateType.Cinematic] = new CinematicState();

        PlayerFSM = new StateMachine<PlayerController>();
        PlayerFSM.Setup(this, PlayerStates[(int)PlayerStateType.Default]);

        Debug.Log($"1. Setup - {this}");
    }

    public void Hit()
    {
        StartCoroutine(DamageCoroutine());

    }

    private IEnumerator DamageCoroutine()
    {
        ChangeState(PlayerStateType.Damaged);
        Debug.Log("아야");

        var temp = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        var waitTime = new WaitForSeconds(0.05f);

        anim.ChangeAnimationState("Damage");

        temp.enabled = false;
        yield return waitTime;
        temp.enabled = true;
        yield return waitTime;
        temp.enabled = false;
        yield return waitTime;
        temp.enabled = true;
        yield return waitTime;
        temp.enabled = false;
        yield return waitTime;
        temp.enabled = true;
        yield return waitTime;

        anim.ChangeAnimationState("Default");

        Debug.Log("이제 아프지 않아요오");
        ChangeState(PlayerStateType.Default);

        // 체력 감소 이벤트
        // 체력 감소 UI 이벤트
    }

    public void ChangeState(PlayerStateType _type)
    {
        PlayerFSM.ChangeState(PlayerStates[(int)_type]);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        interaction.Interact();
    }

    public void InteractWithEquipment(InputAction.CallbackContext context)
    {
        interaction.InteractWithEquipment();
    }

    public void HandleCameraScroll(InputAction.CallbackContext context)
    {

    }

    public void HandleCameraRotation()
    {
        if (input.look.sqrMagnitude >= threshold && !cameraPositionLock)
        {
            float deltaTimeMultiplier = isCurrentDeviceMouse ? cameraRotationSpeed : Time.deltaTime;

            cinemachineTargetYaw += input.look.x * deltaTimeMultiplier;
            cinemachineTargetPitch += -input.look.y * deltaTimeMultiplier;
        }

        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, cameraBottomClamp, cameraTopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch,
        cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}