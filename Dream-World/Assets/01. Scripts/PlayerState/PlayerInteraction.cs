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

    [SerializeField]
    private Transform BlockPointer;
    private Vector3 targetBlockPos;

    public GameObject[] EquipmentModel;
    public GameObject DestructVFX;

    public Collider obj;
    public Transform dragableObj;
    public Collider ladderObj;

    private FOVSystem fov;
    private PlayerController controller;

    public int equipmentIndex = 0;

    // Start is called before the first frame update
    public void Setup()
    {
        fov = GetComponent<FOVSystem>();
        controller = GetComponent<PlayerController>();
        triggerTime = TriggerTime;

        Debug.Log($"3. Setup - {this}");
    }

    public void Interact()
    {
        if (fov.ClosestInteractTransform != null)
        {
            string tag = fov.ClosestInteractTransform.tag;

            switch (tag)
            {
                case "Item":
                    StartCoroutine(PickUpDelay(fov.ClosestInteractTransform));
                    break;

                case "Dragable":
                    controller.ChangeState(PlayerStateType.Dragging);
                    break;

                case "Climbing":
                    controller.ChangeState(PlayerStateType.Climbing);
                    break;
            }
        }

        //if (isConstructionMode)
        //{
        //    previewPrefab.Construct();
        //    isConstructionMode = !isConstructionMode;
        //    return;
        //}
        //else if (obj == null)
        //{
        //    isConstructionMode = !isConstructionMode;
        //    previewPrefab.gameObject.SetActive(isConstructionMode);
        //    return;
        //}
        //if (obj.CompareTag("Dragable"))
        //{
        //    isInteracting = true;
        //    dragableObj = obj.transform;
        //    controller.ChangeMoveState(PlayerStateType.Dragging);
        //    return;
        //}
    }
    // 인터랙션이 가능한지 체크하는 함수
    public void InteractionCheck()
    {
        // 인터랙션이 가능함
    }

    public void InteractWithEquipment()
    {
        StartCoroutine(InteractAnimationDelay());
        // Physics.SphereCast()
    }

    IEnumerator PickUpDelay(Transform transform)
    {
        controller.ChangeState(PlayerStateType.Interaction);
        controller.anim.ChangeAnimationState("Player_Action_PickUp");
        yield return new WaitForSeconds(0.5f);
        Destroy(transform.gameObject);
        controller.ChangeState(PlayerStateType.Default);
    }

    // 개똥 시연용 스크립트
    private IEnumerator InteractAnimationDelay()
    {
        EquipmentModel[equipmentIndex].SetActive(true);
        controller.ChangeState(PlayerStateType.Interaction);
        switch (equipmentIndex)
        {
            case 0:
                controller.anim.ChangeAnimationState("Player_Action_Axe");
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
                            controller.anim.ChangeAnimationState("Player_Action_Axe");
                            var vfx2 = Instantiate(DestructVFX, BlockPointer.position, Quaternion.identity, null);
                            Destroy(vfx2, 4f);
                            yield return new WaitForSeconds(0.583f);
                            
                            ingredient.AffectedByEquipment();
                        }
                    }
                }
                break;
            case 1:
                controller.anim.ChangeAnimationState("Player_Action_Pickaxe");
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
                controller.anim.ChangeAnimationState("Player_Action_Shovel");
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
        controller.ChangeState(PlayerStateType.Default);
    }

    //public void ChangeEquipment(PlayerEquipment _newEquipment)
    //{
    //    currentEquipment.gameObject.SetActive(false);
    //    currentEquipment = _newEquipment;
    //    currentEquipment.gameObject.SetActive(true);
    //}

    //private BlockData GetClosestBlockData()
    //{
    //    //Debug.Log(new Vector3(x, y, z));
    //    return GridSystem.Instance.StageGrid.GetGridObject(targetBlockPos);
    //}

    //private void SnapPlayerPos()
    //{
    //    var ladder = ladderObj.transform.GetComponent<Ladder>();
    //    movement.SetVerticalPoint(ladder.Pivot, ladder.ReachHeight);
    //    ladderObj = null;
    //    controller.ChangeMoveState(PlayerStateType.Climbing);
    //}

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

    //}    //    StopUseItem();

    //    int itemID = CraftingTable.instance.SlotOutLineRedrow(itemSlotCount);

    //    foreach (Item item in Inventory.instance.equipmentItems)
    //    {
    //        if (item.itemID == itemID)
    //        {
    //            Instantiate(Resources.Load<GameObject>(item.itemGameObjectPath), gameObject.transform.Find("HandItems"));
    //        }
    //    }

}
