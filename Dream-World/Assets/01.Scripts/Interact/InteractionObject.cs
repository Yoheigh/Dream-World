using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Define;

[RequireComponent(typeof(Rigidbody))]
public abstract class InteractionObject : MonoBehaviour
{
    public abstract ObjectType ObjectType { get; }

    public abstract void InteractWithPlayer(PlayerController _player);
}