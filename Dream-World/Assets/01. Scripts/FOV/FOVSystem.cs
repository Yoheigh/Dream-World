using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ObjectTagType
{
    Block = 0, Interactable, Item
}

public class FOVSystem : MonoBehaviour
{
    public ObjectTagType targetTag = ObjectTagType.Interactable;

    public float viewRadius;            //시야 거리
    public float viewAngle;             //시야 각
    public float refreshDelay = 0.1f;  // 추가 : 코루틴 속도

    private Transform closestTransform;
    public Transform ClosestTransform
    { get => closestTransform;
      set => closestTransform = value;
    }  // 얘도 추가요~

    private Renderer tempRenderer;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();      // 보이는 타겟 리스트
    public virtual void Start()
    {
        StartCoroutine(FindTargetsWithDelay(refreshDelay));
    }

    void Update()
    {
         ClosestTargetColor();
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    public void SetTargetLayer(LayerMask targetLayer)
    {
        targetMask = targetLayer;
    }

    public Transform GetClosestTarget()
    {
        float closestDistance = Mathf.Infinity;
        for (int i = 0; i < visibleTargets.Count; i++)
        {
            Transform _target = visibleTargets[i];
            float dstToTarget = Vector3.Distance(transform.position, _target.position);

            // 블럭 사이의 거리를 계산할 때 높낮이를 고려해야 한다면 Distance()로 구한 float 대신 각 Vector3.y의 크기를 비교해야 한다.

            if(dstToTarget < closestDistance)
            {
                ClosestTransform = _target;
                closestDistance = dstToTarget;
            }
        }

        return ClosestTransform;
    }

    void ClosestTargetColor()
    {
        if (GetClosestTarget() != null)
        {
            if (!ClosestTransform.CompareTag(targetTag.ToString())) return;

            Renderer renderer = ClosestTransform.GetComponentInChildren<Renderer>();

            if (renderer == null) return;

            if (renderer != tempRenderer && tempRenderer != null)
                tempRenderer.material.color = Color.white;

            renderer.material.color = new Color(0.5f, 0.5f, 1f);
            tempRenderer = renderer;
        }
    }

    void FindVisibleTargets()
    {
        //VisibleTargetColor(Color.white);
        visibleTargets.Clear();

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

        //VisibleTargetColor(Color.green);
    }

    public Vector3 DirFromAngle(float angleInDegress, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegress += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegress * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegress * Mathf.Deg2Rad));
    }

    void VisibleTargetColor(Color color)
    {
        for (int i = 0; i < visibleTargets.Count; i++)
        {
            visibleTargets[i].GetComponent<Renderer>().material.SetColor("_Color", color);
        }
    }
}
