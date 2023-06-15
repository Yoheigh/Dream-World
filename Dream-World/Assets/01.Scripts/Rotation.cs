using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public int Speed = 2;

    void Start()
    {
        gameObject.transform.localEulerAngles = new Vector3(0, -90, 90);
    }

    void Update()
    {
        gameObject.transform.Rotate(Vector3.up * Time.deltaTime * Speed);
    }
}
