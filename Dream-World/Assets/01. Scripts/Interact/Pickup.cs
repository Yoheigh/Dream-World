using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : InteractionObject
{
    public override void InteractWithPlayer()
    {
        objectType = ObjectType.Pickup;
        // 아이템 구현

        Manager.Instance.Inventory.AddItem(new Wood());
        Destroy(gameObject);
    }
}

public class Wood : ItemV2
{
    public Wood()
    {
        itemID = 100;
        itemType = ItemTypeV2.Ingredient;
        itemName = "나무";
    }
}
