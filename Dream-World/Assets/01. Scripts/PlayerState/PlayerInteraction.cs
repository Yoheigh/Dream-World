using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.VFX;

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

    private bool isConstructionMode = false;

    [SerializeField]
    private Transform BlockPointer;
    private Vector3 targetBlockPos;

    [SerializeField]
    private ConstructurePreview previewPrefab;
    //private Vector3 targetBlockOriginPos = new Vector3(0, 0, 1.1f);

    public GameObject[] EquipmentModel;
    public GameObject DestructVFX;

    public Collider obj;
    public Transform dragableObj;
    public Collider ladderObj;

    private CustomInput input;
    private FOVSystem fov;
    private PlayerMovement move;

    public int equipmentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<CustomInput>();
        fov = GetComponent<FOVSystem>();
        move = GetComponent<PlayerMovement>();
        triggerTime = TriggerTime;
    }

    IEnumerator PickUpDelay(Transform transform)
    {
        move.ChangeMoveState(PlayerStateType.Interaction);
        move.ChangeAnimationState("Player_Action_PickUp");
        yield return new WaitForSeconds(0.5f);
        Destroy(transform.gameObject);
        move.ChangeMoveState(PlayerStateType.Default);
    }

    public void Interact()
    {
        if (fov.ClosestInteractTransform != null)
        {
            if (fov.ClosestInteractTransform.CompareTag("Item"))
            {
                StartCoroutine(PickUpDelay(fov.ClosestInteractTransform));
                return;
            }

            //else if (fov.ClosestInteractTransform.CompareTag("Dragable"))
            //{
            //        isInteracting = true;
            //        move.ChangeMoveState(PlayerStateType.Dragging);
            //}
        }

        if (isConstructionMode)
        {
            previewPrefab.Construct();
            isConstructionMode = !isConstructionMode;
            return;
        }
        else if (obj == null)
        {
            isConstructionMode = !isConstructionMode;
            previewPrefab.gameObject.SetActive(isConstructionMode);
            return;
        }
        if (obj.CompareTag("Dragable"))
        {
            isInteracting = true;
            dragableObj = obj.transform;
            move.ChangeMoveState(PlayerStateType.Dragging);
            return;
        }
    }

    public void InteractWithEquipment()
    {
        StartCoroutine(InteractAnimationDelay());
        // Physics.SphereCast()
    }

    private IEnumerator InteractAnimationDelay()
    {
        EquipmentModel[equipmentIndex].SetActive(true);
        move.ChangeMoveState(PlayerStateType.Interaction);
        switch (equipmentIndex)
        {
            case 0:
                move.ChangeAnimationState("Player_Action_Axe");
                var colliders = Physics.OverlapSphere(transform.position, 0.8f);
                foreach (Collider col in colliders)
                {
                    if (col.CompareTag("Ingredient"))
                    {
                        var ingredient = col.GetComponent<IngredientObject>();
                        if ((int)ingredient.GetObjectType() == equipmentIndex)
                        {
                            var vfx = Instantiate(DestructVFX, BlockPointer.position, Quaternion.identity, null);
                            Destroy(vfx, 4f);
                            yield return new WaitForSeconds(0.583f);
                            move.ChangeAnimationState("Player_Action_Axe");
                            var vfx2 = Instantiate(DestructVFX, BlockPointer.position, Quaternion.identity, null);
                            Destroy(vfx2, 4f);
                            yield return new WaitForSeconds(0.583f);
                            
                            ingredient.AffectedByEquipment();
                        }
                    }
                }
                break;
            case 1:
                move.ChangeAnimationState("Player_Action_Pickaxe");
                yield return new WaitForSeconds(0.3f);
                var colliders2 = Physics.OverlapSphere(transform.position, 0.8f);
                foreach (Collider col in colliders2)
                {
                    if (col.CompareTag("Ingredient"))
                    {
                        var ingredient = col.GetComponent<IngredientObject>();
                        if ((int)ingredient.GetObjectType() == equipmentIndex)
                        {
                            ingredient.AffectedByEquipment();
                            var vfx = Instantiate(DestructVFX, BlockPointer.position, Quaternion.identity, null);
                            Destroy(vfx, 4f);
                        }
                    }
                }
                break;
            case 2:
                move.ChangeAnimationState("Player_Action_Shovel");
                yield return new WaitForSeconds(0.4f);
                var ray = Physics.Raycast(transform.position, Vector3.down, LayerMask.GetMask("Block"));
                    //if (ray.CompareTag("Ingredient"))
                    //{
                    //    var ingredient = col.GetComponent<IngredientObject>();
                    //    if ((int)ingredient.GetObjectType() == equipmentIndex)
                    //    {
                    //        ingredient.AffectedByEquipment();
                    //        var vfx = Instantiate(DestructVFX, BlockPointer.position, Quaternion.identity, null);
                    //        Destroy(vfx, 4f);
                    //    }
                    //}
                break;
        }

        yield return new WaitForSeconds(0.3f);

        EquipmentModel[equipmentIndex].SetActive(false);
        move.ChangeMoveState(PlayerStateType.Default);
    }

    //public void ChangeEquipment(PlayerEquipment _newEquipment)
    //{
    //    currentEquipment.gameObject.SetActive(false);
    //    currentEquipment = _newEquipment;
    //    currentEquipment.gameObject.SetActive(true);
    //}

    private void Update()
    {
        targetBlockPos = BlockPointer.transform.position;

        if (isInteracting) return;

        if (triggerTime <= 0.0f)
        {
            if (obj != null)
            {
                isInteracting = true;
                SnapPlayerPos();
                triggerTime = TriggerTime;
            }
        }

        // 가장 근처 블럭의 위치에 건축물을 보여주는 함수
        //previewPrefab.transform.position = GetPreviewPosition();
    }

    //private BlockData GetClosestBlockData()
    //{
    //    //Debug.Log(new Vector3(x, y, z));
    //    return GridSystem.Instance.StageGrid.GetGridObject(targetBlockPos);
    //}

    //private Vector3 GetPreviewPosition()
    //{
    //    int x, y, z;
    //    //가장 근처의 x, y, z 값을 반환해주는 함수였던 것
    //    //GridSystem.Instance.StageGrid.GetXYZRound(targetBlockPos, out x, out y, out z);
    //    //return new Vector3(x, y, z);
    //}

    private void SnapPlayerPos()
    {
        var ladder = ladderObj.transform.GetComponent<Ladder>();
        move.SetVerticalPoint(ladder.Pivot, ladder.ReachHeight);
        ladderObj = null;
        move.ChangeMoveState(PlayerStateType.Climbing);
    }

    private void OnTriggerStay(Collider col)
    {
        if (isInteracting) return;

        obj = col;

        if (col.CompareTag("Ladder"))
        {
            triggerTime = input.move.magnitude > 0.01f ? triggerTime - Time.deltaTime : TriggerTime;
            ladderObj = col;
            return;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Ladder"))
        {
            triggerTime = TriggerTime;
        }

        obj = null;
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
