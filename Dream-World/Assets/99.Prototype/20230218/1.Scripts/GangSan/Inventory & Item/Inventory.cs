using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    private static Inventory Instance;
    public static Inventory instance
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
    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public List<Item> ingredientsItems;
    public List<Item> equipmentItems;
    public int itemSlotMaxCount;

    //아이템 추가 함수
    public bool AddItem(int itemID)
    {
        Item item_ = ItemDatabase.instance.GetItem(itemID);

        switch (item_.itemType)
        {
            case ItemType.ingredients:
                foreach (Item _item in ingredientsItems)
                {
                    //이름이 같은 아이템이 있을 때
                    if (_item.itemID == item_.itemID)
                    {
                        //그 아이템의 현재 개수가 최대 개수보다 적을 때
                        if (_item.itemMaxCount > _item.itemCurrentCount)
                        {
                            //아이템 개수를 늘려주고 true 리턴
                            _item.itemCurrentCount++;
                            onChangeItem.Invoke();
                            return true;
                        }
                    }
                }

                if (itemSlotMaxCount > ingredientsItems.Count)
                {
                    Item item__ = new Item(item_.itemID);
                    //아이템창을 추가하고 true 리턴
                    ingredientsItems.Add(item__);
                    onChangeItem.Invoke();
                    return true;

                }
                break;

                //인벤이 비었을 때 추가 하는 코드 추가

            case ItemType.equipment:
                foreach (Item _item in equipmentItems)
                {
                    //이름이 같은 아이템이 있을 때
                    if (_item.itemID == item_.itemID)
                    {
                        //그 아이템의 현재 개수가 최대 개수보다 적을 때
                        if (_item.itemMaxCount > _item.itemCurrentCount)
                        {
                            //아이템 개수를 늘려주고 true 리턴
                            _item.itemCurrentCount++;
                            onChangeItem.Invoke();
                            return true;
                        }
                    }

                }

                if (itemSlotMaxCount > equipmentItems.Count)
                {
                    Item item__ = new Item(item_.itemID);
                    //아이템창을 추가하고 true 리턴
                    equipmentItems.Add(item__);
                    onChangeItem.Invoke();
                    return true;
                }
                break;
        }    
        //가지고 있는 아이템중 

        //가지고 있는 아이템중 이름이 같은 아이템이 없거나 최대 개수 일 때

        //아이템 창 최대 개수보다 적게 아이템 창을 사용하고 있을 때

        //추가하지 못했을 시 false 리턴
        return false;
    }

    public bool RemoveItem(int itemID_, int itemCount)
    {
        Item item__ = ItemDatabase.instance.GetItem(itemID_);

        switch (item__.itemType)
        {
            case ItemType.ingredients:
                for(int i = 0; i < itemCount; i++)
                {
                    foreach (Item item in ingredientsItems)
                    {
                        if (item.itemID == itemID_)
                        {
                            if(item.itemCurrentCount >= 1)
                            {
                                item.itemCurrentCount--;
                                if(item.itemCurrentCount <= 0)
                                {
                                    ingredientsItems.Remove(item);
                                    onChangeItem.Invoke();
                                }
                            }
                            //if (item.itemCurrentCount >= itemCount)
                            //{
                            //    item.itemCurrentCount -= itemCount;
                            //    if (item.itemCurrentCount <= 0)
                            //        ingredientsItems.Remove(item);
                            //    onChangeItem.Invoke();
                            //}
                        }
                    }
                }
                break;

            case ItemType.equipment:
                foreach (Item item in equipmentItems)
                {
                    if (item.itemID == itemID_)
                    {
                        if (item.itemCurrentCount >= itemCount)
                        {
                            item.itemCurrentCount -= itemCount;
                            if (item.itemCurrentCount <= 0)
                                equipmentItems.Remove(item);
                            onChangeItem.Invoke();
                            return true;
                        }
                    }
                }
                break;
        }   

        return false;   
    }



}
