using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftSlot : MonoBehaviour
{
    InventoryV2 Inventory => Manager.Instance.Inventory;

    // 해당 레시피
    public ItemRecipe itemRecipe; 

    // 앞에서 보여줄 것들
    public GameObject[] slotView;
    public Image[] needImage;
    public Text[] needCount;

    public Button CreateButton;

    // 완성 아이템 아이콘인데 이거 레이아웃 바뀌면 확 바뀔 듯
    // public Image resultImage;

    // 내부 변수
    private string tempString;

    private void Start()
    {
        Draw();
    }

    public void Draw()
    {
        tempString = null;

        // 크래프트 슬롯 초기화
        for (int i = 0; i < needImage.Length; i++)
        {
            needImage[i].gameObject.SetActive(false);
            needCount[i].gameObject.SetActive(false);
        }
        Debug.Log("슬롯 초기화");

        for (int i = 0; i < itemRecipe.needItemCount; i++)
        {
            if (Inventory.GetInventoryItem(itemRecipe.ingredients[i]) != null)
            {
                needCount[i].text = $"{Inventory.GetInventoryItem(itemRecipe.ingredients[i]).itemCount} / {itemRecipe.ingredientCounts[i]}";
                needImage[i].sprite = itemRecipe.ingredients[i].itemIcon;
            }
            else
            {
                needCount[i].text = $"{0} / {itemRecipe.ingredientCounts[i]}";
                needImage[i].sprite = itemRecipe.ingredients[i].itemIcon;
                Debug.Log("아이템 없으니 0개");
            }

            needImage[i].gameObject.SetActive(true);
            needCount[i].gameObject.SetActive(true);
        }

        
        // resultImage.sprite = itemRecipe.result.itemIcon;
    }
}
