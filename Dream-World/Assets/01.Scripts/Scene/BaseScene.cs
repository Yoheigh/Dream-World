using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    public virtual string sceneName { get; set; }
    public virtual int sceneIndex { get; set; }

    public bool sceneDataShow;
    public string sceneData0, sceneData1;

    public virtual Define.Scene sceneType { get; set; }
    public StageFlag sceneFlag;

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
