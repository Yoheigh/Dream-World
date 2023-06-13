using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuList : UIPanel
{
    // 인덱스 지 맘대로 넣었음
    public override void Init()
    {
        for(int i = 0; i < base.Buttons.Length; i++)
        {
            int index = i;
            Buttons[index].onClick.AddListener(() => Manager.Instance.UI.ShowPanel(index + 1));
        }
    }
}
