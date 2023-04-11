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

    //������ �߰� �Լ�
    public bool AddItem(int itemID)
    {
        Item item_ = ItemDatabass.instance.GetItem(itemID);

        switch (item_.itemType)
        {
            case ItemType.ingredients:
                foreach (Item _item in ingredientsItems)
                {
                    //�̸��� ���� �������� ���� ��
                    if (_item.itemID == item_.itemID)
                    {
                        //�� �������� ���� ������ �ִ� �������� ���� ��
                        if (_item.itemMaxCount > _item.itemCurrentCount)
                        {
                            //������ ������ �÷��ְ� true ����
                            _item.itemCurrentCount++;
                            onChangeItem.Invoke();
                            return true;
                        }
                    }
                }

                if (itemSlotMaxCount > ingredientsItems.Count)
                {
                    Item item__ = new Item(item_.itemID);
                    //������â�� �߰��ϰ� true ����
                    ingredientsItems.Add(item__);
                    onChangeItem.Invoke();
                    return true;

                }
                break;

                //�κ��� ����� �� �߰� �ϴ� �ڵ� �߰�

            case ItemType.equipment:
                foreach (Item _item in equipmentItems)
                {
                    //�̸��� ���� �������� ���� ��
                    if (_item.itemID == item_.itemID)
                    {
                        //�� �������� ���� ������ �ִ� �������� ���� ��
                        if (_item.itemMaxCount > _item.itemCurrentCount)
                        {
                            //������ ������ �÷��ְ� true ����
                            _item.itemCurrentCount++;
                            onChangeItem.Invoke();
                            return true;
                        }
                    }

                }

                if (itemSlotMaxCount > equipmentItems.Count)
                {
                    Item item__ = new Item(item_.itemID);
                    //������â�� �߰��ϰ� true ����
                    equipmentItems.Add(item__);
                    onChangeItem.Invoke();
                    return true;
                }
                break;
        }    
        //������ �ִ� �������� 

        //������ �ִ� �������� �̸��� ���� �������� ���ų� �ִ� ���� �� ��

        //������ â �ִ� �������� ���� ������ â�� ����ϰ� ���� ��

        //�߰����� ������ �� false ����
        return false;
    }

    public bool RemoveItem(int itemID_, int itemCount)
    {
        Item item__ = ItemDatabass.instance.GetItem(itemID_);

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
