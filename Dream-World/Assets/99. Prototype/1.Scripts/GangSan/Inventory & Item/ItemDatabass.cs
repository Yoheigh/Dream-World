using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�� ���� ���� bass�� ����� �ϴ°�
public class ItemDatabass : MonoBehaviour
{
    #region Sington;
    private static ItemDatabass Instance;
    public static ItemDatabass instance
    {
        get
        {
            if (Instance != null)
                return Instance;

            else
                return null;
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else
            Destroy(gameObject);
    }
    #endregion
    public List<Item> items;

    public Item GetItem(int itemID)
    {
        foreach(Item item in items)
        {
            if(item.itemID == itemID)
            {
                Debug.Log(item.itemID + "���� ��");
                return item;
            }
        }

        return null;
    }
}
