using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystemManager : MonoSingleton<UISystemManager>
{
    [SerializeField] private List<UIPanel> panels;
    [SerializeField] private List<UIPanel> popupPrefabs;    // 잠시 판넬을 팝업으로 씁시다
    // [SerializeField] private UIPopup popupPrefabs;

    public ScreenTransition Transition;                     // 화면 전환
    public GameObject SystemUI;                             // SystemUI 부모 객체
    public GameObject PlayerUI;                             // PlayerUI 부모 객체

    // 각각 별개의 슬롯
    public List<ItemSlot> ingredientSlots;
    public List<ItemSlot> EquipmentSlots;
    public List<ItemSlot> BuildingSlots;

    public HealthUI HP;
    public GameObject VerticalBar;

    [SerializeField]
    private GameObject Canvas;
    private int currentPanelIndex;
    private Stack<UIPopup> popupStack = new Stack<UIPopup>();
    private Stack<UIPanel> panelStack = new Stack<UIPanel>();

    public bool isActivateUI;

    //public void ClosePopup()
    //{
    //    if (popupStack.Count == 0)
    //        return;
    //}

    //public void ShowPopup()
    //{

    //}

    // 소유중인 UIPanel들 값 초기화
    public void Setup()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].Init();
        }

        Canvas.SetActive(true);
        VerticalBar.SetActive(false);
        currentPanelIndex = 0;
        HP.Draw(3);
        ShowPanel(currentPanelIndex);
        isActivateUI = true;
        ClosePanel();
    }

    public void ShowPanel(int index)
    {
        if (Canvas.activeSelf == false)
        {
            Canvas.SetActive(true);
            Managers.Sound.PlaySFX(102);
            isActivateUI = true;
        }

        if (currentPanelIndex == index)
            return;

        currentPanelIndex = index;
        for (int i = 0; i < panels.Count; i++)
        {
            if (i == index)
            {
                panels[i].Show();
                Debug.Log((currentPanelIndex + 1) % panels.Count);
            }
            else
            {
                panels[i].Hide();
            }
        }
    }

    public void AddPanelPopup(int index)
    {
        if (panelStack.Contains(popupPrefabs[index])) return;

        if (panelStack.Count == 0)
            panels[currentPanelIndex].Hide();
        else if (panelStack.Count > 1)
            panelStack.Peek().Hide();

        panelStack.Push(popupPrefabs[index]);
        panelStack.Peek().Show();
    }

    public void ClosePanel()
    {
        // 켜져있는 Popup이 하나 이상이면 실행
        if (panelStack.Count > 0)
        {
            Debug.Log($"panelStack 개수 : {panelStack.Count}");
            // 최상위 오브젝트 Hide() 시키고 Stack에서 제외
            panelStack.Pop().Hide();

            // 스택에 다음 UI가 존재할 경우 Show()
            if (panelStack.Count > 0)
            {
                panelStack.Peek().Show();
                return;
            }

            // 스택에 값이 없을 경우 Panel 실행
            if (panelStack.Count == 0)
            {
                panels[currentPanelIndex].Show();
            }
        }
        else if (panelStack.Count == 0 && isActivateUI == true)
        {
            panels?[currentPanelIndex].Hide();
            currentPanelIndex = -1;
            Canvas.SetActive(false);
            isActivateUI = false;
        }
    }

    public void NextPanel()
    {
        currentPanelIndex = (currentPanelIndex + 1) % panels.Count;
        ShowPanel(currentPanelIndex);
    }

    public void PreviousPanel()
    {
        currentPanelIndex--;
        if (currentPanelIndex < 0)
        {
            currentPanelIndex = panels.Count - 1;
        }
        ShowPanel(currentPanelIndex);
    }

    public void CloseAll()
    {
        // UIPopup 먼저 전부 종료
        if (panelStack.Count > 0)
        {
            for (int i = 0; i < panelStack.Count; i++)
            {
                ClosePanel();
            }
        }

        if (currentPanelIndex > 0)
        {
            ClosePanel();
            currentPanelIndex = -1;
        }
        else
            return;
    }

    // 다른 곳에서 매니저 접근해서 가져오는 걸 리팩토링하도록 하자
    public void DrawItemSlots()
    {
        // 재료 슬롯 초기화
        for(int i = 0; i < ingredientSlots.Count; i++)
        {
            ingredientSlots[i].Init();
        }
        for (int i = 0; i < Managers.Inventory.ingredients.Count; i++)
        {
            Debug.Log($"{i}번째 자원 슬롯 Draw");
            ingredientSlots[i].DrawWithCount(Managers.Inventory.ingredients[i]);
        }

        // 장비 슬롯 초기화
        for (int i = 0; i < EquipmentSlots.Count; i++)
        {
            EquipmentSlots[i].Init();
        }
        for (int i = 0; i < Managers.Inventory.equipments.Count; i++)
        {
            Debug.Log($"{i}번째 장비 슬롯 Draw");
            EquipmentSlots[i].Draw(Managers.Inventory.equipments[i]);
        }

        //건물 슬롯 초기화
        for (int i = 0; i < BuildingSlots.Count; i++)
        {
            BuildingSlots[i].Init();
        }
        for (int i = 0; i < Managers.Inventory.buildings.Count; i++)
        {
            BuildingSlots[i].Draw(Managers.Inventory.buildings[i]);
        }
    }

    public void ActivateEquipSlot(int index)
    {
        for(int i = 0; i < EquipmentSlots.Count; i++)
        {
            if (i == index)
            {
                EquipmentSlots[i].DisableObj.SetActive(false);
                continue;
            }
            EquipmentSlots[i].DisableObj.SetActive(true);
            Managers.Sound.PlaySFX(101);
        }
    }

    public void ActivateBuildSlot(int index)
    {
        for (int i = 0; i < BuildingSlots.Count; i++)
        {
            if (i == index)
            {
                BuildingSlots[i].DisableObj.SetActive(false);
                continue;
            }
            BuildingSlots[i].DisableObj.SetActive(true);
            Managers.Sound.PlaySFX(101);
        }
    }
}
