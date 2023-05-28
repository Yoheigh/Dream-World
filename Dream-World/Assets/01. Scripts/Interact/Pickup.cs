using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : InteractionObject
{
    public override void InteractWithPlayer()
    {
        objectType = ObjectType.Pickup;
        Destroy(gameObject);
    }
}
