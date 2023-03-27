using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    public int itemID;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("충돌");
        if(other.CompareTag("Player"))
        {
            Debug.Log("실행");
            if (Inventory.instance.AddItem(itemID))
            {
                Destroy(gameObject);
                Inventory.instance.onChangeItem.Invoke();
            }
        }
    }
}
