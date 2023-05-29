using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Manager Manager => Manager.Instance;

    [SerializeField]
    private PlayerController mainController;

    private StageData currentStageData;

    private bool isGameOver = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Manager.Inventory.AddItem(new ItemV2(100, ItemTypeV2.Ingredient, "³ª¹«"));
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Manager.Inventory.AddItem(new ItemV2(200, ItemTypeV2.Ingredient, "¹ÙÀ§"));
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Manager.Inventory.AddItem(new ItemV2(300, ItemTypeV2.Ingredient, "Ã¶"));
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Manager.Craft.CraftItemCheck(Manager.Craft.recipes[0]);
        }
    }
}
