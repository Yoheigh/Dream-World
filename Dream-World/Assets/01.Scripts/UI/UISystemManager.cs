using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystemManager : MonoSingleton<UISystemManager>
{
    [SerializeField] private List<UIPanel> panels;
    [SerializeField] private List<UIPanel> popupPrefabs;    // ��� �ǳ��� �˾����� ���ô�
    // [SerializeField] private UIPopup popupPrefabs;

    public ScreenTransition Transition;                     // ȭ�� ��ȯ
    public GameObject SystemUI;                             // SystemUI �θ� ��ü
    public GameObject PlayerUI;                             // PlayerUI �θ� ��ü

    // ���� ������ ����
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

    // �������� UIPanel�� �� �ʱ�ȭ
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
        // �����ִ� Popup�� �ϳ� �̻��̸� ����
        if (panelStack.Count > 0)
        {
            Debug.Log($"panelStack ���� : {panelStack.Count}");
            // �ֻ��� ������Ʈ Hide() ��Ű�� Stack���� ����
            panelStack.Pop().Hide();

            // ���ÿ� ���� UI�� ������ ��� Show()
            if (panelStack.Count > 0)
            {
                panelStack.Peek().Show();
                return;
            }

            // ���ÿ� ���� ���� ��� Panel ����
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
        // UIPopup ���� ���� ����
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

    // �ٸ� ������ �Ŵ��� �����ؼ� �������� �� �����丵�ϵ��� ����
    public void DrawItemSlots()
    {
        // ��� ���� �ʱ�ȭ
        for(int i = 0; i < ingredientSlots.Count; i++)
        {
            ingredientSlots[i].Init();
        }
        for (int i = 0; i < Managers.Inventory.ingredients.Count; i++)
        {
            Debug.Log($"{i}��° �ڿ� ���� Draw");
            ingredientSlots[i].DrawWithCount(Managers.Inventory.ingredients[i]);
        }

        // ��� ���� �ʱ�ȭ
        for (int i = 0; i < EquipmentSlots.Count; i++)
        {
            EquipmentSlots[i].Init();
        }
        for (int i = 0; i < Managers.Inventory.equipments.Count; i++)
        {
            Debug.Log($"{i}��° ��� ���� Draw");
            EquipmentSlots[i].Draw(Managers.Inventory.equipments[i]);
        }

        //�ǹ� ���� �ʱ�ȭ
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
