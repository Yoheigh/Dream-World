using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Pickup,         // 1회성 줍는 오브젝트
    Grabable,       // 들고 다닐 수 있는 오브젝트 
    Dragable,       // 끌어다닐 수 있는 오브젝트
    StageObject,    // 상호작용으로 작동하는 오브젝트 ( 예시 : 버튼, 상자 )
}

public abstract class InteractionObject : MonoBehaviour
{
    // 인터랙션 오브젝트에 필요한 게 뭐가 있을까
    public ObjectType objectType;

    public abstract void InteractWithPlayer();
}
