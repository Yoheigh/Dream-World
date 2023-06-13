using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class CraftSlot : MonoBehaviour
{
    // 해당 레시피
    public ItemRecipe itemRecipe; 

    // 완성 아이템 아이콘인데 이거 레이아웃 바뀌면 확 바뀔 듯
    public Sprite resultImage;
    public string resultDescription;

    public SelectableButton Button;

    public void Draw()
    {
        resultImage = itemRecipe.result.itemIcon;
        resultDescription = itemRecipe.result.itemDescription;
    }
}
