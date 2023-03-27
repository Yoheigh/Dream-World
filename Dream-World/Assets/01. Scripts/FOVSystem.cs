using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVSystem : MonoBehaviour
{
    public float viewRadius;        //�þ� �Ÿ�
    public float viewAngle;         //�þ� ��

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();      // ���̴� Ÿ�� ����Ʈ
    public virtual void Start()
    {
        StartCoroutine(FindTargetsWithDelay(0.2f));
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        VisibleTargetColor(Color.white);
        visibleTargets.Clear();

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)                      // ?
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))   // ��ֹ��� �տ� �ִ���
                {
                    visibleTargets.Add(target);
                }
            }
        }

        VisibleTargetColor(Color.green);
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
