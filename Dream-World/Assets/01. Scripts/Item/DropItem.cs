using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DropItem : MonoBehaviour, ICollectable
{
    [SerializeField]
    private int itemID;

    public void Collect()
    {
        Debug.Log("Collected");
        Inventory.instance.AddItem(itemID);
        Destroy(this.gameObject);
    }
}
