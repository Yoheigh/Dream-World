using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : Singleton<Manager>
{
    public StageManager Stage = new StageManager();
    public SoundManager Sound = new SoundManager();
    public DataManager Data = new DataManager();

    public CraftSystem Craft = new CraftSystem();
    public BuildSystem Build = new BuildSystem();
    public FlagSystem Flag = new FlagSystem();

    public InventoryV2 Inventory = new InventoryV2();

    protected override void Awake2()
    {
        // 인벤토리 초기화
        Inventory.Init();
    }
}
