using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterStateEnum
{
    Idle, Patrol, Detect, Hit, Attack, Stun
}

[RequireComponent(typeof(FOVSystem))]
[System.Serializable]
public class MonsterV2 : MonoBehaviour
{
    // Navmesh 뺀 몬스터
    public MonsterData monsterData;
    public MonsterStateEnum monsterState;
    public FOVSystem fov;
    public Animator anim;
    private State<MonsterV2>[] states;
    private StateMachine<MonsterV2> stateMachine;
    private Rigidbody rigid;

    public Vector3[] patrolPositions;

    // 내부 변수
    private bool isCanMove;
    private bool isCanAttack;
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

        if (patrolPositions.Length == 0)
            anim.Play("Monster_Idle");
        else
            anim.Play("Monster_Walk");
    }

    public void Setup()
    {
        m_speed = monsterData.speed;

        rigid = GetComponent<Rigidbody>();
        fov = GetComponent<FOVSystem>();

        states = new State<MonsterV2>[6];
        stateMachine = new StateMachine<MonsterV2>();

        states[(int)MonsterStateEnum.Idle] = new MonsterOwnedStates.Idle();
        states[(int)MonsterStateEnum.Patrol] = new MonsterOwnedStates.Patrol();
        states[(int)MonsterStateEnum.Detect] = new MonsterOwnedStates.Detect();
        states[(int)MonsterStateEnum.Hit] = new MonsterOwnedStates.Hit();
        states[(int)MonsterStateEnum.Attack] = new MonsterOwnedStates.Attack();
        states[(int)MonsterStateEnum.Stun] = new MonsterOwnedStates.Stun();

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
                OnDetect();
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

    public void Hit()
    {
        StartCoroutine(HitCo());
    }    

    IEnumerator HitCo()
    {
        ChangeState(MonsterStateEnum.Hit);
        Manager.Instance.Sound.PlaySFX(1225);
        yield return new WaitForSeconds(0.01f);
        Destroy(gameObject);
    }

    public void Stun()
    {
        StartCoroutine(StunCo());
    }

    IEnumerator StunCo()
    {
        ChangeState(MonsterStateEnum.Stun);
        yield return new WaitForSeconds(3f);
        ChangeState(MonsterStateEnum.Detect);
    }

    // Idle 업데이트 안에
    public bool CheckStopEnough()
    {
        if (stopCheckTime >= monsterData.stopDelay)
        {
            stopCheckTime = 0;
            ChangeState(MonsterStateEnum.Patrol);
            anim.Play("Monster_Walk");
            return true;
        }

        // Debug.Log($"{stopCheckTime} -> {monsterData.stopDelay}");

        stopCheckTime += Time.deltaTime;
        return false;
    }

    // Patrol 업데이트 안에
    public void Patrol()
    {
        if (patrolPositions.Length == 0f) return;

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
                    anim.Play("Monster_LookAround");
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
        StartCoroutine(OnDetectDelay());
    }

    private IEnumerator OnDetectDelay()
    {
        anim.Play("Monster_Surprised");
        stopCheckTime = 0f;
        m_speed = 0f;
        yield return new WaitForSeconds(0.5f);
        m_speed = monsterData.speed;
        attackCheckTime = monsterData.attackCooltime;
        ChangeState(MonsterStateEnum.Detect);
    }

    // Detect 업데이트 안에
    public void FollowTarget()
    {
        Vector3 dir = (target.position - transform.position);
        dir.y = 0f;

        if (Vector3.Distance(transform.position, target.transform.position) > monsterData.forgetDistance /*!fov.visibleTargets.Contains(target.transform)*/)
        {
            // navMeshAgent.SetDestination(transform.position);
            target = null;
            anim.Play("Monster_LookAround");
            ChangeState(MonsterStateEnum.Idle);
            attackCheckTime = monsterData.attackCooltime;
            return;
        }
        else
        {
            CheckCanAttack();

            if (Vector3.Distance(transform.position, target.transform.position) < monsterData.canAttackDistance)
            {
                anim.Play("Monster_Idle");

                transform.rotation = Quaternion.LookRotation(dir);
                rigid.velocity = new Vector3(0f, rigid.velocity.y, 0f);

                if (isCanAttack == false) return;
                ChangeState(MonsterStateEnum.Attack);
            }
            else
            {
                Debug.DrawLine(transform.position, target.position);

                anim.Play("Monster_Run");

                transform.rotation = Quaternion.LookRotation(dir);
                rigid.velocity = dir.normalized * (m_speed + 0.8f) + new Vector3(0f, rigid.velocity.y, 0f);
            }

            // navMeshAgent.SetDestination(target.position);
        }
    }

    public void CheckCanAttack()
    {
        if (isCanAttack)
            return;
        attackCheckTime += Time.deltaTime;
        if (monsterData.attackCooltime <= attackCheckTime)
        {
            isCanAttack = true;
            attackCheckTime = 0;
        }
    }

    public void Attack()
    {
        isCanAttack = false;
        StartCoroutine(AttackCo());
    }

    IEnumerator AttackCo()
    {
        anim.Play("Monster_Attack");
        yield return new WaitForSeconds(0.25f);

        Collider[] colliders = Physics.OverlapBox(transform.position + transform.forward, /* monsterData.attackSize */ new Vector3(0.5f, 0.5f, 0.5f));

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<PlayerController>().Hit();
            }
        }
        yield return new WaitForSeconds(0.35f);
        ChangeState(MonsterStateEnum.Detect);
    }

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

