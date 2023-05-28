using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아 이제 슬슬 bass를 놔줘야 하는가 (님아..)
public class ItemDatabase : MonoBehaviour
{
    #region Sington;
    private static ItemDatabase Instance;
    public static ItemDatabase instance
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
    public List<BlockData> blocks;

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

    public BlockData GetBlock(int blockID)
    {
        foreach (BlockData block in blocks)
        {
            if (block.blockID == blockID)
            {
                Debug.Log(block.blockID + "리턴 됨");
                return block;
            }
        }
        return default;
    }
}
