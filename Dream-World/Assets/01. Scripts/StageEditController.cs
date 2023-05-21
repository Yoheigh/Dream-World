using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEditController : MonoBehaviour
{
    public CustomInput input;

    void Start()
    {
        input = FindObjectOfType<CustomInput>().GetComponent<CustomInput>();
    }

    void Update()
    {
        
    }

    void Move()
    {

    }
}
