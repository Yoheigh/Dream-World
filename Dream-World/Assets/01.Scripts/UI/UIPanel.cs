using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class UIPanel : MonoBehaviour
{
    public Button[] Buttons;

    private void OnEnable()
    {
        ResetSelection();
    }

    public virtual void Init() { }

    public virtual void ResetSelection()
    {
        Buttons?[0].Select();
        Debug.Log($"{Buttons?[0].name} 버튼 활성화");
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
