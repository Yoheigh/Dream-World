using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class BuildSystem
{
    // 건물 지을 위치
    [SerializeField]
    private Vector3 buildPositionOffset;

    // 플레이어 캐릭터의 앞쪽 벡터값
    public Vector3 PlayerForwardVector
    {
        get => playerForwardVector;
        set => playerForwardVector = value;
    }
    private Vector3 playerForwardVector;

    // 제작할 건물 가져오기
    [SerializeField]
    private GameObject entity;

    // 건물 지어지는 이펙트
    public GameObject BuildVFX;

    // 내부 변수
    private Vector3 buildPosition;
    private int x, y, z;

    public void UpdatePos()
    {
        // 위치가 바뀔 때마다 반올림해서 변경
        playerForwardVector.GetXYZRound(out x, out y, out z);
    }

    public void BuildCheck()
    {
        //// 1. 거기에 그리드 오브젝트 있는지
        //if (!GridSystem.Instance.StageGrid.GetGridObject(x + 1, y, z).GetGridObjectData().isAffectedByGravity) return;
        //if (!GridSystem.Instance.StageGrid.GetGridObject(x - 1, y, z).GetGridObjectData().isAffectedByGravity) return;
        //if (!GridSystem.Instance.StageGrid.GetGridObject(x, y, z + 1).GetGridObjectData().isAffectedByGravity) return;
        //if (!GridSystem.Instance.StageGrid.GetGridObject(x, y, z - 1).GetGridObjectData().isAffectedByGravity) return;
    }

    // 2. 설치할 GridObject의 Data에 있는 조건들을 가져와서
    // 해당 위치에 건물을 지을 수 있는지 확인

    // 3. 오버랩

    // 조건 처리

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(buildPosition, 0.1f);
    }
}
