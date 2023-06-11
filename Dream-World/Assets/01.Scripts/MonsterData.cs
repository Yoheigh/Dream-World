using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MonsterData
{
    public int monsterID;
    public MonsterPatrolType monsterPatrolType;
    public MonsterAttackType monsterAttackType;
    public int hp;
    public float speed;
    public float force;
    public float patrolRange;
    public float sightDistance;
    public float detectDistance;
    public float forgetDistance;
    public float canAttackDistance;
    public float attackCooltime;
    public Vector3 attackSize;
    public float stopDelay;
}

public enum MonsterPatrolType
{
    PointToPoint, Turn
}
public enum MonsterPatrolDirection
{
    Right, Left
}

public enum MonsterAttackType
{
    Long, Short
}

