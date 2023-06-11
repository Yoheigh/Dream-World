using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CustomInput), typeof(FlagSystem), typeof(CameraSystem))]
public class Manager : MonoBehaviour
{
    private Manager () { }
    public static Manager Instance { get; private set; }

    public StageManager Stage = new StageManager();
    public SoundManager Sound = new SoundManager();
    public DataManager Data = new DataManager();

    public InventoryV2 Inventory = new InventoryV2();

    public CraftSystem Craft = new CraftSystem();
    public BuildSystem Build = new BuildSystem();

    // MonoBehaviour 달린 것들
    public CustomInput Input;
    public CameraSystem Camera;
    public FlagSystem Flag;

    private void Awake()
    {
        #region 싱글톤
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            transform.parent = null;
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        // 인벤토리 초기화
        Inventory.Init();

        // 인풋 시스템 등록
        Input = GetComponent<CustomInput>();
        Input.Setup();

        // 카메라 시스템 등록
        Camera = GetComponent<CameraSystem>();
        Camera.Setup();

        // Flag 시스템 등록
        Flag = GetComponent<FlagSystem>();
        Flag.Setup();
    }

    private void LateUpdate()
    {
        Camera.HandleCameraRotation();
        Camera.HandleCameraScroll(Input.zoomIn, Input.zoomOut);

        if (UnityEngine.Input.GetKeyDown(KeyCode.L)) Flag.ExcuteFlag(0);
    }
}
