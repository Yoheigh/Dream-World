using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public StageData stageData;

    private sbyte hpCount = 3;

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

    void Setup()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
