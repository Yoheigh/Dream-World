using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildSystem : MonoBehaviour
{
    [SerializeField]
    private BuildPreview previewPrefab;

    private Transform testTransform;

    // 내부 변수
    private Vector3 buildPosition;

    private void Update()
    {
        // 얜 계속 체크할거라 위치 변경해줘야 함
        buildPosition = testTransform.position.GetXYZRound();
    }

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

    public bool BuildCheck()
    {
        return Physics.OverlapBox(buildPosition, Vector3.one) == null;
    }

    // 해당 블럭의 가장 근처 x, y, z 값을 반환해주는 함수
    
}
