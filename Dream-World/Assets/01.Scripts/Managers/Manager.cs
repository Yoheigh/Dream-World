using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CustomInput), typeof(FlagSystem), typeof(CameraSystem))]
public class Manager : MonoBehaviour
{
    private Manager() { }
    public static Manager Instance { get; private set; }

    public StageManager Stage = new StageManager();
    public SoundManager Sound = new SoundManager();
    public DataManager Data = new DataManager();

    public InventoryV2 Inventory = new InventoryV2();

    public CraftSystem Craft = new CraftSystem();

    // MonoBehaviour 달린 것들
    public PlayerController Player;

    public CustomInput Input;
    public CameraSystem Camera;
    public FlagSystem Flag;
    public UISystemManager UI;
    public GridSystem Grid;
    public BuildSystem Build;

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

        // 시스템 등록
        Input = GetComponent<CustomInput>();
        Camera = GetComponent<CameraSystem>();
        UI = GetComponent<UISystemManager>();
        Flag = GetComponent<FlagSystem>();
        Grid = GetComponent<GridSystem>();
        Build = GetComponent<BuildSystem>();

        // 시스템 셋업
        Input.Setup();
        Camera.Setup();
        UI.Setup();
        Flag.Setup();

        Sound.PlayBGM(100);

        Inventory.OnChangeItem += UI.DrawItemSlots;
        Inventory.OnChangeEquipment += UI.ActivateItemSlot;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Camera.Setup();
        UI.Setup();
        Grid.Setup();

        if (Player == null)
            Player = FindObjectOfType<PlayerController>();

        Time.timeScale = 1.0f;

        Debug.Log("씬 로딩됨");
    }


    private void LateUpdate()
    {
        Camera.HandleCameraRotation();
        Camera.HandleCameraScroll(Input.zoomIn, Input.zoomOut);

        if (UnityEngine.Input.GetKeyDown(KeyCode.L)) Flag.ForestCutsceneStart();

        if (UnityEngine.Input.GetKeyDown(KeyCode.T)) Flag.GameOver();
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1)) Inventory.ChangeEquipment();
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3)) Build.ChangeBuildMode();
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4)) Build.RotateBuilding();
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha5)) Build.Construct();
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha0)) Flag.NextScene();
    }
}
