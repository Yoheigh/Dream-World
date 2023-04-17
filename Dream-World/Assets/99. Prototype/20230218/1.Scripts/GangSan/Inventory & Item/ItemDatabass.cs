using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아 이제 슬슬 bass를 놔줘야 하는가 (님아..)
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
                Debug.Log(item.itemID + "리턴 됨");
                return item;
            }
        }

        return null;
    }
}
