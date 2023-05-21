using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constructure : MonoBehaviour
{
    [Header("Constructure")]
    [SerializeField]
    private Vector3 pivot;
    public  Vector3 Pivot
    {
        get => pivot + transform.position;
        set { }
    }
}
