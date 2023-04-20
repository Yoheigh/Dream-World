using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridSystem : Singleton<GridSystem>
{
    [SerializeField]
    public BlockData[,,] blockDatas;

    private Grid<BlockData> stageGrid;
    public Grid<BlockData> StageGrid { private set { } get { return stageGrid; } }

    public int StageWidth = 100;
    public int StageHeight = 30;

    [Tooltip("웬만하면 안 건드는 게 정신건강에 좋을 겁니다")]
    public float BlockSize = 1f;
    public float offset = 0.5f;

    public float cellSize = 0.1f;

    protected override void Awake2()
    {
        stageGrid = new Grid<BlockData>(StageWidth, StageHeight, Vector3.zero,
                                        () => new BlockData());
    }

    private void Start()
    {
        GenerateStageBlocks();
        StageGrid.SetGridObject(1, 1, 1, ItemDatabass.instance.GetBlock(10002));
        Debug.Log(StageGrid.GetGridObject(0, 0, 0).blockName);
        Debug.Log(StageGrid.GetGridObject(5, 1, 3).blockName);
        //Debug.Log(StageGrid.GetGridObject(512, 175, 3).blockName);
        //Debug.Log(StageGrid.GetGridObject(1, 0, 0).blockName);
    }

    void GenerateStageBlocks()
    {
        for(int x = 0; x < stageGrid.gridX; x++) 
        {
            for (int y = 0; y < stageGrid.gridY; y++)
            {
                for (int z = 0; z < stageGrid.gridZ; z++)
                {
                    StageGrid.SetGridObject(x, y, z, ItemDatabass.instance.GetBlock(10001));
                }
            }
        }
    }

    // 이 함수 에디터 사양 엄청 잡아먹음 ㅡㅡ
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    for (gridX = 0; gridX < maxGridX; gridX += gridSize)
    //    {
    //        for (gridZ = 0; gridZ < maxGridZ; gridZ += gridSize)
    //        {
    //            for (gridY = 0; gridY < maxGridY; gridY += gridSize)
    //            {
    //                Vector3 point = new Vector3(gridX + offset, gridY + offset, gridZ + offset);
    //                Gizmos.DrawSphere(point, cellSize);
    //            }
    //        }
    //    }
    //}
}
