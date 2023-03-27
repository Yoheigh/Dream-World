using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;

    private Rigidbody rigidbody;
    public Camera playerCamera;

    private Vector3 moveVector;
    private Vector3 forceDirection;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
        PlayerController.playerControl += PlayerMove;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void PlayerMove(InputAction playerInput)
    {
        moveVector = playerInput.ReadValue<Vector2>();
        forceDirection += moveVector.x * GetCameraRight(playerCamera);
        forceDirection += moveVector.y * GetCameraForward(playerCamera);

        rigidbody.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;
        Debug.Log("현재 플레이어의 인풋값 : " + moveVector);
    }

    private void LookAt(InputAction playerInput)
    {
        Vector3 direction = rigidbody.velocity;
        direction.y = 0f;

        if (moveVector.sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            rigidbody.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rigidbody.angularVelocity = Vector3.zero;
    }
}