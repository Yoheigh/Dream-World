using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerEquipmentType
{
    None = 0, Tool, Consumable
}

public abstract class PlayerEquipment : MonoBehaviour
{
    [Header("Player Equipment")]
    [SerializeField]
    protected PlayerEquipmentType equipmentType;

    // protected EquipmentData equipmentData;

    [SerializeField]
    private GameObject EquipmentPrefab;

    public abstract void InteractWithEquipment();

}

public struct ToolData
{
    public PlayerEquipmentType equipmentType;
    public int toolID;
    public string toolName;
    public float toolRange;
    public float toolActionSpeed;
    public float toolActionDelay;
}