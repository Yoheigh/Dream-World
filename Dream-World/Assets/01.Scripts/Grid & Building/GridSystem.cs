using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridSystem : Singleton<GridSystem>
{
    private GridV2 stageGrid;
    public GridV2 StageGrid { private set { } get { return stageGrid; } }

    public LayerMask gridObjectLayer;

    public int StageWidth = 100;
    public int StageHeight = 50;

    protected override void Awake2()
    {
        
    }

    public void Setup()
    {
        stageGrid = new GridV2(StageWidth, StageHeight);
        gridObjectLayer = LayerMask.GetMask("Block");
        CheckStageBlocks();
    }

    private void Start()
    {
        //GenerateStageBlocks();
    }

    //모든 블럭 게임 오브젝트 설치 요청
    void GenerateStageBlocks()
    {
        for (int x = 0; x < stageGrid.gridX; x++)
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
    public void CheckStageBlocks()
    {
        for (int x = 0; x < stageGrid.gridX; x++)
        {
            for (int y = 0; y < stageGrid.gridY; y++)
            {
                for (int z = 0; z < stageGrid.gridZ; z++)
                {
                    CheckStageBlock(x, y, z);
                }
            }
        }
        Debug.Log("그리드 체크 완료");
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

    public void CheckStageBlock(int x, int y, int z)
    {
        Collider[] collider = Physics.OverlapBox(new Vector3(x + 0.5f, y, z + 0.5f), new Vector3(0.49f, 0.49f, 0.49f), Quaternion.identity, gridObjectLayer);

        if (collider.Length <= 0)
        {
            InStageGrid(x, y, z, null);
            return;
        }

        else if(collider.Length > 1)
        {
            Debug.LogError($"한 좌표에 두 개 이상의 그리드 오브젝트 검출됨 {x}, {y} ,{z}");
            return;
        }

        else
        {
            InStageGrid(x, y, z, collider[0].gameObject);
            Debug.Log($"그리드 오브젝트 등록 {x}, {y} ,{z}, {collider[0].gameObject}");
        }
    }

    //public void SaveGrid()
    //{
    //    string json = JsonUtility.ToJson(stageGrid.GridObjects[0, 0, 0]);
    //    //string json = JsonHelper.ToJson<GridObject>(stageGrid.GridObjects[0,0,0]);

    //    string fileName = "Grid";
    //    string path = Application.dataPath + "/" + fileName + ".json";

    //    System.IO.File.WriteAllText(path, json);
    //}
    //public TextAsset jsonAsset;
    //public void LoadGrid()
    //{
    //    stageGrid.GridObjects[0,0,0] = JsonUtility.FromJson<GridObject>(jsonAsset.text);
    //}

    public void InStageGrid(int x, int y, int z, GameObject gridObejct)
    {
        if(!gridObejct)
        {
            StageGrid.InGrid(x, y, z, new Air());
            return;
        }

        switch(gridObejct.name)
        {
            case "RedCube":
                StageGrid.InGrid(x, y, z, new RedCube());
                break;

            default:
                StageGrid.InGrid(x, y, z, new Block(1));
                break;
        }

        StageGrid.GetGridObject(x, y, z).SetGameObject(gridObejct);
        
    }
}
