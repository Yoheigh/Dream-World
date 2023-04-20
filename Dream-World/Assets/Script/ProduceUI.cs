using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceUI : MonoBehaviour
{
    public GameObject Target_EquipmentUI;
    public GameObject Target_StructureUI;

    void Update()
    {
        EnableProduceUI();
    }

    void EnableProduceUI()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Target_EquipmentUI.SetActive(!Target_EquipmentUI.activeSelf);
            Target_StructureUI.SetActive(!Target_StructureUI.activeSelf);
            /*
            if (!Target_EquipmentUI.activeSelf)
            {
                Target_EquipmentUI.SetActive(true);
            }
            if (Target_StructureUI.activeSelf)
            {
                Target_StructureUI.SetActive(false);
            }*/
        }
    }
}
