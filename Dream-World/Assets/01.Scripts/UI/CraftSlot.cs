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
    public Image resultImage;
    public Text resultName;
    public Text resultDescription;

    public SelectableButton Button;

    public void Draw()
    {
        resultImage.sprite = itemRecipe.result.itemIcon;
        resultName.text = itemRecipe.result.itemName;
        resultDescription.text = itemRecipe.result.itemDescription;
    }
}
