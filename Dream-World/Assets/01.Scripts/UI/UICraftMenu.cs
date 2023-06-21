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

    // CraftSlot이 생성될 부모 오브젝트
    public Transform UIRoot;

    // CraftSlot 게임오브젝트
    public GameObject CraftSlotPrefab;

    // 해당 크래프트 슬롯
    public List<CraftSlot> craftSlots;

    // 필요한 재료 개수와 아이콘 보여주는 UI
    public GameObject[] slotView;
    public Image[] needImage;
    public Text[] needCount;

    public Image completeBar;

    public int currentIndex = -1;

    // 완성 아이템 아이콘인데 이거 레이아웃 바뀌면 확 바뀔 듯
    // public Image resultImage;

    // 내부 변수
    private string tempString;
    private bool isSelect;

    // 첫 번째 버튼 선택하고 화면 드로우
    private void OnEnable()
    {
        RedrawRecipes();

        for (int i = 0; i < craftSlots.Count; i++)
        {
            // 이렇게 안하면 C#의 클로저 현상 일어남
            int index = i;

            // 해당 버튼 Select 될 때마다 정보 업데이트
            craftSlots[i].Button.SetCallback(() =>
            {
                currentIndex = index;
                Draw();
            });
        }

        ResetSelection();
    }

    public void RedrawRecipes()
    {
        // 모든 CraftSlots 제거
        foreach(var item in craftSlots)
        {
            Destroy(item.gameObject);
        }

        craftSlots.Clear();

        for(int i = 0; i < Inventory.recipes.Count; i++)
        {
            GameObject newSlotObj = Instantiate(CraftSlotPrefab, UIRoot);
            newSlotObj.name = $"{Inventory.recipes[i].name}_CraftSlot";
            var newSlot = newSlotObj.GetComponent<CraftSlot>();

            craftSlots.Add(newSlot);

            newSlot.itemRecipe = Inventory.recipes[i];
            newSlot.Draw();
        }

        // 버튼 내비게이션 등록
        for (int i = 0; i < craftSlots.Count; i++)
        {
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.Explicit;

            if(i == 0)
            {
                nav.selectOnUp = craftSlots[craftSlots.Count - 1].Button;
                nav.selectOnDown = craftSlots[i + 1].Button;
            }
            else if(i == craftSlots.Count - 1)
            {
                nav.selectOnUp = craftSlots[i - 1].Button;
                nav.selectOnDown = craftSlots[0].Button;
            }
            else
            {
                nav.selectOnUp = craftSlots[i - 1].Button;
                nav.selectOnDown = craftSlots[i + 1].Button;
            }
            
            craftSlots[i].Button.navigation = nav;
            Debug.Log($"{i}번째 슬롯 완벽");
        }
    }

    public override void ResetSelection()
    {
        craftSlots[0].Button.Select();
        currentIndex = 0;
        Draw();
    }

    // 해당 아이템의 정보 가져오기
    public void Draw()
    {
        tempString = null;
        var itemRecipe = craftSlots[currentIndex].itemRecipe;

        // 크래프트 슬롯 초기화
        for (int i = 0; i < needImage.Length; i++)
        {
            needImage[i].gameObject.SetActive(false);
            needCount[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < itemRecipe.needItemCount; i++)
        {
            if (Inventory.GetInventoryItem(itemRecipe.ingredients[i]) != null)
            {
                needCount[i].color = Color.white;
                needCount[i].text = $"{Inventory.GetInventoryItem(itemRecipe.ingredients[i]).itemCount} / {itemRecipe.ingredientCounts[i]}";
                needImage[i].color = Color.white;
                needImage[i].sprite = itemRecipe.ingredients[i].itemIcon;
            }
            else
            {
                needCount[i].color = Color.red;
                needCount[i].text = $"{0} / {itemRecipe.ingredientCounts[i]}";
                needImage[i].color = Color.gray;
                needImage[i].sprite = itemRecipe.ingredients[i].itemIcon;
            }

            needImage[i].gameObject.SetActive(true);
            needCount[i].gameObject.SetActive(true);
        }

        // resultImage.sprite = itemRecipe.result.itemIcon;
    }

    public void CraftItem()
    {
        Craft.CraftItem(craftSlots[currentIndex].itemRecipe);
    }

}
