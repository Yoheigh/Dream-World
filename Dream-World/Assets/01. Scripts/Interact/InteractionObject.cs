using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ObjectType : sbyte
{
    Pickup,         // 1회성 줍는 오브젝트
    Grabable,       // 들고 다닐 수 있는 오브젝트 
    Dragable,       // 끌어다닐 수 있는 오브젝트
    StageObject,    // 상호작용으로 작동하는 오브젝트 ( 예시 : 버튼, 상자 )
}

[RequireComponent(typeof(Rigidbody))]
public abstract class InteractionObject : MonoBehaviour
{
    public abstract ObjectType ObjectType { get; }

    public abstract void InteractWithPlayer(PlayerController _player);
}