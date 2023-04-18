using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField]
    public BlockData[,,] blockDatas;

    private Grid<BlockData> StageGrid;

    public int StageWidth = 100;
    public int StageHeight = 30;

    [Tooltip("웬만하면 안 건드는 게 정신건강에 좋을 겁니다")]
    public float BlockSize = 1f;
    public float offset = 0.5f;

    

    public float cellSize = 0.1f;

    void Awake()
    {
        StageGrid = new Grid<BlockData>(StageWidth, StageHeight, Vector3.zero,
                                        () => new BlockData());
    }

    private void Start()
    {
        //StageGrid.SetGridObject(1, 0, 0, ItemDatabass.instance.GetBlock(10002));
        //Debug.Log(StageGrid.GetGridObject(0, 0, 0).blockName);
        //Debug.Log(StageGrid.GetGridObject(5, 1, 3).blockName);
        //Debug.Log(StageGrid.GetGridObject(512, 175, 3).blockName);
        //Debug.Log(StageGrid.GetGridObject(1, 0, 0).blockName);

        GenerateStageBlocks();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateStageBlocks()
    {
        for(int x = 0; x < StageGrid.gridX; x++) {
            for (int y = 0; y < StageGrid.gridY; x++)
            {
                for (int z = 0; z < StageGrid.gridZ; x++)
                {
                    Debug.Log($"{x},{y},{z}");
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
