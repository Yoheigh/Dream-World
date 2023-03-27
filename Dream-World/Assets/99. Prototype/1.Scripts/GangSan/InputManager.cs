using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Singleton
    private static InputManager Instance;
    public static InputManager instance
    {
        get
        {
            if (Instance == null)
            {
                return null;
            }

            return Instance;
        }
    }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion


    public bool inventoryInput = false;
    private void Update()
    {
        if(inventoryInput != Input.GetKeyDown(KeyCode.I))
        inventoryInput = Input.GetKeyDown(KeyCode.I);
    }
}
