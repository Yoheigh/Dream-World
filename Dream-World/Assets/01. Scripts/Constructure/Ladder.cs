using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Constructure
{
    [SerializeField]
    private float maxConstHeight = 3f;

    [SerializeField]
    private float ConstHeight = 1f;

    public float ReachHeight = 1f;

    //private void Start()
    //{
        
    //}

    private int CheckConstHeight()
    {
        int temp = 1;
        return temp;
    }    


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Pivot, 0.1f);
    }
}
