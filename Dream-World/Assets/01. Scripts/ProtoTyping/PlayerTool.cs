using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTool : MonoBehaviour
{
    IngredientObjectType toolEffectiveType = IngredientObjectType.Wood;

    public GameObject Axe;
    public GameObject Pickaxe;
    public GameObject Shovel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ChangeTool(IngredientObjectType _toolEffectiveType)
    {
        if (toolEffectiveType == _toolEffectiveType)
            return;

        toolEffectiveType = _toolEffectiveType;
        switch (toolEffectiveType)
        {
            case IngredientObjectType.Wood:
                Axe.SetActive(true);
                Pickaxe.SetActive(false);
                Shovel.SetActive(false);
                break;
            case IngredientObjectType.Rock:
                Axe.SetActive(false);
                Pickaxe.SetActive(true);
                Shovel.SetActive(false);
                break;
            case IngredientObjectType.Dirt:
                Axe.SetActive(false);
                Pickaxe.SetActive(false);
                Shovel.SetActive(true);
                break;
        }
    }
}
