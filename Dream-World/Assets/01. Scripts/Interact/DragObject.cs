using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : InteractionObject
{
    public override ObjectType ObjectType => ObjectType.Dragable;

    [SerializeField]
    private Vector3[] OffsetAnchors = new Vector3[4]
    {
        // 벡터에 따른 플레이어 방향 전환
        new Vector3(1, 0, 0),       // 0, -90, 0
        new Vector3(-1, 0, 0),      // 0, 90, 0
        new Vector3(0, 0, 1),       // 0, 0, 0
        new Vector3(0, 0, -1)       // 0, -180, 0
    };

    private Vector3 tempVector;

    public override void InteractWithPlayer()
    {
        Vector3 player = Vector3.zero;
        for(int i = 0; i < OffsetAnchors.Length; i++)
        {

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        for(int i = 0; i < OffsetAnchors.Length; i++)
        {
            tempVector = transform.position + OffsetAnchors[i];
            Gizmos.DrawSphere(tempVector, 0.1f);
        }
    }
}
