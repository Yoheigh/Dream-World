using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class CraftSystem
{
    InventoryV2 Inventory => Manager.Instance.Inventory;

    public List<ItemRecipe> recipes;

    public bool CraftItemCheck(ItemRecipe recipe)
    {
        int index = 0;
        int requireCheck = 0;

        Debug.Log($"{recipe}를 만들기 위한 재료를 검색합니다.");

        for(index = 0; index < recipe.ingredients.Count; index++)
        {
            var needItem = recipe.ingredients[index];

            // 인벤토리에서 필요한 아이템 있는지 가져오기
            Inventory.GetInventoryItem(needItem.itemID, (obj) =>
            {
                // 필요한 아이템이 리턴될 경우 실행하는 콜백
                Debug.Log($"{index + 1}번째 필요 아이템 : {needItem.itemCount}개 필요한 {obj.itemName}이 {obj.itemCount}만큼 있습니다.");
                requireCheck++;
            });
        }

        // 필요한 개수가 검색한 아이템 개수와 같으면 체크
        if (requireCheck == index)
        {
            Debug.Log("필요한 아이템이 전부 있습니다.");
            return true;
        }
        else
            Debug.Log("아이템이 부족합니다.");

        return false;
    }

    public void CraftItemCheckAll()
    {
        for(int i = 0; i < recipes.Count; i++)
        {
            CraftItemCheck(recipes[i]);
        }
    }
}
