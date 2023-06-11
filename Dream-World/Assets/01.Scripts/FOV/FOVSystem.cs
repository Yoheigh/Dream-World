using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FOVSystem : MonoBehaviour
{
    public float viewRadius;            //시야 거리
    public float viewAngle;             //시야 각
    public float interactionRadius = 1f;      // 상호작용 거리
    public float refreshDelay = 0.1f;  // 재탐색 시간

    [SerializeField]
    private Transform closestTransform;
    public Transform ClosestTransform
    {
        get => closestTransform;
        private set => closestTransform = value;
    }

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();      // 보이는 타겟 리스트

    public virtual void Start()
    {

    }

    // flag에 따라 오브젝트 탐색 실행 및 중지
    public void FindTargetsWithDelay(bool _flag)
    {
        switch (_flag)
        {
            case true:
                StartCoroutine(FindTargetsWithDelay(refreshDelay));
                break;

            case false:
                StopCoroutine(FindTargetsWithDelay(refreshDelay));
                break;
        }
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
            GetClosestTarget();
            // Debug.Log(ClosestTransform);
        }
    }

    // 타겟 레이어를 변경하는 일은 아마 없을 것
    public void SetTargetLayer(LayerMask targetLayer)
    {
        targetMask = targetLayer;
    }

    // targetMask인 게임 오브젝트 && 사이에 obstacleMask가 없을 경우 List에 등록
    void FindVisibleTargets()
    {
        ClearTargets();

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)                      // ?
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))   // 장애물이 앞에 있는지
                {
                    visibleTargets.Add(target);
                }
            }
        }

        //visibleTargets2.Clear();

        //Collider[] targetsInViewRadius2 = Physics.OverlapSphere(transform.position, viewRadius, interactionTargetMask);

        //for (int i = 0; i < targetsInViewRadius2.Length; i++)
        //{
        //    Transform target = targetsInViewRadius2[i].transform;
        //    Vector3 dirToTarget = (target.position - transform.position).normalized;

        //    if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)                      // ?
        //    {
        //        float dstToTarget = Vector3.Distance(transform.position, target.position);

        //        if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))   // 장애물이 앞에 있는지
        //        {
        //            visibleTargets2.Add(target);
        //        }
        //    }
        //}

        //VisibleTargetColor(Color.green);
    }

    // visibleTargets 중에서 가장 가까운 오브젝트를 closestTransform에 등록
    public Transform GetClosestTarget()
    {
        // 시야 안에 오브젝트가 없으면 return null;
        if (visibleTargets.Count == 0)
        {
            closestTransform = null;
            return closestTransform;
        }

        float closestDistance = Mathf.Infinity;
        for (int i = 0; i < visibleTargets.Count; i++)
        {
            Transform _target = visibleTargets[i];
            float dstToTarget = Vector3.Distance(transform.position, _target.position);

            // 블럭 사이의 거리를 계산할 때 높낮이를 고려해야 한다면 Distance()로 구한 float 대신 각 Vector3.y의 크기를 비교해야 한다.
            //if (dstToTarget <= viewRadius)
            //{
            // 이전에 탐색한 오브젝트와 동일한 오브젝트일 경우 처리 종료
            if (_target == closestTransform)
                return closestTransform;

            Vector3 dirToTarget = (_target.position - transform.position).normalized;

            // 상호작용할 오브젝트가 인터랙션할 수 있는 범위 안에 들어왔을 경우
            if (Physics.Raycast(transform.position, dirToTarget, interactionRadius, targetMask))   // 장애물이 앞에 있는지
            {
                // 범위에 있는 오브젝트 중 가장 가까운 걸 리턴
                if (dstToTarget < closestDistance)
                {
                    ClosestTransform = _target;
                    closestDistance = dstToTarget;
                }
            }
            //}
        }
        // 1. 시야 안에 오브젝트 있음

        // 2. 이전에 선택했던 오브젝트가 아님

        // 해당 오브젝트 리턴하고 UI 이벤트 업데이트
        return closestTransform;
        // UI 이벤트 Invoke(closestTransform)
    }

    // 기존의 렌더러 접근에서 바꿀 예정
    void VisibleTargetColor(Color color)
    {
        for (int i = 0; i < visibleTargets.Count; i++)
        {
            visibleTargets[i].GetComponent<Renderer>().material.SetColor("_Color", color);
        }
    }

    // 등록된 오브젝트들 제거
    public void ClearTargets()
    {
        visibleTargets.Clear();
        closestTransform = null;
    }

    public Vector3 DirFromAngle(float angleInDegress, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegress += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegress * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegress * Mathf.Deg2Rad));
    }
}
