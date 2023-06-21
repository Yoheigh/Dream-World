using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingObject : TriggerObject
{
    public bool isEnemyProjectile = false;

    public GameObject obj;
    public MeshRenderer mesh;

    private void OnEnable()
    {
        // mesh = obj.GetComponentInChildren<MeshRenderer>();
        GetComponent<Rigidbody>().useGravity = false;
    }

    protected override void TriggerWith(Collider other)
    {
        if (isEnemyProjectile == false)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<MonsterV2>().Hit();
            }
        }
        Destroy(gameObject);
    }

    protected override void TriggerWithPlayer(PlayerController _player)
    {
        if (isEnemyProjectile == true)
        {
            _player.Hit();
        }
        Destroy(gameObject);
    }
}
