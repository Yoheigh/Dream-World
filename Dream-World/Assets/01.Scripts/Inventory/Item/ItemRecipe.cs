using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemRecipe", menuName = "Item/ItemRecipe", order = 5)]
public class ItemRecipe : ScriptableObject
{
    [Header("조합에 필요한 아이템 개수")]
    [Range(1, 3)]
    public byte needItemCount;

    [Header("조합에 필요한 아이템 ( 위에서부터 1 ~ 3 )")]
    public ItemV2[] ingredients;

    [Header("필요한 아이템 개수 ( 위에서부터 1 ~ 3 )")]
    public int[] ingredientCounts;

    [Header("완성 아이템")]
    public ItemV2 result = null;

    // 에디터 내부 변수
    private readonly byte defaultItemCount = 1;
    private byte tempCount;

    // 레시피 수정 시 바로 적용
    private void OnValidate()
    {
        if (needItemCount != tempCount)
        {
            tempCount = needItemCount;

            ingredients = new ItemV2[needItemCount];
            ingredientCounts = new int[needItemCount];

            for (int i = 0; i < needItemCount; i++)

                if (ingredientCounts[i] == 0)
                    ingredientCounts[i] = defaultItemCount;
        }
    }
}
