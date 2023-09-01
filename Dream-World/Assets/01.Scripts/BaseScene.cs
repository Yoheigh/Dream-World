using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    public string sceneName;
    public int sceneIndex;
    public bool sceneDataShow;
    public string sceneData0, sceneData1;
    public string sceneType;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {

    }

    protected virtual void Clear()
    {

    }
}
