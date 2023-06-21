using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnmoveMonster : MonoBehaviour
{
    // Navmesh 뺀 몬스터
    public MonsterData monsterData;
    public MonsterStateEnum monsterState;
    public Animator anim;
    private Rigidbody rigid;

    // 내부 변수
    private bool isCanAttack;
    private float attackCheckTime = 0;

    [SerializeField]
    private float m_speed;

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        StartCoroutine(DetectBehaviour());
    }

    private void Update()
    {
        CheckCanAttack();
    }

    IEnumerator DetectBehaviour()
    {
        var colliders = Physics.OverlapSphere(transform.position, 1.5f);
        for(int i = 0; i < colliders.Length - 1; i++)
        {
            if (colliders[i].CompareTag("Player"))
                Attack();

        }
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(DetectBehaviour());
    }

    public void Hit()
    {
        StartCoroutine(HitCo());
    }

    IEnumerator HitCo()
    {
        anim.Play("Monster_Stun");
        yield return new WaitForSeconds(1f);
        Destroy(this);
    }

    public void Stun()
    {
        StartCoroutine(StunCo());
    }

    IEnumerator StunCo()
    {
        anim.Play("Monster_Stun");
        isCanAttack = false;
        yield return new WaitForSeconds(1f);
        isCanAttack = false;
        yield return new WaitForSeconds(1f);
        isCanAttack = false;
        anim.Play("Monster_Idle");
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
        if (isCanAttack == false) return;

        isCanAttack = false;
        StartCoroutine(AttackCo());
    }

    IEnumerator AttackCo()
    {
        anim.Play("Monster_Attack");
        yield return new WaitForSeconds(0.2f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<PlayerController>().Hit();
            }
        }
        yield return new WaitForSeconds(0.35f);
        anim.Play("Monster_Idle");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, monsterData.detectDistance);
    }
}
