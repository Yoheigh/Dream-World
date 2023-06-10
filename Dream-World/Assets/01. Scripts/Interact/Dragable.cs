using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Dragable : InteractionObject
{
    public override ObjectType ObjectType => ObjectType.Dragable;

    [SerializeField]
    private Vector3[] OffsetAnchors = new Vector3[4]
    {
        // 벡터에 따른 플레이어 방향 전환
        new Vector3(1, 0, 0),       // 1번 : 0, -90, 0
        new Vector3(-1, 0, 0),      // 2번 : 0, 90, 0
        new Vector3(0, 0, 1),       // 3번 : 0, 180, 0
        new Vector3(0, 0, -1)       // 4번 : 0, 0, 0
    };

    private Vector3 tempVector;

    public override void InteractWithPlayer(PlayerController _player)
    {
        Debug.Log("질질 끕니다");
    }

    public Vector3 AnchoredPosition(Vector3 _playerPos, out Vector3 _playerRot)
    {
        Vector3[] anchors = new Vector3[4];
        for (int i = 0; i < anchors.Length; i++)
        {
            anchors[i] = transform.position + OffsetAnchors[i];
        }

        var returnVec = _playerPos.GetClosestVector3(anchors);

        Vector3 tempPos = Vector3.zero;

        for (int i = 0; i < anchors.Length; i++)
        {
            if (returnVec == anchors[i])
            {
                switch (i)
                {
                    case 0:tempPos = new Vector3(0, -90, 0);
                        break;
                    case 1: tempPos = new Vector3(0, 90, 0);
                        break;
                    case 2: tempPos = new Vector3(0, -180, 0);
                        break;
                    case 3: tempPos = new Vector3(0, 0, 0);
                        break;
                }
                break;
            }
        }
        // 플레이어의 방향을 해당 앵커 포인트 방면으로 받는 Vector3를 반환한다.
        _playerRot = tempPos;

        // 플레이어와 가장 가까운 앵커 포인트를 반환한다.
        return returnVec;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        for (int i = 0; i < OffsetAnchors.Length; i++)
        {
            tempVector = transform.position + OffsetAnchors[i];
            Gizmos.DrawSphere(tempVector, 0.1f);
        }
    }
}
