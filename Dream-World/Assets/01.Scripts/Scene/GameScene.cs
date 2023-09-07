using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    // public override string sceneName
    // public override int sceneIndex

    public override Define.Scene sceneType => Define.Scene.GameScene;

    protected new virtual void Init()
    {
        base.Init();

        // 플레이어 조작 초기화 및 이것저것
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
