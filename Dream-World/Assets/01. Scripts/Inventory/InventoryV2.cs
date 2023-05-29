using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[System.Serializable]
public class InventoryV2
{
    Manager Manager => Manager.Instance;

    public List<ItemV2> ingredients;    // 재료
    public List<ItemV2> consumables;    // 건축물, 소모품
    public List<ItemV2> equipments;      // 장비

    // 최대 슬롯 개수 && Capacity 할당량
    public int MaxIngredientSlots = 16;
    public int MaxConsumableSlots = 16;
    public int MaxEquipmentSlots = 4;

    public void Init()
    {
        // 메모리 할당
        ingredients = new List<ItemV2>(MaxIngredientSlots);
        consumables = new List<ItemV2>(MaxConsumableSlots);
        equipments = new List<ItemV2>(MaxEquipmentSlots);
    }

    // 인벤토리에 아이템 추가 ( 아이템 ID )
    public void AddItem(int itemID)
    {
        Manager.Data.GetItem(itemID);
    }

    // 인벤토리에 아이템 추가 ( 아이템 객체 )
    public void AddItem(ItemV2 item)
    {
        switch(item.itemType)
        {
            case ItemTypeV2.Ingredient:
                for (int i = 0; i < MaxIngredientSlots; i++)
                {
                    try
                    {
                        Debug.Log("1. 아이템을 찾고 있습니다.");
                        var itemInSlot = ingredients[i];
                        Debug.Log("2. 아이템을 찾았습니다.");

                        // 아이템이 들어있으면
                        if (itemInSlot.itemID == item.itemID)
                        {
                            // 아이템 최대 개수보다 적으면
                            // 줍는 아이템의 개수를 더했을 때 최대 개수보다 적거나 같으면
                            if(itemInSlot.itemMaxCount > itemInSlot.itemCount &&
                                itemInSlot.itemMaxCount >= itemInSlot.itemCount + item.itemCount)
                            {
                                itemInSlot.itemCount += item.itemCount;
                                Debug.Log($"3. {i}번째 슬롯 {itemInSlot.itemName}에 {item.itemCount} 개수 추가 현재 : {itemInSlot.itemCount}");

                                return;
                            }
                        }
                    }
                    catch(System.ArgumentOutOfRangeException)
                    {
                        Debug.Log("2. 아이템이 존재하지 않습니다.");
                        break;
                    }
                }

                // 아이템 슬롯이 비어 있으면
                // 아이템 슬롯 최대 개수보다 아이템이 적으면
                if (MaxIngredientSlots > ingredients.Count)
                {
                    // 아이템 슬롯에 새로운 아이템 추가
                    ingredients.Add(item);
                    Debug.Log($"3. 아이템 추가 : {item}");


                    // 이벤트 처리
                }
                break;

            case ItemTypeV2.Equipment:

                break;
        }
    }

    // 인벤토리에서 아이템 제거
    public void RemoveItem() { }

    // 아이템 하나 찾을 때마다 한 번씩 돌리기 좀 그런데
    // 콜백 넣어서 하나 찾을 때마다 바로 리턴하게 할까?
    public ItemV2 GetInventoryItem(int itemID, Action<ItemV2> callback = null)
    {
        foreach(var item in ingredients)
        {
            if(item.itemID == itemID)
            {
                callback?.Invoke(item);
                return item;
            }
        }
        return null;
    }
}
