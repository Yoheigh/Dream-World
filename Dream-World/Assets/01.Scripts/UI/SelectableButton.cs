using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableButton : Button
{
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        Debug.Log($"{this.gameObject} 선택 안 됨");
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        Debug.Log($"{this.gameObject} 선택!!!!됨!!@!@!@!@!!!!!!!이다므읻마럼륻ㅁ");
    }
}
