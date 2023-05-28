using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Pickup,
    Grabable,
    StageObject
}

public abstract class InteractionObject : MonoBehaviour
{
    // 인터랙션 오브젝트에 필요한 게 뭐가 있을까
    public ObjectType objectType;

    public abstract void InteractWithPlayer();
}
