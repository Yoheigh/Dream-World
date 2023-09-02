using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

[RequireComponent(typeof(CustomInput), typeof(FlagSystem), typeof(CameraSystem))]
public class Managers : MonoBehaviour
{
    static Managers s_instance;
    // public static Managers Instance { get { Init(); return s_instance; } private set { } }
    public static Managers Instance { get; set; }

    //public static void Init()
    //{
    //    if (s_instance == null)
    //    {
    //        GameObject go = GameObject.Find("@Managers");
    //        if (go == null)
    //        {
    //            go = new GameObject { name = "@Managers" };
    //            go.AddComponent<Managers>();
    //        }

    //        DontDestroyOnLoad(go);
    //        s_instance = go.GetComponent<Managers>();
    //    }
    //}

    SceneManager _stage = new SceneManager();
    SoundManager _sound = new SoundManager();
    DataManager _data = new DataManager();

    InventoryV2 _inventory = new InventoryV2();
    CraftSystem _craft = new CraftSystem();

    // MonoBehaviour 달린 것들
    public PlayerController Player;
    CustomInput _input;
    CameraSystem _camera;
    FlagSystem _flag = new FlagSystem();
    UISystemManager _ui;
    GridSystem _grid;
    BuildSystem _build;

    public static SceneManager Stage { get { return Instance?._stage; } }
    public static SoundManager Sound { get { return Instance?._sound; } }
    public static DataManager Data { get { return Instance?._data; } }

    public static InventoryV2 Inventory { get { return Instance?._inventory; } }
    public static CraftSystem Craft { get { return Instance?._craft; } }
    public static FlagSystem Flag { get { return Instance?._flag; } }
    public static CustomInput Input { get { return Instance?._input; } }
    public static BuildSystem Build { get { return Instance?._build; } }
    public static GridSystem Grid { get { return Instance?._grid; } }
    public static UISystemManager UI { get { return Instance?._ui; } }
    public static FlagSystem FLAG { get { return Instance?._flag; } }
    public static CameraSystem CAMERA { get { return Instance?._camera; } }



    private bool isGamePaused = false;

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

        // 시스템 등록
        _input = GetComponent<CustomInput>();
        Camera = GetComponent<CameraSystem>();
        UI = GetComponent<UISystemManager>();
        _flag = GetComponent<FlagSystem>();
        Grid = GetComponent<GridSystem>();
        Build = GetComponent<BuildSystem>();

        _inventory.OnChangeItem += UI.DrawItemSlots;
        _inventory.OnChangeEquipment += UI.ActivateEquipSlot;
        _inventory.OnChangeBuilding += UI.ActivateBuildSlot;

        _input.Setup();
        // Flag.Init();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        //Debug.Log("지금 씬 이름 : " + SceneManager.GetActiveScene().name);

        Flag.currentSceneIndex = 1;

        AsyncOperation op_next = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(Flag.currentSceneIndex, LoadSceneMode.Single);
        //AsyncOperation op_before = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        //op_next.allowSceneActivation = false;
        //op_before.allowSceneActivation = false;

        //if (op_next.isDone && op_before.isDone)
        //{
        //    Debug.Log("오우쒯");
        //    op_before.allowSceneActivation = true;
        //    op_next.allowSceneActivation = true;
        //    SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        //}
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Player == null)
            Player = FindObjectOfType<PlayerController>();

        // 시스템 셋업
        Camera.Setup();
        UI.Setup();
        Flag.Setup();
        Grid.Setup();
        Build.Setup();

        // 인벤토리 초기화
        _inventory.Init();
        UI.DrawItemSlots();

        Sound.PlayBGM(100);

        Time.timeScale = 1.0f;

        Debug.Log("씬 로딩됨");
    }

    void ResetGame()
    {
        Flag.currentSceneIndex = 1;

        AsyncOperation op_next = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(Flag.currentSceneIndex, LoadSceneMode.Single);
    }

    void GamePause()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused == true)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;
    }


    private void LateUpdate()
    {
        Camera.HandleCameraRotation();
        Camera.HandleCameraScroll(Input.zoomIn, Input.zoomOut);

        if (UnityEngine.Input.GetKeyDown(KeyCode.L)) Flag.ForestCutsceneStart();

        if (UnityEngine.Input.GetKeyDown(KeyCode.R)) Flag.GameOver();
        if (UnityEngine.Input.GetKeyDown(KeyCode.T)) GamePause();
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1)) _inventory.ChangeEquipment();
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2)) _inventory.ChangeBuilding();
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3)) Build.ChangeBuildMode();
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4)) Build.RotateBuilding();
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha5)) Build.Construct();
        // if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha8)) Flag.ForestCutsceneStart();
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha9)) Flag.NextSceneWithTransition();
        if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha0)) ResetGame();
    }
}
