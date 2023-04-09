using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CustomInput))]
public class PlayerController : MonoBehaviour
{
    [Header("플레이어 설정")]
    [Tooltip("걸을 때, 1초에 moveSpeed 미터만큼 이동합니다.")]
    public float moveSpeed = 1.0f;

    [Tooltip("달릴 때, 1초에 sprintSpeed 미터만큼 이동합니다.")]
    public float sprintSpeed = 1.533f;

    [Tooltip("캐릭터의 회전 속도를 조절합니다. 낮을 수록 빠르게 회전합니다.")]
    [Range(0.0f, 0.3f)]
    public float rotationSmoothTime = 0.12f;

    [Tooltip("가속력을 정의합니다.")]
    public float speedChangeRate = 10.0f;

    [Tooltip("점프할 때, jumpHeight 블럭 만큼 점프합니다.")]
    public float jumpHeight = 1.2f;

    [Tooltip("다시 점프할 수 있을 때까지 jumpTimeout 초 걸립니다.")]
    public float jumpTimeout;
    [Tooltip("공중에서 추락 상태까지 fallTimeout 초 걸립니다.")]
    public float FallTimeout;

    [Tooltip("중력을 조절합니다. 낮을 수록 빠르게 떨어집니다.")]
    public float gravity = -10.0f;

    [Header("바닥 인식")]
    public bool isGround = true;

    public float groundCheckOffset = -0.14f;
    public float groundCheckRadius = 0.10f;

    public LayerMask GroundLayers;

    [Header("시네머신 카메라")]
    public GameObject CinemachineCameraTarget;

    public float cameraTopClamp = 70.0f;
    public float cameraBottomClamp = -30.0f;
    public bool cameraPositionLock = false;

    public bool isCurrentDeviceMouse = true;

    // 일단 붙여놓고 나중에 체크
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float cameraAngleOverride = 0.0f;

    // 내부 변수
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    private float speed;
    private float targetRotation;
    private float rotationVelocity;
    private float verticalVelocity;
    private float terminalVelocity = 53.0f;

    private float threshold = 0.01f;

    private Animator animator;
    private CharacterController characterController;
    private CustomInput input;
    private GameObject MainCamera;

    void Awake()
    {
        if (MainCamera == null)
            MainCamera = Camera.main.gameObject;
    }


    void Start()
    {
        cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        // TryGetComponent(out animator);
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        input = GetComponent<CustomInput>();
    }

    private void Update()
    {
        Move();
        Gravity();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        
    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (input.look.sqrMagnitude >= threshold && !cameraPositionLock)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = isCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            cinemachineTargetYaw += input.look.x * deltaTimeMultiplier;
            cinemachineTargetPitch += input.look.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, cameraBottomClamp, cameraTopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch,
            cinemachineTargetYaw, 0.0f);
    }

    private void Gravity()
    {
        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void Move()
    {
        //     set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = input.sprint ? sprintSpeed : moveSpeed;
        //     a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        //     note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        //     if there is no input, set the target speed to 0
        if (input.move == Vector2.zero) targetSpeed = 0.0f;

        //     a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(characterController.velocity.x, 0.0f, characterController.velocity.z).magnitude;

        float speedOffset = 0.1f;
        // float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;
        float inputMagnitude = input.move.magnitude;

        //     accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            //         creates curved result rather than a linear one giving a more organic speed change
            //         note T in Lerp is clamped, so we don't need to clamp our speed
            speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
            Time.deltaTime * speedChangeRate);

            //round speed to 3 decimal places
            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = targetSpeed;
        }

        //    _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        //    if (_animationBlend < 0.01f) _animationBlend = 0f;

        //     normalise input direction
        Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (input.move != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              MainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                rotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        // move the player
        characterController.Move(targetDirection.normalized * (speed * Time.deltaTime) +
                         new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

        //update animator if using character
        //if (_hasAnimator)
        //{
        //    _animator.SetFloat(_animIDSpeed, _animationBlend);
        //    _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        //}
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}