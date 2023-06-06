using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Delay : MonoBehaviour
{
    void Start()
    {
        if (gameObject.CompareTag("UI") == true)
        {
            Destroy(gameObject, 3);
        }
    }
}
