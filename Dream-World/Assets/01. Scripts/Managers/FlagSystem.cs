using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlagSystem
{
    // 오브젝트 키 모음
    public Dictionary<int, StageFlag> stageFlags;
    public List<StageFlag> stageFlags2;
}

[System.Serializable]
public class StageFlag
{
    // 작동 가능 여부
    public bool isAvailable = false;

    // flag 활성화 조건
    public List<Action> requireActions;

    // flag 활성화 이후 처리
    public List<Action> resultActions;

}
