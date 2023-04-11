using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

enum Interactions
{
    Hand,
    Place,
    Axe,
    Pickaxe,
    Shovel
}

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

    // 애니메이션 값
    public int animIDSpeed;
    public float animationBlend;
    public int animIDMotionSpeed;

    // 프로토타입용 애니메이션 State 변수
    private const string PLAYER_IDLE = "Player_Idle";
    private const string PLAYER_WALK = "Player_Walk";
    private const string PLAYER_SPRINT = "Player_Sprint";
    private const string PLAYER_ACTION_SHOVEL = "Player_Action_Shovel";
    private const string PLAYER_ACTION_PICKAXE = "Player_Action_Pickaxe";
    private const string PLAYER_ACTION_AXE = "Player_Action_Axe";
    private const string PLAYER_ACTION_PICKUP = "Player_Action_PickUp";
    private const string PLAYER_ACTION_PLACE = "Player_Action_Place";
    private const string PLAYER_ACTION_THROW = "Player_Action_Throw";
    private string animCurrentState = null;

    private bool isControl = true; // 플레이어 컨트롤 가능 여부
    private Interactions interactions = Interactions.Hand;

    private Animator animator;
    private CharacterController characterController;
    private CustomInput input;
    private GameObject MainCamera;
    private FOVSystem fov;

    // ★강교수님 보여드리려고 만든 텍스트
    public Text currentTool;

    void Awake()
    {
        if (MainCamera == null)
            MainCamera = Camera.main.gameObject;
    }


    void Start()
    {
        cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        // _hasAnimator = TryGetComponent(out animator);
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        input = GetComponent<CustomInput>();
        fov = GetComponent<FOVSystem>();

        input.RegisterInteractStarted(Interact);
        input.RegisterChangeToolStarted(ChangeTool);

        currentTool.text = "현재 도구 : " + interactions.ToString();

        AssignAnimationIDs();
    }

    private void Update()
    {
        SimpleMove();
        //Gravity();

        //if (isControl == false) return;

        //if (Keyboard.current.xKey.isPressed)
        //    StartCoroutine(AnimationDelay(PLAYER_ACTION_PICKUP));

        //if (Keyboard.current.cKey.isPressed)
        //    StartCoroutine(AnimationDelay(PLAYER_ACTION_PLACE));

        //if (Keyboard.current.eKey.isPressed)
        //    StartCoroutine(AnimationDelay(PLAYER_ACTION_AXE));

        //if (Keyboard.current.rKey.isPressed)
        //    StartCoroutine(AnimationDelay(PLAYER_ACTION_PICKAXE));

        //if (Keyboard.current.tKey.isPressed)
        //    StartCoroutine(AnimationDelay(PLAYER_ACTION_SHOVEL));
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void AssignAnimationIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        //animIDGrounded = Animator.StringToHash("Grounded");
        //animIDJump = Animator.StringToHash("Jump");
        //animIDFreeFall = Animator.StringToHash("FreeFall");
        animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    public void Jump(InputAction.CallbackContext context)
    {

    }

    public void ChangeTool(InputAction.CallbackContext context)
    {
        if ((int)interactions >= 4)
        {
            interactions = 0;
        }
        else
            interactions++;

        currentTool.text = "현재 도구 : " + interactions.ToString();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (isControl == false) return;
        // fov.GetClosestTarget().GetComponent<IInteractable>().Interact();
        switch (interactions)
        {
            case Interactions.Hand:
                StartCoroutine(AnimationDelay(PLAYER_ACTION_PICKUP, 1.2f));
                break;
            case Interactions.Place:
                StartCoroutine(AnimationDelay(PLAYER_ACTION_PLACE, 1.7f));
                break;
            case Interactions.Axe:
                StartCoroutine(AnimationDelay(PLAYER_ACTION_AXE, 0.8f));
                break;
            case Interactions.Pickaxe:
                StartCoroutine(AnimationDelay(PLAYER_ACTION_PICKAXE, 0.8f));
                break;
            case Interactions.Shovel:
                StartCoroutine(AnimationDelay(PLAYER_ACTION_SHOVEL, 1.5f));
                break;
        }
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
            float deltaTimeMultiplier = isCurrentDeviceMouse ? 1.0f : Time.deltaTime;

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

    // 만들다 말음
    private void Gravity()
    {
        if (verticalVelocity < terminalVelocity)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void Move()
    {
        float targetSpeed = input.sprint ? sprintSpeed : moveSpeed;
        
        if (input.move == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(characterController.velocity.x, 0.0f, characterController.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = input.move.magnitude;

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
            Time.deltaTime * speedChangeRate);

            //round speed to 3 decimal places
            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = targetSpeed;
        }

        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime);
        if (animationBlend < 0.01f) animationBlend = 0f;

        Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y);

        if (input.move != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              MainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                rotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        characterController.Move(targetDirection.normalized * (speed * Time.deltaTime) +
                         new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

        //if (_hasAnimator)
        //{
        animator.SetFloat(animIDSpeed, animationBlend);
        animator.SetFloat(animIDMotionSpeed, inputMagnitude);
        //}
    }

    void SimpleMove()
    {
        if (isControl == false) return;

        float targetSpeed = input.sprint ? sprintSpeed : moveSpeed;
        if (input.move == Vector2.zero)
        {
            targetSpeed = 0.0f;
            //ChangeAnimationState(PLAYER_IDLE);
        }

        Vector3 moveDir = new Vector3(input.move.x, 0, input.move.y);
        float inputMagnitude = input.move.magnitude;

        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0f;

        if (input.move != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg +
                              MainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                    rotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            //ChangeAnimationState(PLAYER_WALK);
        }
        
        transform.position += targetDirection * targetSpeed * Time.deltaTime;
        //transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * 20f);

        animator.SetFloat(animIDSpeed, animationBlend);
        animator.SetFloat(animIDMotionSpeed, inputMagnitude);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void ChangeAnimationState(string animState)
    {
        if (animCurrentState == animState)
            return;

        animator.Play(animState);
        animCurrentState = animState;
    }

    private IEnumerator AnimationDelay(string animState, float delay)
    {
        isControl = false;
        ChangeAnimationState(animState);

        // float temp = animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(delay);
        // 현재 재생중인 애니메이션의 길이 만큼 대기...인데 급해서 좀;
        animCurrentState = null;
        isControl = true;
    }
}