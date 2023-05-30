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

    // 아이템이 변경될 때 실행되는 이벤트
    public Action OnChangeItem;

    // 장비 아이템 선택할 때 실행되는 이벤트
    public Action OnChangeEquipment;

    // 내부 변수
    private List<ItemV2> tempList;
    private int tempSlotsCount;
    private int currentEquipmentSlot;   // 선택된 장비 슬롯

    public void Init()
    {
        // 메모리 할당
        ingredients = new List<ItemV2>(MaxIngredientSlots);
        consumables = new List<ItemV2>(MaxConsumableSlots);
        equipments = new List<ItemV2>(MaxEquipmentSlots);

        OnChangeEquipment += () => { Debug.Log("장비 변경됨 수고 비읍"); };
        OnChangeEquipment += () => { Debug.Log($"{currentEquipmentSlot} 현재 장비 슬롯"); };
    }

    // 인벤토리에 아이템 추가 ( 아이템 ID )
    public void AddItem(int _itemID)
    {
        Manager.Data.GetItem(_itemID);
    }

    // 인벤토리에 아이템 추가 ( 아이템 객체 )
    /* 반복문 너무 많이 쓰는데 리팩토링 생각 */
    public void AddItem(ItemV2 _item)
    {
        tempList = null;
        tempSlotsCount = 0;

        // 아이템 타입에 따라서 처리할 리스트 지정
        switch (_item.itemType)
        {
            case ItemTypeV2.Ingredient:
                tempList = ingredients;
                tempSlotsCount = MaxIngredientSlots;
                break;

            case ItemTypeV2.Consumable:
                tempList = consumables;
                tempSlotsCount = MaxConsumableSlots;
                break;

            case ItemTypeV2.Equipment:
                tempList = equipments;
                tempSlotsCount = MaxEquipmentSlots;
                break;
        }

        // 실제 구현
        for (int i = 0; i < tempSlotsCount; i++)
        {
            try
            {
                var itemInSlot = tempList[i];

                // 아이템이 들어있으면
                if (itemInSlot.itemID == _item.itemID)
                {
                    // 아이템 최대 개수보다 적으면
                    // 줍는 아이템의 개수를 더했을 때 최대 개수보다 적거나 같으면
                    if (itemInSlot.itemMaxCount > itemInSlot.itemCount &&
                        itemInSlot.itemMaxCount >= itemInSlot.itemCount + _item.itemCount)
                    {
                        itemInSlot.itemCount += _item.itemCount;
                        Debug.Log($"{i + 1}번째 슬롯 {itemInSlot.itemName} + {_item.itemCount}개 추가 -> 현재 : {itemInSlot.itemCount}개");

                        return;
                    }
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                Debug.Log("아이템이 존재하지 않습니다.");
                break;
            }
        }

        // 아이템 슬롯이 비어 있으면
        // 아이템 슬롯 최대 개수보다 아이템이 적으면
        if (tempSlotsCount > tempList.Count)
        {
            // 아이템 슬롯에 새로운 아이템 추가
            tempList.Add(_item);
            Debug.Log($"아이템 추가 : {_item.itemName}");

            // 이벤트 처리
            // OnItemAdded.Invoke();
        }
    }

    // 인벤토리에서 아이템 제거
    public void RemoveItem(ItemV2 _item)
    {
        tempList = null;

        switch (_item.itemType)
        {
            case ItemTypeV2.Ingredient:
                tempList = ingredients;
                break;

            case ItemTypeV2.Consumable:
                tempList = consumables;
                break;

            case ItemTypeV2.Equipment:
                tempList = equipments;
                break;
        }

        foreach (var item in tempList)
        {
            if (item.itemID == _item.itemID)
            {
                // 아이템 개수가 빼야하는 만큼 충분히 있다면
                if (item.itemCount - _item.itemCount >= 0)
                {
                    // 아이템 개수 감소
                    item.itemCount -= _item.itemCount;
                    Debug.Log($"{item.itemName}의 개수 - {_item.itemCount} 감소 -> 현재 : {item.itemCount}");
                    return;
                }

                // 아이템 개수가 0이면
                if (item.itemCount == 0)
                {
                    // 아이템 슬롯에서 아이템 제거
                    tempList.Remove(item);
                    Debug.Log($"{item.itemName} 제거");

                    // 이벤트 처리
                }
                return;
            }
        }
    }

    // 인벤토리에 있는 아이템 반환 ( 반환할 때마다 콜백 가능 )
    /* 필요한 아이템 찾을 때마다 콜백으로 필요 개수 줄이는 식으로 사용 */
    public ItemV2 GetInventoryItem(int _itemID, Action<ItemV2> callback = null)
    {
        foreach (var item in ingredients)
        {
            if (item.itemID == _itemID)
            {
                callback?.Invoke(item);
                return item;
            }
        }
        return null;
    }

    // 그냥 이 아이템 있냐고 그냥 찾아버리기
    public ItemV2 GetInventoryItem(ItemV2 _item, Action<ItemV2> callback = null)
    {
        foreach (var item in ingredients)
        {
            if (item.itemID == _item.itemID)
            {
                callback?.Invoke(item);
                return item;
            }
        }
        return null;
    }

    // 장비 아이템 선택
    public void SelectEquipment(int _slot)
    {
        if (currentEquipmentSlot == _slot) return;

        currentEquipmentSlot = _slot;
        OnChangeEquipment?.Invoke();
    }

    // 장비 아이템 선택
    public void ChangeEquipment()
    {
        currentEquipmentSlot = (currentEquipmentSlot + 1) % (MaxEquipmentSlots + 1);
        OnChangeEquipment?.Invoke();
    }
}
