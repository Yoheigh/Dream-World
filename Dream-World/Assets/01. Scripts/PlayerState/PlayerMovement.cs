using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region 코드였던 것
    //public float moveSpeed = 1f;
    //public float rotateSpeed = 10f;

    //private Rigidbody rigidbody;
    //public Camera playerCamera;

    //private Vector3 moveVector;
    //private Vector3 forceDirection;

    //private void Start()
    //{
    //    rigidbody = GetComponent<Rigidbody>();
    //    playerCamera = Camera.main;
    //    PlayerController.playerControl += PlayerMove;
    //    PlayerController.playerControl += RotateCamera;
    //}

    //private Vector3 GetCameraRight(Camera playerCamera)
    //{
    //    Vector3 forward = playerCamera.transform.forward;
    //    forward.y = 0;
    //    return forward.normalized;
    //}

    //private Vector3 GetCameraForward(Camera playerCamera)
    //{
    //    Vector3 right = playerCamera.transform.right;
    //    right.y = 0;
    //    return right.normalized;
    //}

    //private void PlayerMove(InputAction playerInput)
    //{
    //    moveVector = playerInput.actionMap["Move"].ReadValue<Vector2>();
    //    forceDirection += moveVector.x * GetCameraRight(playerCamera);
    //    forceDirection += moveVector.y * GetCameraForward(playerCamera);

    //    rigidbody.AddForce(forceDirection, ForceMode.Impulse);
    //    forceDirection = Vector3.zero;
    //    //Debug.Log("현재 플레이어의 인풋값 : " + moveVector);
    //}

    //private void PlayerLook()
    //{
    //    Vector3 direction = rigidbody.velocity;
    //    direction.y = 0f;

    //    if (moveVector.sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
    //        rigidbody.rotation = Quaternion.LookRotation(direction, Vector3.up);
    //    else
    //        rigidbody.angularVelocity = Vector3.zero;
    //}

    //// Unity third person view camera using mouse delta position
    //void RotateCamera(InputAction playerInput)
    //{
    //    Vector2 mouseDelta = playerInput.actionMap["CameraRotation"].ReadValue<Vector2>();
    //    Vector3 rotation = new Vector3(0f, mouseDelta.x, 0f) * rotateSpeed;
    //    rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(rotation));
    //}
    #endregion
    private float moveSpeed = 1f;
    private float turnSmoothTime = 0.1f;

    InputAction playerInput;

    //3D Platformer movement
    private float turnSmoothVelocity;
    private Vector2 inputDirection;
    private Vector3 moveDirection;
    private Vector3 turnDirection;
    private float turnSpeed;
    
    //move function in fixedUpdate
    private void FixedUpdate()
    {
        Move();
    }

    //move function
    private void Move()
    {
        moveDirection = new Vector3(inputDirection.x, 0f, inputDirection.y).normalized;
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= moveSpeed * Time.deltaTime;
        transform.position += moveDirection;
        turnDirection = new Vector3(inputDirection.x, 0f, inputDirection.y).normalized;
        turnSpeed = Mathf.SmoothDampAngle(transform.eulerAngles.y, Mathf.Atan2(turnDirection.x, turnDirection.z) * Mathf.Rad2Deg, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, turnSpeed, 0f);
    }

}