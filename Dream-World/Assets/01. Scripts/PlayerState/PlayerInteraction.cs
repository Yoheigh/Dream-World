using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Rendering.PostProcessing;

public class PlayerInteraction : MonoBehaviour
{
    [Header("PlayerInteraction")]
    [Tooltip("접촉한 오브젝트와 자동으로 상호작용하기 까지 TriggerTime 초 걸립니다.")]
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

    [SerializeField]
    private Transform BlockPointer;
    private Vector3 targetBlockPos;

    [SerializeField]
    private GameObject previewPrefab;
    //private Vector3 targetBlockOriginPos = new Vector3(0, 0, 1.1f);

    private PlayerEquipment currentEquipment;

    public Collider obj;

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
        if(obj != null)
        {
            if(obj.CompareTag("Dragable"))
            { 
                isInteracting = true;
                move.ChangeMoveState(PlayerStateType.Dragging);
            }
            return;
        }

        if (fov.ClosestInteractTransform != null)
        {
            if (fov.ClosestInteractTransform.CompareTag("Item"))
                Debug.Log(fov.ClosestTransform);

            //else if (fov.ClosestInteractTransform.CompareTag("Dragable"))
            //{
            //        isInteracting = true;
            //        move.ChangeMoveState(PlayerStateType.Dragging);
            //}
        }
    }

    public void InteractWithEquipment()
    {
        currentEquipment.InteractWithEquipment(fov.ClosestTransform.GetComponent<IngredientObject>());
        PlayerInteractAnimation();
    }

    public void PlayerInteractAnimation()
    {
        if (currentEquipment.name == "Axe")
            move.ChangeAnimationState("Player_Action_Axe");
        else if (currentEquipment.name == "Pickaxe")
            move.ChangeAnimationState("Player_Action_Pickaxe");
        else if (currentEquipment.name == "Shovel")
            move.ChangeAnimationState("Player_Action_Shovel");

    }

    public void ChangeEquipment(PlayerEquipment _newEquipment)
    {
        currentEquipment.gameObject.SetActive(false);
        currentEquipment = _newEquipment;
        currentEquipment.gameObject.SetActive(true);
    }

    private void Update()
    {
        Debug.Log(GetClosestBlockData().blockID);

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
    
    private BlockData GetClosestBlockData()
    {
        targetBlockPos = BlockPointer.position;
        int x = Mathf.FloorToInt(targetBlockPos.x);
        int y = Mathf.FloorToInt(targetBlockPos.y);
        int z = Mathf.FloorToInt(targetBlockPos.z);

        //Debug.Log(new Vector3(x, y, z));

        return GridSystem.Instance.StageGrid.GetGridObject(targetBlockPos);
    }

    private void SnapPlayerPos()
    {
        var ladder = obj.transform.GetComponent<Ladder>();
        move.SetVerticalPoint(ladder.Pivot, ladder.ReachHeight);
        move.ChangeMoveState(PlayerStateType.Climbing);
        obj = null;
    }

    private void OnTriggerStay(Collider col)
    {
        if (isInteracting) return;

        obj = col;

        if (col.CompareTag("Ladder"))
        {
            triggerTime = input.move.magnitude > 0.01f ? triggerTime - Time.deltaTime : TriggerTime;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Ladder"))
        {
            triggerTime = TriggerTime;
        }
    }

    //public void StopUseItem()
    //{
    //    CraftingTable.instance.SlotOutLineRedrow(-1);

    //    List<GameObject> childs = new List<GameObject>();
    //    for (int j = 0; j < gameObject.transform.Find("HandItems").childCount; j++)
    //    {
    //        childs.Add(gameObject.transform.Find("HandItems").GetChild(j).gameObject);
    //    }

    //    foreach (GameObject gameObject_ in childs)
    //    {
    //        Destroy(gameObject_);
    //    }
    //}

    //public void UseItem(int itemSlotCount)
    //{
    //    StopUseItem();

    //    int itemID = CraftingTable.instance.SlotOutLineRedrow(itemSlotCount);

    //    foreach (Item item in Inventory.instance.equipmentItems)
    //    {
    //        if (item.itemID == itemID)
    //        {
    //            Instantiate(Resources.Load<GameObject>(item.itemGameObjectPath), gameObject.transform.Find("HandItems"));
    //        }
    //    }
    //}

}
