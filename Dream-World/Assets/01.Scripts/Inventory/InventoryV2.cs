using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[System.Serializable]
public class InventoryV2
{
    Managers Manager => Managers.Instance;

    /* 딕셔너리화 필요한 경우 할 예정 */
    /* 인벤토리 정리를 인덱스로 하기 위해 List로 설정 */
    public List<ItemV2> ingredients;    // 재료
    public List<ItemV2> buildings;      // 건축물, 소모품
    public List<ItemV2> equipments;     // 장비
    public List<ItemRecipe> recipes;    // 아이템 레시피 모음

    // 최대 슬롯 개수 && Capacity 할당량
    public int MaxIngredientSlots = 16;
    public int MaxBuildingSlots = 4;
    public int MaxEquipmentSlots = 3;

    // 아이템이 변경될 때 실행되는 이벤트
    public Action OnChangeItem;

    // 아이템 선택할 때 실행되는 이벤트
    public Action<int> OnChangeEquipment;
    public Action<int> OnChangeBuilding;
    public int currentEquipmentSlot;   // 선택된 장비 슬롯
    public int currentBuildingSlot;    // 선택된 건물 슬롯

    // 내부 변수
    private List<ItemV2> tempList;
    private int tempSlotsCount;

    public void Init()
    {
        // 메모리 할당
        //ingredients = new List<ItemV2>(MaxIngredientSlots);
        //buildings = new List<ItemV2>(MaxBuildingSlots);
        //equipments = new List<ItemV2>(MaxEquipmentSlots);

        //OnChangeEquipment += () => { Debug.Log("장비 변경됨 수고 비읍"); };
        //OnChangeEquipment += () => { Debug.Log($"{currentEquipmentSlot} 현재 장비 슬롯"); };

        ingredients.Clear();
        buildings.Clear();
        equipments.Clear();

        //for (int i = 0; i < ingredients.Count; i++)
        //{
        //    RemoveItem(ingredients[i]);
        //}
        //for (int i = 0; i < buildings.Count; i++)
        //{
        //    RemoveItem(buildings[i]);
        //}
        //for (int i = 0; i < equipments.Count; i++)
        //{
        //    RemoveItem(equipments[i]);
        //}
    }

    // 인벤토리에 아이템 추가 ( 아이템 ID )
    public void AddItem(int _itemID)
    {
        Managers.Data.GetItem(_itemID);
    }

    // 인벤토리에 아이템 추가 ( 아이템 객체 )d
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

            case ItemTypeV2.Building:
                tempList = buildings;
                tempSlotsCount = MaxBuildingSlots;
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

                        // 이벤트 처리
                        OnChangeItem?.Invoke();
                        return;
                    }
                }
            }
            catch
            {
                Debug.Log("아이템이 존재하지 않습니다.");
                break;
            }
        }

        // 아이템 슬롯이 비어 있으면
        // 아이템 슬롯 최대 개수보다 아이템이 적으면
        if (tempSlotsCount > tempList.Count)
        {
            switch (_item.itemType)
            {
                case ItemTypeV2.Ingredient:
                    tempList.Add(new ItemV2(_item));
                    break;

                case ItemTypeV2.Equipment:
                    tempList.Add(new Equipment(_item as Equipment));
                    break;

                case ItemTypeV2.Building:
                    tempList.Add(new Building(_item as Building));
                    break;
            }

            // 아이템 슬롯에 새로운 아이템 추가
            Debug.Log($"아이템 추가 {_item.name}");

            // 이벤트 처리
            OnChangeItem?.Invoke();
        }

        /* 딕셔너리 구현 */
        //if (tempDic.TryGetValue(_item.itemName, out var result) == true)
        //{
        //    // 아이템 최대 개수보다 적으면
        //    // 줍는 아이템의 개수를 더했을 때 최대 개수보다 적거나 같으면
        //    if (result.itemMaxCount > result.itemCount &&
        //        result.itemMaxCount >= result.itemCount + _item.itemCount)
        //    {
        //        result.itemCount += _item.itemCount;
        //        Debug.Log($"{result.itemName} + {_item.itemCount}개 추가 -> 현재 : {result.itemCount}개");
        //    }
        //}

        //// 아이템 슬롯이 비어 있으면
        //// 아이템 슬롯 최대 개수보다 아이템이 적으면
        //if (tempSlotsCount > tempDic.Count)
        //{
        //    ItemV2 newItem = new ItemV2(_item);

        //    // 아이템 슬롯에 새로운 아이템 추가
        //    tempDic.Add(_item.itemName, newItem);
        //    Debug.Log($"아이템 추가 : {newItem.itemName}");

        //    // 이벤트 처리
        //    OnChangeItem?.Invoke();
        //}

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

            case ItemTypeV2.Building:
                tempList = buildings;
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
                }

                // 아이템 개수가 0이면
                if (item.itemCount == 0)
                {
                    // 아이템 슬롯에서 아이템 제거
                    tempList.Remove(item);
                    Debug.Log($"{item.itemName} 제거");
                }

                // 이벤트 처리
                OnChangeItem?.Invoke();

                return;
            }
        }

        // 아이템 슬롯이 비워질 경우 체크
        OnChangeEquipment?.Invoke(currentEquipmentSlot);
        OnChangeBuilding?.Invoke(currentBuildingSlot);
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

    public bool TryGetInventoryItem(ItemV2 _item, out ItemV2 item)
    {
        var temp = GetInventoryItem(_item);

        if (temp != null)
        {
            item = temp;
            return true;
        }
        else
            Debug.LogError("아이템이 없습니다");

        item = null;
        return false;
    }

    // 장비 아이템 선택
    //public void SelectEquipment(int _slot)
    //{
    //    if (currentEquipmentSlot == _slot) return;

    //    currentEquipmentSlot = _slot;
    //    OnChangeEquipment?.Invoke();
    //}

    // 장비 아이템 선택
    public void ChangeEquipment()
    {
        currentEquipmentSlot = (currentEquipmentSlot + 1) % (MaxEquipmentSlots + 1);

        if (currentEquipmentSlot >= MaxEquipmentSlots)
            currentEquipmentSlot = 0;

        OnChangeEquipment?.Invoke(currentEquipmentSlot);
        Debug.Log($"장비 슬롯 변경 {currentEquipmentSlot}");
    }

    // 장비 아이템 선택
    public void ChangeBuilding()
    {
        currentBuildingSlot = (currentBuildingSlot + 1) % (MaxBuildingSlots + 1);

        if (currentBuildingSlot >= MaxBuildingSlots)
            currentBuildingSlot = 0;

        OnChangeBuilding?.Invoke(currentBuildingSlot);
        Debug.Log($"건물 슬롯 변경 {currentBuildingSlot}");
    }


    // 아이템 레시피 추가
    public void AddItemRecipe(ItemRecipe _recipe)
    {
        recipes.Add(_recipe);
    }
}
