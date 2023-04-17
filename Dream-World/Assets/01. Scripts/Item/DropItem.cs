using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DropItem : MonoBehaviour, ICollectable
{
    void Start()
    {

    }

    void Update()
    {

    }

    private IEnumerator FloatingMovement()
    {
        yield return null;
    }

    public void Collect()
    {
        Destroy(this.gameObject);
        // ItemDatabass.instance.GetItem();
    }
}
