using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCube : IngredientObject
{
    public GameObject Water;
    protected override void Destruction()
    {
        Water.SetActive(true);

        base.Destruction();
    }
}
