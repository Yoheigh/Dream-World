using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.VFX;

public class PlayerInteraction : MonoBehaviour
{
    //[Header("PlayerInteraction")]
    //[Tooltip("접촉한 오브젝트와 자동으로 상호작용하기 까지 TriggerTime 초 걸립니다.")]
    //public float TriggerTime = 0.3f;

    //[SerializeField]
    //private float triggerTime;

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

    private FOVSystem fov;
    private PlayerController controller;

    public int equipmentIndex = 0;

    // Start is called before the first frame update
    public void Setup()
    {
        fov = GetComponent<FOVSystem>();
        controller = GetComponent<PlayerController>();

        Debug.Log($"3. Setup - {this}");
    }

    public void Interact()
    {
        if (!InteractionCheck()) return;


    }

    // 인터랙션이 가능한지 체크하는 함수
    public bool InteractionCheck()
    {
        if (fov.ClosestTransform != null)
        {
            string tag = fov.ClosestTransform.tag;

            switch (tag)
            {
                case "Item":
                    StartCoroutine(PickUpDelay(fov.ClosestTransform));
                    break;

                case "Dragable":
                    controller.ChangeState(PlayerStateType.Dragging);
                    break;

                case "Climbing":
                    controller.ChangeState(PlayerStateType.Climbing);
                    break;
            }

            return true;
        }

        // 그렇지 않으면 x
        return false;
    }

    public void InteractWithEquipment()
    {

    }

    IEnumerator PickUpDelay(Transform transform)
    {
        controller.ChangeState(PlayerStateType.Interaction);
        controller.anim.ChangeAnimationState("Player_Action_PickUp");
        yield return new WaitForSeconds(0.5f);
        Destroy(transform.gameObject);
        controller.ChangeState(PlayerStateType.Default);
    }

    //private IEnumerator InteractAnimationDelay()
    //{
    //    인터랙션 state로 변경

    //    도구 모델 활성화
    //     현재 도구에 따른 애니메이션 재생
    //     도구에 따른 특수 효과 발동

    //    EquipmentModel[equipmentIndex].SetActive(true);
    //    controller.ChangeState(PlayerStateType.Interaction);
    //    switch (equipmentIndex)
    //    {
    //        case 0:
    //            controller.anim.ChangeAnimationState("Player_Action_Axe");
    //            var colliders = Physics.OverlapSphere(transform.position, 0.8f);
    //            foreach (Collider col in colliders)
    //            {
    //                if (col.CompareTag("Ingredient"))
    //                {
    //                    var ingredient = col.GetComponent<IngredientObject>();
    //                    if ((int)ingredient.GetObjectType() == equipmentIndex)
    //                    {
    //                        var vfx = Instantiate(DestructVFX, BlockPointer.position, Quaternion.identity, null);
    //                        Destroy(vfx, 4f);
    //                        yield return new WaitForSeconds(0.583f);
    //                        controller.anim.ChangeAnimationState("Player_Action_Axe");
    //                        var vfx2 = Instantiate(DestructVFX, BlockPointer.position, Quaternion.identity, null);
    //                        Destroy(vfx2, 4f);
    //                        yield return new WaitForSeconds(0.583f);

    //                        ingredient.AffectedByEquipment();
    //                    }
    //                }
    //            }
    //            break;
    //    }

    //    yield return new WaitForSeconds(0.3f);

    //    EquipmentModel[equipmentIndex].SetActive(false);
    //    controller.ChangeState(PlayerStateType.Default);
    //}

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
