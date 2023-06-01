using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridSystem : Singleton<GridSystem>
{    
    private GridV2 stageGrid;
    public GridV2 StageGrid { private set { } get { return stageGrid; } }

    public int StageWidth = 100;
    public int StageHeight = 30;

    protected override void Awake2()
    {
        stageGrid = new GridV2(StageWidth, StageHeight);
    }

    private void Start()
    {
        GenerateStageBlocks();
    }

    //모든 블럭 게임 오브젝트 설치 요청
    void GenerateStageBlocks()
    {
        for(int x = 0; x < stageGrid.gridX; x++) 
        {
            for (int y = 0; y < stageGrid.gridY; y++)
            {
                for (int z = 0; z < stageGrid.gridZ; z++)
                {
                   GenerateStageBlock(x, y, z);
                }
            }
        }
    }

    //블럭 게임 오브젝트 설치 후 그리드 오브젝트 객체에서 게임 오브젝트 관리 
    public void GenerateStageBlock(int x, int y, int z)
    {
        if (stageGrid.GetGridObject(x, y, z).gridObjectData.blockName == "Air")
            return;
        GameObject block = Instantiate(Resources.Load<GameObject>(stageGrid.GetGridObject(x, y, z).gridObjectData.blockPrefabPath));
        block.transform.position = new Vector3(x + 0.5f, y + 0.5f, z + 0.5f);
        block.transform.rotation = Quaternion.identity;
        block.name = stageGrid.GetGridObject(x, y, z).gridObjectData.blockName;
        stageGrid.GetGridObject(x, y, z).SetGameObject(block);
    }

    //게임 오브젝트 삭제, 추후 확장 예정
    public void DestoryStageBlock(GameObject gameObject_)
    {
        Destroy(gameObject_);
    }

    public bool CheckCanCraft(int x, int y, int z)
    {
        Collider[] colliders = Physics.OverlapBox(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), new Vector3(0.49f, 0.49f, 0.49f));

        if (colliders.Length > 0)
            return false;

        return true;
    }
}
