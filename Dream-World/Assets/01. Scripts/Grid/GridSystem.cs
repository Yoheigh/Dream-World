using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public BlockData[,,] blockDatas;

    public float gridSize = 1f;
    public float offset = 0.5f;

    public int maxGridX = 5;
    public int maxGridY = 1;
    public int maxGridZ = 5;

    public float gridX;
    public float gridY;
    public float gridZ;

    public float cellSize = 0.1f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
