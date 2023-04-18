using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("PlayerInteraction")]
    [Tooltip("접촉한 오브젝트와 상호작용하기 까지 TriggerTime 초 걸립니다.")]
    public float TriggerTime = 0.3f;
    
    [SerializeField]
    private float triggerTime;

    [SerializeField]
    private bool isInteracting = false;
    public bool IsInteracting
    {
        get => isInteracting;
        set => isInteracting = value;
    }

    private PlayerEquipment currentEquipment;

    private Collider obj;

    private CustomInput input;
    private FOVSystem fov;
    private PlayerMovement move;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<CustomInput>();
        fov = GetComponent<FOVSystem>();
        move = GetComponent<PlayerMovement>();
        triggerTime = TriggerTime;
    }

    public void Interact()
    {
        if (fov.ClosestTransform != null)
        {
            Debug.Log(fov.ClosestTransform);
        }
            

        Debug.Log("인터랙션~");
    }

    public void InteractWithEquipment()
    {
        currentEquipment.InteractWithEquipment();
    }

    public void ChangeEquipment(PlayerEquipment _newEquipment)
    {
        currentEquipment = _newEquipment;
    }

    public void StopUseItem()
    {
        CraftingTable.instance.SlotOutLineRedrow(-1);

        List<GameObject> childs = new List<GameObject>();
        for (int j = 0; j < gameObject.transform.Find("HandItems").childCount; j++)
        {
            childs.Add(gameObject.transform.Find("HandItems").GetChild(j).gameObject);
        }

        foreach (GameObject gameObject_ in childs)
        {
            Destroy(gameObject_);
        }
    }

    public void UseItem(int itemSlotCount)
    {
        StopUseItem();

        int itemID = CraftingTable.instance.SlotOutLineRedrow(itemSlotCount);

        foreach (Item item in Inventory.instance.equipmentItems)
        {
            if (item.itemID == itemID)
            {
                Instantiate(Resources.Load<GameObject>(item.itemGameObjectPath), gameObject.transform.Find("HandItems"));
            }
        }
    }

    private void Update()
    {
        if (isInteracting) return;

        if (triggerTime <= 0.0f)
        {
            if (obj != null)
            {
                SnapPlayerPos();
                isInteracting = true;
                triggerTime = TriggerTime;
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (isInteracting) return;

        if (col.CompareTag("Ladder"))
        {
            triggerTime = input.move.magnitude > 0.01f ? triggerTime - Time.deltaTime : TriggerTime;
            obj = col;
        }
    }


    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Ladder"))
        {
            triggerTime = TriggerTime;
        }
    }

    private void SnapPlayerPos()
    {
        var ladder = obj.transform.GetComponent<Ladder>();
        move.SetVerticalPoint(ladder.Pivot, ladder.ReachHeight);
        move.ChangeMoveState(PlayerStateType.Climbing);
        obj = null;
    }
}
