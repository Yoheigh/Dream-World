using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Constructure
{
    [SerializeField]
    private GameObject LadderModel;

    [SerializeField]
    private int maxConstHeight = 3;

    [SerializeField]
    private int baseConstHeight = 1;

    [SerializeField]
    private int ConstHeight = 1;

    [SerializeField]
    private float baseReachHeight = 1.1f;

    // 얘 프로퍼티로 바꿔줄 예정
    public float ReachHeight = 1.1f;

    private void Start()
    {
        CheckConstHeight();
    }

    private void CheckConstHeight()
    {
        if (ConstHeight == baseConstHeight) return;

        for(int i = baseConstHeight; i < ConstHeight; i++)
        {
            Instantiate(LadderModel, transform.position + (Vector3.up * i), transform.rotation, this.transform);
        }

        ReachHeight = ConstHeight + 0.1f;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Pivot, 0.1f);
    }
}
