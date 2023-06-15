using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Monster : MonoBehaviour
{
    public MonsterData monsterData;
    public MonsterStateEnum monsterState;
    private State<Monster>[] states;
    private StateMachine<Monster> stateMachine;
    public Transform attackPos;
    public LayerMask attackLayer;
    private bool isCanAttack = true;
    private bool isCanMove;
    private int currentPatrolPos = 0;
    private float attackCheckTime = 0;
    private float stopCheckTime = 0;
    private Transform target;

    public Vector3[] patrolPositions;

    NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = monsterData.speed;

        Setup();
    }

    public void Setup()
    {
        states = new State<Monster>[5];
        stateMachine = new StateMachine<Monster>();

        //states[(int)MonsterStateEnum.Idle] = new MonsterState.Idle();
        //states[(int)MonsterStateEnum.Patrol] = new MonsterState.Patrol();
        //states[(int)MonsterStateEnum.Detect] = new MonsterState.Detect();
        //states[(int)MonsterStateEnum.Attack] = new MonsterState.Attack();
        //states[(int)MonsterStateEnum.Hit] = new MonsterState.Hit();

        stateMachine.Setup(this, states[(int)MonsterStateEnum.Patrol]);

    }

    private void FixedUpdate()
    {
        stateMachine.Execute();
        Debug.Log("몬스터 업데이트");
    }

    public bool FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, monsterData.sightDistance, attackLayer);
        foreach (var item in colliders)
        {
            Debug.Log(item.name);
            if(item.CompareTag("Player"))
            {
                target = item.transform;
                return true;
            }
        }
        return false;
    }

    public bool CheckStopEnough()
    {
        if(stopCheckTime >= monsterData.stopDelay)
        {
            stopCheckTime = 0;
            return true;
        }

        stopCheckTime += Time.deltaTime;
        return false;
    }

    public void Patrol()
    {
        switch (monsterData.monsterPatrolType)
        {
            case MonsterPatrolType.PointToPoint:
                //목적지를 향해 이동하는 코드
                navMeshAgent.SetDestination(patrolPositions[currentPatrolPos]);
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                Debug.Log(currentPatrolPos);

                //목적지에 도착했는지 체크 하는 코드
                if (Vector3.Distance(patrolPositions[currentPatrolPos], transform.position) <= 0.01f)
                {
                    //도착했을 때 상태를 아이들로 변경 후 목적지를 다음 목적지로 변경
                    stateMachine.ChangeState(states[(int)MonsterStateEnum.Idle]);
                    if(currentPatrolPos < patrolPositions.Length - 1)
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

    public void FollowTarget()
    {
        if(Vector3.Distance(target.position, transform.position) > monsterData.forgetDistance)
        {
            navMeshAgent.SetDestination(transform.position);
            return;
        }

        else
        {
            transform.LookAt(target.position);
            navMeshAgent.SetDestination(target.position);
            stateMachine.ChangeState(states[(int)MonsterStateEnum.Patrol]);
        }
    }

    public void CheckCanAttack()
    {
        if (isCanAttack)
            return;
        attackCheckTime += Time.deltaTime;
        if(monsterData.attackCooltime <= attackCheckTime)
        {
            isCanAttack = true;
            attackCheckTime = 0;
        }
    }

    public void Attack()
    {
        Collider[] colliders = Physics.OverlapBox(attackPos.position, monsterData.attackSize,Quaternion.identity,attackLayer);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                // collider.GetComponent<PlayerController>().Hit(monsterData.force);
            }
        }
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

//public enum MonsterStateEnum
//{
//    Idle ,Patrol , Detect , Attack , Hit
//}
