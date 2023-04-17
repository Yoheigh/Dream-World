using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerEquipmentType
{
    Melee = 0, Ranged, Special
}

public class PlayerEquipment : MonoBehaviour
{
    [Header("Player Equipment")]
    [SerializeField]
    protected PlayerEquipmentType equipmentType;

    //[SerializeField]
    //protected EquipmentData equipmentData;


}

public struct ToolData
{
    public int toolID;
    public string toolName;
    public float toolRange;
    public float toolActionSpeed;
    public float toolActionDelay;
}