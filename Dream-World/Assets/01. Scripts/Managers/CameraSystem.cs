using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CameraSystem : MonoBehaviour
{
    CustomInput Input => Manager.Instance.Input;

    public GameObject MainCamera;
    public PlayerController controller;
    public GameObject playerRoot;


    #region ---- ī�޶� �⺻ ���� ----

    [Header("�ó׸ӽ� ī�޶�")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("ī�޶� ȸ�� �ӵ��� �����մϴ�.")]
    public float cameraRotationSpeed = 1.0f;

    [Tooltip("ī�޶� ���� �ֻ���� cameraTopClamp ���� �����մϴ�.")]
    public float cameraTopClamp = 70.0f;

    [Tooltip("ī�޶� ���� ���ϴ��� cameraBottomClamp ���� �����մϴ�.")]
    public float cameraBottomClamp = 10.0f;

    [Tooltip("ī�޶�� �÷��̾� ���� �ּ� �Ÿ��� ���մϴ�.")]
    public float cameraOffsetMin = 5.0f;

    [Tooltip("ī�޶�� �÷��̾� ���� �ִ� �Ÿ��� ���մϴ�.")]
    public float cameraOffsetMax = 15.0f;

    [Tooltip("ī�޶�� �÷��̾� ���� �Ÿ��� �����ϴ� ������ ���մϴ�.")]
    public float cameraScrollSensivity = 0.2f;

    [Tooltip("ī�޶� �������� �����մϴ�.")]
    public bool cameraPositionLock = false;

    [Tooltip("ī�޶� ȸ���� �����մϴ�.")]
    public bool cameraRotationLock = false;

    [Tooltip("�÷��̾��� �������� ���󰩴ϴ�.")]
    public bool isFollowPlayer = true;

    public bool isCurrentDeviceMouse = false;

    // public bool isCurrentDeviceMouse = true;

    // �ϴ� �ٿ����� ���߿� üũ
    //[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    //public float cameraAngleOverride = 0.0f;

    #endregion

    #region ---- ���� ����----
    CinemachineVirtualCamera cinemachine;
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;
    private float threshold = 0.01f;

    #endregion

    public void Setup()
    {
        if (MainCamera == null)
            MainCamera = Camera.main.gameObject;

        cinemachine = FindObjectOfType<CinemachineVirtualCamera>().GetComponent<CinemachineVirtualCamera>();
        controller = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        
        CinemachineCameraTarget = controller.transform.GetChild(0).gameObject;
        playerRoot = CinemachineCameraTarget;
        cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }

    public void HandleCameraRotation()
    {
        if (isFollowPlayer == true)
        {
            if (Input.look.sqrMagnitude >= threshold && !cameraPositionLock)
            {
                float deltaTimeMultiplier = isCurrentDeviceMouse ? cameraRotationSpeed : Time.deltaTime;

                cinemachineTargetYaw += Input.look.x * deltaTimeMultiplier;
                cinemachineTargetPitch += -Input.look.y * deltaTimeMultiplier;
            }
            cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, cameraBottomClamp, cameraTopClamp);

            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch,
            cinemachineTargetYaw, 0.0f);
        }
        else
            return;
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void HandleCameraMove(Vector3 newPos, Quaternion newRot, float lerpTime = 2f)
    {
        StartCoroutine(CameraMoveCoroutine(newPos, newRot, lerpTime));
    }

    public void HandleCameraMove(Transform newTransform, float lerpTime = 2f)
    {
        StartCoroutine(CameraMoveCoroutine(newTransform.position, newTransform.rotation, lerpTime));
    }

    public void HandleCameraTarget(GameObject newTarget)
    {
        CinemachineCameraTarget = newTarget;
        cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        cinemachine.Follow = newTarget.transform;
        cinemachine.LookAt = newTarget.transform;
    }

    private IEnumerator CameraMoveCoroutine(Vector3 _newPos, Quaternion _newRot, float lerpTime)
    {
        float currentTime = 0f;
        float t;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= lerpTime)
                currentTime = lerpTime;

            var newPos = Vector3.Lerp(MainCamera.transform.position, _newPos, currentTime);
            var newRot = Quaternion.Lerp(MainCamera.transform.rotation, _newRot, currentTime);

            MainCamera.transform.SetPositionAndRotation(newPos, newRot);


            yield return null;
        }
    }

}
