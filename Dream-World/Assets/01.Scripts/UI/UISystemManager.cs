using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystemManager : MonoBehaviour
{
    [SerializeField] private List<UIPanel> panels;
    [SerializeField] private List<UIPanel> popupPrefabs;    // 잠시 판넬을 팝업으로 씁시다
    // [SerializeField] private UIPopup popupPrefabs;

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
        for(int i = 0; i < panels.Count; i++)
        {
            panels[i].Init();
        }
    }

    public void ShowPanel(int index)
    {
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
        if (!panelStack.Contains(popupPrefabs[index]))
        {
            panelStack.Push(popupPrefabs[index]);
            panelStack.Peek().Show();
        }
    }

    public void ClosePanel()
    {
        if (panelStack.Count > 0)
        {
            panelStack.Pop().Hide();
            panelStack?.Peek().Show();

            if (panelStack.Count == 0)
            {
                panels[currentPanelIndex].Show();
            }
        }
        else
        {
            panels[currentPanelIndex].Hide();
            currentPanelIndex = -1;
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
        if(panelStack.Count > 0)
        {
            for (int i = 0; i < panelStack.Count; i++)
            {
                panelStack.Pop();
            }
        }
        
        if(currentPanelIndex > 0)
        {
            panels[currentPanelIndex].Hide();
            currentPanelIndex = -1;
        }
    }
}
