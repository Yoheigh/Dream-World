using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 얘는 나중에 다시 보자 */
public class UIPopup : MonoBehaviour
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
