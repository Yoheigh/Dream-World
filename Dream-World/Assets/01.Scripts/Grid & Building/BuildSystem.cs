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

    // 제작할 건물의 Preview 오브젝트
    [SerializeField]
    private GameObject entity;

    [SerializeField]
    private Building buildingData;

    // 건물 지어지는 이펙트
    public GameObject BuildVFX;

    // 내부 변수
    private Vector3 buildPosition;
    private int x, y, z;
    private byte[] availableRot = new byte[4];
    private Vector3 tempPos;
    private byte tempRot;
    private byte currentRot;

    public void UpdatePos()
    {
        // 위치가 바뀔 때마다 반올림해서 변경
        playerForwardVector.GetXYZRound(out x, out y, out z);
    }

    public void BuildCheck()
    {
        tempRot = 0;

        switch(buildingData.buildCondition)
        {
            case BuildCondition.Top:

                if (GridSystem.Instance.StageGrid.GetGridObject(x, y - 1, z).GetGridObjectData().isConstructableTop)
                    availableRot[0] = 1;

                break;

            case BuildCondition.Side:
                if (GridSystem.Instance.StageGrid.GetGridObject(x + 1, y, z).GetGridObjectData().isConstructableSide)
                    availableRot[0] = 1;
                if (!GridSystem.Instance.StageGrid.GetGridObject(x - 1, y, z).GetGridObjectData().isConstructableSide)
                    availableRot[1] = 1;
                if (!GridSystem.Instance.StageGrid.GetGridObject(x, y, z + 1).GetGridObjectData().isConstructableSide)
                    availableRot[2] = 1;
                if (!GridSystem.Instance.StageGrid.GetGridObject(x, y, z - 1).GetGridObjectData().isConstructableSide)
                    availableRot[3] = 1;

                for(int i = 0; i < availableRot.Length - 1; i++)
                {
                    tempRot += availableRot[i];
                }

                if (tempRot == 0) return;

                break;
        }
    }

    // 2. 설치할 GridObject의 Data에 있는 조건들을 가져와서
    // 해당 위치에 건물을 지을 수 있는지 확인

    // 3. 오버랩

    // 조건 처리

    public void RotateBuilding()
    {
        if (tempRot == 0) return;

        for(byte i = 0; i < availableRot.Length - 1; i++)
        {
            if (availableRot[i] == 0) continue;

            if (currentRot < i)
            {
                entity.transform.rotation = Quaternion.Euler(new Vector3(0, 90f * i, 0));
                currentRot = i;
                break;
            }
            else if (currentRot >= i)
                currentRot = 0;
            // 로테이션 추가
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(buildPosition, 0.1f);
    }
}
