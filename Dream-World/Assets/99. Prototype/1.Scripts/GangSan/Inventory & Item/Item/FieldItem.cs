using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    public int itemID;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�浹");
        if(other.CompareTag("Player"))
        {
            Debug.Log("����");
            if (Inventory.instance.AddItem(itemID))
            {
                Destroy(gameObject);
                Inventory.instance.onChangeItem.Invoke();
            }
        }
    }
}
