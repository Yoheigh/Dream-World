using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystemManager : MonoBehaviour
{
    [SerializeField] private List<UIPanel> panels;
    [SerializeField] private UIPopup popupPrefabs;

    [SerializeField]
    private GameObject Canvas;
    private int currentPanelIndex;
    private Stack<UIPopup> popupStack = new Stack<UIPopup>();

    public void ShowPanel(int index)
    {
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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousPanel();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextPanel();
        }
    }
}
