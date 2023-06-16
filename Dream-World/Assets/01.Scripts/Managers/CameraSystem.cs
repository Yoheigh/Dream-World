using Cinemachine;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CameraSystem : MonoBehaviour
{
    CustomInput Input => Manager.Instance.Input;

    public GameObject MainCamera;
    public PlayerController controller;
    public GameObject playerRoot;

    public GameObject newPoint;

    #region ---- 카메라 기본 설정 ----

    [Header("시네머신 카메라")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("카메라 회전 속도를 가속합니다.")]
    public float cameraRotationSpeed = 1.0f;

    [Tooltip("카메라 각도 최상단을 cameraTopClamp 도로 설정합니다.")]
    public float cameraTopClamp = 70.0f;

    [Tooltip("카메라 각도 최하단을 cameraBottomClamp 도로 설정합니다.")]
    public float cameraBottomClamp = 10.0f;

    [Tooltip("카메라와 플레이어 간의 최소 거리를 정합니다.")]
    public float cameraOffsetMin = 2.0f;

    [Tooltip("카메라와 플레이어 간의 최대 거리를 정합니다.")]
    public float cameraOffsetMax = 15.0f;

    [Tooltip("카메라와 플레이어 간의 거리를 조절하는 감도를 정합니다.")]
    public float cameraScrollSensivity = 4f;

    [Tooltip("카메라 움직임을 고정합니다.")]
    public bool cameraPositionLock = false;

    [Tooltip("카메라 회전을 고정합니다.")]
    public bool cameraRotationLock = false;

    [Tooltip("플레이어의 움직임을 따라갑니다.")]
    public bool isFollowPlayer = true;

    public bool isCurrentDeviceMouse = false;

    // public bool isCurrentDeviceMouse = true;

    // 일단 붙여놓고 나중에 체크
    //[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    //public float cameraAngleOverride = 0.0f;

    #endregion

    #region ---- 내부 변수----
    CinemachineVirtualCamera cinemachine;
    Cinemachine3rdPersonFollow cam;
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;
    private float threshold = 0.01f;
    [SerializeField]
    private float tempDistance;

    #endregion

    public void Setup()
    {
        if (MainCamera == null)
            MainCamera = Camera.main.gameObject;

        cinemachine = FindObjectOfType<CinemachineVirtualCamera>().GetComponent<CinemachineVirtualCamera>();
        cam = cinemachine.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        controller = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();

        tempDistance = cam.CameraDistance;
        CinemachineCameraTarget = controller.transform.GetChild(0).gameObject;
        playerRoot = CinemachineCameraTarget;
        cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }

    public void HandleCameraRotation()
    {
        if (isFollowPlayer == true)
        {
            if (Input.look.sqrMagnitude >= threshold && !cameraRotationLock)
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

    public void HandleCameraMove(Vector3 newPos, Quaternion newRot, float lerpTime = 1f)
    {
        StartCoroutine(CameraMoveCoroutine(newPos, newRot, lerpTime));
    }

    public void HandleCameraMove(Transform newTransform, float lerpTime = 1f)
    {
        StartCoroutine(CameraMoveCoroutine(newTransform.position, newTransform.rotation, lerpTime));
    }

    public void ReturnCameraToPlayer()
    {
        HandleCameraTarget(playerRoot);
    }

    public void HandleCameraScroll(bool zoomIn, bool zoomOut)
    {
            if (zoomIn && !zoomOut)
                tempDistance -= cameraScrollSensivity * Time.unscaledDeltaTime;
            else if (!zoomIn && zoomOut)
                tempDistance += cameraScrollSensivity * Time.unscaledDeltaTime;

        if (tempDistance > cameraOffsetMax)
            tempDistance = cameraOffsetMax;
        else if (tempDistance < cameraOffsetMin)
            tempDistance = cameraOffsetMin;

        cam.CameraDistance = Mathf.Lerp(cam.CameraDistance, tempDistance, 0.2f);

    }

    public void HandleCameraTarget(GameObject newTarget)
    {
        if(newTarget == null)
        {
            cinemachine.Follow = null;
            cinemachine.LookAt = null;
            return;
        }

        CinemachineCameraTarget = newTarget;
        cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        cinemachine.Follow = newTarget.transform;
        cinemachine.LookAt = newTarget.transform;
    }

    private IEnumerator CameraMoveCoroutine(Vector3 _newPos, Quaternion _newRot, float lerpTime)
    {
        float currentTime = 0f;
        var startPosition = MainCamera.transform.position;
        var startRotation = MainCamera.transform.rotation;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / lerpTime;

            Debug.Log(t);

            var newPos = Vector3.Lerp(startPosition, _newPos, t);
            var newRot = Quaternion.Lerp(startRotation, _newRot, t);

            cinemachine.transform.SetPositionAndRotation(newPos, newRot);

            yield return null;
        }
    }

}
