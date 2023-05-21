using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerStateType
{
    Default = 0, Falling, Dragging, Climbing, Interaction, Cinematic
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Default State 플레이어 설정")]
    [Tooltip("걸을 때, 1초에 moveSpeed 미터만큼 이동합니다.")]
    public float moveSpeed = 2.0f;

    [Tooltip("달릴 때, 1초에 sprintSpeed 미터만큼 이동합니다.")]
    public float sprintSpeed = 3.533f;

    [Tooltip("캐릭터의 회전 속도를 조절합니다. 낮을 수록 빠르게 회전합니다.")]
    [Range(0.0f, 0.3f)]
    public float rotationSmoothTime = 0.03f;

    [Tooltip("애니메이션 가속력을 정의합니다.")]
    public float speedChangeRate = 10.0f;

    [Tooltip("미끄러운 바닥에서의 마찰력을 정의합니다.")]
    public float frictionSpeed = 1.0f;

    //[Tooltip("점프할 때, jumpHeight 블럭 만큼 점프합니다.")]
    //public float jumpHeight = 1.2f;

    //[Tooltip("다시 점프할 수 있을 때까지 jumpTimeout 초 걸립니다.")]
    //public float jumpTimeout;

    [Tooltip("공중에서 추락 상태까지 fallTimeout 초 걸립니다.")]
    public float fallTimeout = 0.15f;

    [Tooltip("중력을 조절합니다. 낮을 수록 빠르게 떨어집니다.")]
    public float gravity = -10.0f;

    [Header("바닥 인식")]
    [Tooltip("바닥 인식을 활성화합니다.")]
    public bool isGround = true;

    [Tooltip("캐릭터 중심으로부터 y축으로 groundCheckOffset 만큼 멀어집니다.")]
    public float groundCheckOffset = -0.15f;

    [Tooltip("바닥을 인식할 구체의 반지름입니다.")]
    public float groundCheckRadius = 0.2f;

    [Tooltip("바닥으로 인식할 Layer입니다. (기본 : Block)")]
    public LayerMask groundLayers;

    // 내부 변수
    private float targetRotation;
    private float rotationVelocity;
    private float verticalVelocity;
    private float fallTimeoutDelta;
    private float terminalVelocityMax = 20.0f;
    private float terminalVelocityMin = -10.0f;
    private Vector3 currentPos;         // 사다리, 잡기 등의 플레이어 현재 위치값
    private Vector3 lastPos;            // 사다리, 잡기 등의 플레이어 고정 위치값
    private float maxReachPoint;        
    private float verticalSnap;
    private Vector3 slipperyDirection;  // 미끄러지는 땅에서의 방향 벡터

    // 미끄러지는 상태
    public bool isSlippery = false;
    private float slipperySpeed;

    // 바닥 인식할 구(Sphere) 시작점
    private Vector3 spherePosition;

    // 애니메이션 값
    public int animIDSpeed;
    public float animationBlend;
    public int animIDHoldMove;
    public int animIDMotionSpeed;

    private CharacterController characterController;
    private PlayerController controller;
    private CustomInput input;
    public GameObject MainCamera;
    public Animator animator;

    public void Setup()
    {
        if (MainCamera == null)
            MainCamera = Camera.main.gameObject;

        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<PlayerController>();
        input = FindObjectOfType<CustomInput>().GetComponent<CustomInput>();

        AssignAnimationIDs();
        fallTimeoutDelta = fallTimeout;
        slipperySpeed = 0.0f;

        // 레이어 마스크 제외하고 블럭으로 만들 예정인데 고민 중
        groundLayers = LayerMask.GetMask("Block");

        Debug.Log($"2. Setup - {this}");
    }

    public void InitTest()
    {
        fallTimeoutDelta = fallTimeout;
        currentPos = Vector3.zero;
        lastPos = Vector3.zero;
        Debug.Log("3 - 벡터 초기화");
    }

    public void AssignAnimationIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDHoldMove = Animator.StringToHash("Direction");
        //animIDGrounded = Animator.StringToHash("Grounded");
        //animIDJump = Animator.StringToHash("Jump");
        //animIDFreeFall = Animator.StringToHash("FreeFall");
        animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    public void GroundCheck()
    {
        spherePosition = new Vector3(transform.position.x, transform.position.y - groundCheckOffset,
            transform.position.z);

        isGround = Physics.CheckSphere(spherePosition, groundCheckRadius, groundLayers,
            QueryTriggerInteraction.Ignore);

        //if (_hasAnimator)
        //{
        //    _animator.SetBool(_animIDGrounded, Grounded);
        //}
    }

    public void Gravity()
    {
        // 바닥 감지
        GroundCheck();

        // 바닥을 감지했다면
        if (isGround)
        {
            fallTimeoutDelta = fallTimeout;

            if (verticalVelocity <= 0.0f)
                verticalVelocity = -1.0f;
        }
        else
        {
            if (verticalVelocity < terminalVelocityMin)
                verticalVelocity = terminalVelocityMin;

            if (fallTimeoutDelta >= 0.0f)
                fallTimeoutDelta -= Time.deltaTime;
            else
            {
                input.move = Vector2.zero;
                controller.ChangeState(PlayerStateType.Falling);
            }
        }

        if (verticalVelocity < terminalVelocityMax)
            verticalVelocity += gravity * Time.deltaTime;

        // 중력 적용
        characterController.Move(new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
    }

    public void Move()
    {
        float targetSpeed = input.sprint ? sprintSpeed : moveSpeed;

        if (input.move == Vector2.zero)
        {
            targetSpeed = 0.0f;

            if (slipperySpeed > 0.0f)
                slipperySpeed -= frictionSpeed * Time.deltaTime;
            else if (slipperySpeed < 0.0f)
                slipperySpeed = 0.0f;
        }
        else
        {
            if (slipperySpeed <= targetSpeed)
                slipperySpeed += targetSpeed * frictionSpeed * Time.deltaTime;
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
        }

        if (isSlippery)
            // targetDirection 값에 따라 방향이 바뀌므로 미끄러지려면 수정 필요
            characterController.Move(targetDirection * (slipperySpeed * Time.deltaTime));

        else
            characterController.Move(targetDirection * (targetSpeed * Time.deltaTime));

        //transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * 20f);
        // t_n =  (t - min)/(max - min)

        animator.SetFloat(animIDSpeed, animationBlend);
        animator.SetFloat(animIDMotionSpeed, inputMagnitude);
    }

    public void MoveHolding(Transform _DragableObject)
    {
        float targetSpeed = moveSpeed / 2;

        if (input.move == Vector2.zero)
        {
            targetSpeed = 0.0f;
        }

        // Vector3 moveDir = new Vector3(input.move.x, 0, input.move.y);
        float inputMagnitude = input.move.magnitude;

        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) *
                                    new Vector3(0.0f, 0.0f, input.move.y);

        // animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
        animationBlend = input.move.y;

        //if (animationBlend < 0.01f) animationBlend = 0f;

        //if (input.move != Vector2.zero)
        //{
        //    targetRotation = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg +
        //                      MainCamera.transform.eulerAngles.y;
        //}

        characterController.Move(targetDirection * (targetSpeed * Time.deltaTime) +
                               new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

        _DragableObject.position += targetDirection * (targetSpeed * Time.deltaTime);

        //transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * 20f);
        // t_n =  (t - min)/(max - min)

        animator.SetFloat(animIDHoldMove, animationBlend);
        animator.SetFloat(animIDMotionSpeed, inputMagnitude);

        if (Keyboard.current.fKey.isPressed) // || Gamepad.current.buttonEast.isPressed)
        {
            controller.ChangeState(PlayerStateType.Default);
        }
    }

    public void SetVerticalPoint(Vector3 _pivot, float _maxReachPoint)
    {
        lastPos = _pivot;
        currentPos = _pivot;
        maxReachPoint = _maxReachPoint;
        transform.position = _pivot;
    }

    public void SnapToVerticalPoint()
    {
        transform.position = new Vector3(lastPos.x, verticalSnap, lastPos.z);
    }

    public void MoveVertical() // Vector3 _startPos, float _maxReachPoint 이러헥 쓰고 싶었는데 ㅠㅠ
    {
        float targetSpeed = moveSpeed;

        if (input.move == Vector2.zero)
        {
            targetSpeed = 0.0f;
        }

        if (currentPos.y > lastPos.y + maxReachPoint)
            controller.ChangeState(PlayerStateType.Default);

        else if (currentPos.y <= lastPos.y - 0.3f)
            controller.ChangeState(PlayerStateType.Default);

        //Vector3 moveDir = new Vector3(input.move.x, 0, input.move.y);

        float inputMagnitude = input.move.magnitude;

        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) *
                                    new Vector3(0.0f, input.move.y, 0.0f);

        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);

        if (animationBlend < 0.01f) animationBlend = 0f;

        //if (input.move != Vector2.zero)
        //{
        //    targetRotation = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg +
        //                      MainCamera.transform.eulerAngles.y;
        //

        //characterController.Move(targetDirection * (targetSpeed * Time.deltaTime));
        transform.position += targetDirection * targetSpeed * Time.deltaTime;
        currentPos.y += input.move.y * targetSpeed * Time.deltaTime;

        //transform.position = new Vector3(lastPos.x, transform.position.y, lastPos.z);

        //transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * 20f);
        // t_n =  (t - min)/(max - min)

        animator.SetFloat(animIDSpeed, animationBlend);
        animator.SetFloat(animIDMotionSpeed, inputMagnitude);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spherePosition, groundCheckRadius);
    }
}