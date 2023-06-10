using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public StageData stageData;

    private sbyte hpCount = 3;

    #region ---- ī�޶� �⺻ ���� ----

    [Header("�ó׸ӽ� ī�޶�")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("ī�޶� ȸ�� �ӵ��� �����մϴ�.")]
    public float cameraRotationSpeed = 1.0f;

    [Tooltip("ī�޶� ���� �ֻ���� cameraTopClamp ���� �����մϴ�.")]
    public float cameraTopClamp = 70.0f;

    [Tooltip("ī�޶� ���� ���ϴ��� cameraBottomClamp ���� �����մϴ�.")]
    public float cameraBottomClamp = -30.0f;

    [Tooltip("ī�޶�� �÷��̾� ���� �ּ� �Ÿ��� ���մϴ�.")]
    public float cameraOffsetMin = 5.0f;

    [Tooltip("ī�޶�� �÷��̾� ���� �ִ� �Ÿ��� ���մϴ�.")]
    public float cameraOffsetMax = 15.0f;

    [Tooltip("ī�޶�� �÷��̾� ���� �Ÿ��� �����ϴ� ������ ���մϴ�.")]
    public float cameraScrollSensivity = 0.2f;

    [Tooltip("ī�޶� �����մϴ�.")]
    public bool cameraPositionLock = false;

    public bool isCurrentDeviceMouse = true;

    // �ϴ� �ٿ����� ���߿� üũ
    //[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    //public float cameraAngleOverride = 0.0f;

    #endregion

    #region ---- ���� ����----
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;
    private float threshold = 0.01f;

    private bool isControl = true; // �÷��̾� ��Ʈ�� ���� ����

    #endregion

    void Setup()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
