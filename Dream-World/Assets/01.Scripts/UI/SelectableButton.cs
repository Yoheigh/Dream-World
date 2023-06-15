using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableButton : Button
{
    public Action callback;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        callback?.Invoke();
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        //callback?.Invoke();
    }

    public void SetCallback(Action _callback)
    {
        callback = new Action(_callback);
    }
}
