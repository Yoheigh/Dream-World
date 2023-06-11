using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    protected int maxHP = 1;

    [SerializeField]
    protected int currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    public abstract void TakeDamage(int _damage);
}
