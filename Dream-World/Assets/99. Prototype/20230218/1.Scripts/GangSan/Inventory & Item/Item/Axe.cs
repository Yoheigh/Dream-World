using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : PlayerEquipment , HandItem // ?
{
    IngredientObjectType effectiveType = IngredientObjectType.Wood;

    public override void InteractWithEquipment(IngredientObject obj)
    {
        obj.GetObjectType();
        if(obj.GetObjectType() == effectiveType)
        {
            obj.AffectedByEquipment();
        }
        else
        {
            Debug.Log("ȣȯ���� �ʴ� �����Դϴ�");
        }
    }

}

