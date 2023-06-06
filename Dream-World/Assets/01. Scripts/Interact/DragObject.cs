using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : InteractionObject
{
    public override ObjectType ObjectType => ObjectType.Dragable;

    public override void InteractWithPlayer()
    {
        Debug.Log("으앙 잡힘");
    }
}
