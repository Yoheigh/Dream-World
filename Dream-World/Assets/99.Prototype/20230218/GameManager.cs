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
            Manager.Inventory.AddItem(new Ingredient(100, ItemTypeV2.Ingredient, "����"));
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Manager.Inventory.AddItem(new Ingredient(200, ItemTypeV2.Ingredient, "����"));
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Manager.Inventory.AddItem(new Ingredient(300, ItemTypeV2.Ingredient, "ö"));
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Manager.Craft.CraftItemCheck(Manager.Craft.recipes[0]);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Manager.Inventory.RemoveItem(new Ingredient(100, ItemTypeV2.Ingredient, "����"));
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Manager.Inventory.ChangeEquipment();
        }
    }
}
