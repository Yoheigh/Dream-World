using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class CraftSystem
{
    InventoryV2 Inventory => Managers.Inventory;

    // 보유 중인 아이템 조합법
    List<ItemRecipe> Recipes => Managers.Inventory.recipes;

    // 아이템 조합법에 따라 제작 가능 여부 체크
    public bool CraftItemCheck(ItemRecipe recipe)
    {
        int index = 0;
        int requireCheck = 0;

        Debug.Log($"{recipe.result.itemName}를 만들기 위한 재료를 검색합니다.");

        for(index = 0; index < recipe.ingredients.Length; index++)
        {
            var needItem = recipe.ingredients[index];
            var needItemCount = recipe.ingredientCounts[index];

            // 인벤토리에서 필요한 아이템 있는지 가져오기
            Inventory.GetInventoryItem(needItem.itemID, (obj) =>
            {
                Debug.Log($"{index + 1}번째 필요 아이템 : {needItem.itemCount}개 필요한 {obj.itemName}이(가) {obj.itemCount}만큼 있습니다.");
                
                // 가져온 아이템이 필요한 아이템의 개수와 같을 경우
                if (obj.itemCount >= needItemCount)
                {
                    // 필요한 아이템이 리턴될 경우 실행하는 콜백
                    requireCheck++;
                }
            });
        }

        // 필요한 아이템 개수만큼 성공적으로 체크했을 경우
        if (requireCheck == index)
        {
            Debug.Log("필요한 아이템이 전부 있습니다.");
            
            // 조합 가능
            return true;
        }
        else
            Debug.Log("아이템이 부족합니다.");

        return false;
    }

    // 아이템 제작
    public void CraftItem(ItemRecipe recipe)
    {
        if (CraftItemCheck(recipe) == true)
        {
            Inventory.AddItem(recipe.result);

            // 아이템 제거
            for(int i = 0; i < recipe.needItemCount; i++)
            {
                var newItem = new ItemV2(recipe.ingredients[i]);
                newItem.itemCount = recipe.ingredientCounts[i];
                Inventory.RemoveItem(newItem);
            }
        }
        else
            Debug.Log("재료가 부족합니다.");
    }

    // 현재 플레이어가 가지고 있는 모든 조합법 체크
    public void CraftItemCheckAll()
    {
        for(int i = 0; i < Recipes.Count; i++)
        {
            CraftItemCheck(Recipes[i]);
        }
    }
}
