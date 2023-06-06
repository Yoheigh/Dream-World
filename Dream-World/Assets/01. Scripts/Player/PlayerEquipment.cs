using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerEquipmentType
{
    Tool = 0, Consumable
}

public abstract class PlayerEquipment : MonoBehaviour
{
    [Header("Player Equipment")]
    [SerializeField]
    protected ToolData toolData;

    [SerializeField]
    private Vector3 originPos;

    [SerializeField]
    private Vector3 originRot;

    [SerializeField]
    private GameObject EquipmentPrefab;

    private void OnEnable()
    {
        EquipmentPrefab.SetActive(true);
        transform.localPosition = originPos;
        transform.rotation = Quaternion.Euler(originRot);
        Debug.Log("장비 변경");
    }

    private void OnDisable()
    {
        EquipmentPrefab.SetActive(false);
    }

    public abstract void InteractWithEquipment(IngredientObject obj);

}

public struct ToolData
{
    public PlayerEquipmentType equipmentType;
    public EffectiveType toolEffectiveType;
    public int toolID;
    public string toolName;
    public string toolPrefabPath;
    public string toolAnimName;
    public float toolRange;
    public float toolActionSpeed;
    public float toolActionDelay;

    // public ToolData(int toolID, PlayerEquipmentType _equipmentType, string _toolName, string _toolPrefabPath)
}