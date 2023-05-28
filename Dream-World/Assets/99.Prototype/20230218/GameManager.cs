using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private PlayerController mainController;

    private StageData currentStageData;

    private bool isGameOver = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
