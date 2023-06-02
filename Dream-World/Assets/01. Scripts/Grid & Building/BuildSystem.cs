using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    // 건물 지을 위치
    public Vector3 BuildPosition;

    // 1. 거기에 그리드 오브젝트 있는지

    // 2. 설치할 GridObject의 Data에 있는 조건들을 가져와서
    // 해당 위치에 건물을 지을 수 있는지 확인

    // 3. 오버랩

    // 조건 처리

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(BuildPosition, 0.1f);
    }
}
