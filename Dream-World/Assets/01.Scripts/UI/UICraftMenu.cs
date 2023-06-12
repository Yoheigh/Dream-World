using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICraftMenu : UIPanel
{
    InventoryV2 Inventory => Manager.Instance.Inventory;
    CraftSystem Craft => Manager.Instance.Craft;

    // static event
    public static Action OnCraftChange;

    // 해당 크래프트 슬롯
    public CraftSlot selectedSlot;

    // 필요한 재료 개수와 아이콘 보여주는 UI
    public GameObject[] slotView;
    public Image[] needImage;
    public Text[] needCount;

    // 완성 아이템 아이콘인데 이거 레이아웃 바뀌면 확 바뀔 듯
    // public Image resultImage;

    // 내부 변수
    private string tempString;
    private bool isSelect;

    public override void ResetSelection()
    {
        selectedSlot.Button.Select();
    }

    private void Start()
    {
        selectedSlot.Button.Select();
    }

    public void Draw()
    {
        tempString = null;
        var itemRecipe = selectedSlot.itemRecipe;

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

    public void test()
    {
        if (Craft.CraftItemCheck(selectedSlot.itemRecipe))
            Debug.Log("제작 가능~");
        else
            Debug.Log("제작☆불가능~");
    }

    public void CraftItem()
    {
        Craft.CraftItem(selectedSlot.itemRecipe);
    }

}
