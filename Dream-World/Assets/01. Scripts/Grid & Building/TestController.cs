using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            GridSystem.Instance.StageGrid.SetGridObject(10, 10, 10, new RedCube());
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GridSystem.Instance.StageGrid.SetGridObject(11, 10, 10, new BlueCube());
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Destroy(GridSystem.Instance.StageGrid.GetGridObject(0,0,0).gameObject);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log(GridSystem.Instance.StageGrid.GetGridObject(10, 10, 10).rightGridObjectIndex);
        }
    }

    private void Start()
    {
        GridSystem.Instance.StageGrid.SetGridObject(99, 29, 99, new Block(1));
        GridSystem.Instance.StageGrid.SetGridObject(0, 0, 0, new Block(1));
    }
}
