using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemRecipe", menuName = "ItemRecipe", order = 0)]
public class ItemRecipe : ScriptableObject
{
    [Header("조합에 필요한 아이템")]
    public List<ItemV2> ingredients = new List<ItemV2>(3);

    [Header("완성 아이템")]
    public ItemV2 result = null;
}
