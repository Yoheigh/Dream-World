using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : InteractionObject
{
    public ItemV2 item;

    public override ObjectType ObjectType { get { return ObjectType.Pickup; } }

    public override void InteractWithPlayer(PlayerController _player)
    {
        // 아이템 구현
        Manager.Instance.Inventory.AddItem(item);
        Destroy(gameObject);
    }
}

//public class Wood : ItemV2
//{
//    public Wood()
//    {
//        itemID = 100;
//        itemType = ItemTypeV2.Ingredient;
//        itemName = "나무";
//    }
//}
