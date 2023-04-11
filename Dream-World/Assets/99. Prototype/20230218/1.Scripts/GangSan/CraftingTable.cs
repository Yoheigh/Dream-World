using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour
{
    //애는 UI랑 시스템 둘 다 하는 애
    #region Singleton
    private static CraftingTable Instance;
    public static CraftingTable instance
    {
        get
        {
            if (Instance == null)
            {
                return null;
            }

            return Instance;
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
    public GameObject[] prefabs;
    public Transform slotsHolder;
    public CraftingSlot[] slots;

    Inventory inventory;
    private void Start()
    {
        slots = slotsHolder.GetComponentsInChildren<CraftingSlot>();

        inventory = Inventory.instance;
        inventory.onChangeItem += CheckCanCrafting;
    }

    void CheckCanCrafting()
    {
        foreach(CraftingSlot slot in slots)
        {
            if (slot.isHaveItem)
                continue;

            Item item = ItemDatabass.instance.GetItem(slot.itemID);

            bool[] isEnoughs = new bool[item.needIngredientsItemID.Length];
            bool canCrafting = true;

            for (int i = 0; i < item.needIngredientsItemID.Length; i++)
            {
                foreach(Item item_ in inventory.ingredientsItems)
                {
                    if(item.needIngredientsItemID[i] == item_.itemID && item.needIngredientsItemAmount[i] <= item_.itemCurrentCount)
                    {
                        isEnoughs[i] = true;
                    }
                }
            }

            foreach(bool isEnough in isEnoughs)
            {
                if (!isEnough)
                    canCrafting = false;
            }

            slot.isCanCrafting = canCrafting;

        }
    }

    public bool Crafting(int itemID)
    {
        Item item = ItemDatabass.instance.GetItem(itemID);
        for (int i = 0; i < item.needIngredientsItemID.Length; i++)
        {
            Inventory.instance.RemoveItem(item.needIngredientsItemID[i], item.needIngredientsItemAmount[i]);
        }

        if (Inventory.instance.AddItem(itemID))
        {
            return true;
        }

        return false;






        //switch(itemID)
        //{
        //    case 100:
        //        bool enoughWood = false;
        //        bool enoughIron = false;
        //        foreach (Item item_ in inventory.equipmentItems)
        //        {
        //            if (item_.itemID == 1001 && item_.itemCurrentCount >= 1)
        //                enoughWood = true;

        //            if (item_.itemID == 1002 && item_.itemCurrentCount >= 1)
        //                enoughIron = true;
        //        }

        //        if (enoughIron && enoughWood)
        //        {
        //            if (inventory.AddItem(itemID))
        //            {
        //                inventory.RemoveItem(1001, 1);
        //                inventory.RemoveItem(1002, 1);
        //                Instantiate(prefabs[0], Player_Break.instance.transform);
        //            }
        //        }

        //        break;
        //}
    }

    public int SlotOutLineRedrow(int itemSlotCount)
    {
        foreach (CraftingSlot slot in slots)
        {
            slot.DeSelect();
        }

        if (itemSlotCount == -1)
            return -1;

        foreach (CraftingSlot slot in slots)
        {
            if (slot.slotID == itemSlotCount)
            {
                if (slot.isHaveItem)
                {
                    slot.Select();
                    return slot.itemID;
                }
            }
        }
        return -1;
    }

    
}
