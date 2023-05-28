using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemRecipe", menuName = "ItemRecipe", order = 0)]
public class ItemRecipe : ScriptableObject
{
    [Header("���տ� �ʿ��� ������")]
    public List<ItemV2> ingredients = new List<ItemV2>(3);

    [Header("�ϼ� ������")]
    public ItemV2 result = null;
}
