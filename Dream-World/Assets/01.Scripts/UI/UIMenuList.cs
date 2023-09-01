using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuList : UIPanel
{
    // �ε��� �� ����� �־���
    public override void Init()
    {
        for(int i = 0; i < base.Buttons.Length; i++)
        {
            int index = i;
            Buttons[index].onClick.AddListener(() => Managers.Instance.UI.AddPanelPopup(index));
        }
    }
}
