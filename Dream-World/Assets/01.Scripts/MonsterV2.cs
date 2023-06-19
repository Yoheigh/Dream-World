using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterStateEnum
{
    Idle, Patrol, Detect, Hit
}

[RequireComponent(typeof(FOVSystem))]
[System.Serializable]
public class MonsterV2 : MonoBehaviour
{
    // Navmesh 뺀 몬스터
    public MonsterData monsterData;
    public MonsterStateEnum monsterState;
    public FOVSystem fov;
    private State<MonsterV2>[] states;
    private StateMachine<MonsterV2> stateMachine;
    private Rigidbody rigid;

    public Vector3[] patrolPositions;

    // 내부 변수
    private bool isCanMove;
    private int currentPatrolPos = 0;
    private float attackCheckTime = 0;
    private float stopCheckTime = 0;
    private Transform target;

    [SerializeField]
    private float m_speed;

    private void Awake()
    {
        Setup();

        // 계속 찾어 넌
        fov.FindTargetsWithDelay(true);
    }

    public void Setup()
    {
        m_speed = monsterData.speed;

        rigid = GetComponent<Rigidbody>();
        fov = GetComponent<FOVSystem>();

        states = new State<MonsterV2>[4];
        stateMachine = new StateMachine<MonsterV2>();

        states[(int)MonsterStateEnum.Idle] = new MonsterOwnedStates.Idle();
        states[(int)MonsterStateEnum.Patrol] = new MonsterOwnedStates.Patrol();
        states[(int)MonsterStateEnum.Detect] = new MonsterOwnedStates.Detect();
        states[(int)MonsterStateEnum.Hit] = new MonsterOwnedStates.Hit();

        stateMachine.Setup(this, states[(int)MonsterStateEnum.Patrol]);
    }

    private void Update()
    {
        stateMachine.Execute();
    }

    // 플레이어 찾았으면 Detect 상태로 이동
    public void PlayerDetected()
    {
        for (int i = 0; i < fov.visibleTargets.Count; i++)
        {
            if (fov.visibleTargets[i].CompareTag("Player"))
            {
                target = fov.visibleTargets[i];
                ChangeState(MonsterStateEnum.Detect);
            }
        }
    }

    //public bool FindTarget()
    //{
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, monsterData.sightDistance, attackLayer);
    //    foreach (var item in colliders)
    //    {
    //        Debug.Log(item.name);
    //        if (item.CompareTag("Player"))
    //        {
    //            target = item.transform;
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    // Idle 업데이트 안에
    public bool CheckStopEnough()
    {
        if (stopCheckTime >= monsterData.stopDelay)
        {
            stopCheckTime = 0;
            ChangeState(MonsterStateEnum.Patrol);
            return true;
        }

        // Debug.Log($"{stopCheckTime} -> {monsterData.stopDelay}");

        stopCheckTime += Time.deltaTime;
        return false;
    }

    // Patrol 업데이트 안에
    public void Patrol()
    {
        switch (monsterData.monsterPatrolType)
        {
            case MonsterPatrolType.PointToPoint:
                // 목적지를 향해 이동하는 코드
                // navMeshAgent.SetDestination(patrolPositions[currentPatrolPos]);

                Vector3 dir = (patrolPositions[currentPatrolPos] - transform.position);
                dir.y = 0f;

                // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);
                transform.rotation = Quaternion.LookRotation(dir);
                rigid.velocity = dir.normalized * m_speed;

                //목적지에 도착했는지 체크 하는 코드
                if (Mathf.Abs(patrolPositions[currentPatrolPos].x - transform.position.x) <= 0.1f
                    && Mathf.Abs(patrolPositions[currentPatrolPos].z - transform.position.z) <= 0.1f)
                {
                    //도착했을 때 상태를 아이들로 변경 후 목적지를 다음 목적지로 변경
                    ChangeState(MonsterStateEnum.Idle);
                    if (currentPatrolPos < patrolPositions.Length - 1)
                    {
                        currentPatrolPos++;
                    }
                    else
                    {
                        currentPatrolPos = 0;
                    }
                }
                break;

            case MonsterPatrolType.Turn:
                break;
        }
    }

    public void OnDetect()
    {
        StopCoroutine(OnDetectDelay());
        StartCoroutine(OnDetectDelay());
    }

    private IEnumerator OnDetectDelay()
    {
        stopCheckTime = 0f;
        m_speed = 0f;
        rigid.AddForce(Vector3.up, ForceMode.Impulse);
        yield return new WaitForSeconds(1f);
        m_speed = monsterData.speed;

    }

    // Detect 업데이트 안에
    public void FollowTarget()
    {
        if (!fov.visibleTargets.Contains(target.transform))
        {
            // navMeshAgent.SetDestination(transform.position);
            target = null;
            ChangeState(MonsterStateEnum.Idle);
            return;
        }
        else
        {
            Debug.DrawLine(transform.position, target.position);

            Vector3 dir = (target.position - transform.position);
            dir.y = 0f;

            transform.rotation = Quaternion.LookRotation(dir);
            rigid.velocity = dir.normalized * (m_speed + 0.8f);

            // navMeshAgent.SetDestination(target.position);
        }
    }

    //public void CheckCanAttack()
    //{
    //    if (isCanAttack)
    //        return;
    //    attackCheckTime += Time.deltaTime;
    //    if (monsterData.attackCooltime <= attackCheckTime)
    //    {
    //        isCanAttack = true;
    //        attackCheckTime = 0;
    //    }
    //}

    //public void Attack()
    //{
    //    Collider[] colliders = Physics.OverlapBox(attackPos.position, monsterData.attackSize, Quaternion.identity, attackLayer);

    //    foreach (var collider in colliders)
    //    {
    //        if (collider.CompareTag("Player"))
    //        {
    //            // collider.GetComponent<PlayerController>().Hit(monsterData.force);
    //        }
    //    }
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, monsterData.detectDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, monsterData.forgetDistance);
    }

    public void ChangeState(MonsterStateEnum monsterStateEnum)
    {
        stateMachine.ChangeState(states[(int)monsterStateEnum]);
    }
}

