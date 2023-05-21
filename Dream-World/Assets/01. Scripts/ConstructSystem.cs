using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructSystem
{
    [SerializeField]
    private ConstructurePreview previewPrefab;


    public void Construct()
    {
        if (!IsConstructable()) return;

        // 해당 블럭을 설치한다
    }

    public bool IsConstructable()
    {
        return true;
    }

    public void RotatePrefab()
    {
        // 해당 블럭의 크기와 중심 피봇을 기준으로 방향을 돌린다
    }

    public void MovePrefabPosition()
    {
        // Preview 되는 프리팹의 위치를 변경한다
    }
}
