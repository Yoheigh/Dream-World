using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.VFX;
using Unity.VisualScripting;
using Unity.Collections.LowLevel.Unsafe;

public class PlayerInteraction : MonoBehaviour
{
    //[Header("PlayerInteraction")]
    //[Tooltip("접촉한 오브젝트와 자동으로 상호작용하기 까지 TriggerTime 초 걸립니다.")]
    //public float TriggerTime = 0.3f;

    public Action<InteractionObject> OnTargetChange;
    public Action<PlayerEquipment> OnEquipmentChange;

    [SerializeField]
    private bool isInteracting = false;
    public bool IsInteracting
    {
        get => isInteracting;
        set => isInteracting = value;
    }


    [SerializeField]
    private Transform BlockPointer;

    private FOVSystem fov;
    private PlayerController controller;

    // 내부 변수
    private bool isHolding = false;
    private Transform holdingObject;
    // private float t;           // Lerp 같은 거 할 때 범용적으로 쓸 변수
    private Equipment currentEquipment;   // 현재 장착한 장비
    private WaitForSeconds equipWaitTime;

    // Start is called before the first frame update
    public void Setup()
    {
        fov = GetComponent<FOVSystem>();
        controller = GetComponent<PlayerController>();

        Debug.Log($"3. Setup - {this}");
    }

    public void Interact()
    {
        // 손에 무언가 들고 있는 상태일 경우 처리
        if (isHolding)
        {
            // 던지기 코드 처리
            /* 지금은 그냥 보여주기용 */
            StartCoroutine(ObjectMoveToGrid(holdingObject));
            isHolding = false;


            Debug.Log("그런 위험한 건 내려놓고 얘기하자구");
            return;
        }

        // 인벤토리에서 오브젝트를 선택해서 사용한다면
        // 이것도 isHolding에서 처리
        // 해당 상태를 소모
        // return;

        // 근처에 상호작용 가능한 오브젝트가 없을 경우 종료
        if (!NearObjectCheck()) return;

        var temp = fov.ClosestTransform.GetComponent<InteractionObject>();

        // 오브젝트의 인터랙션 기능 활성화
        temp.InteractWithPlayer();

        // 오브젝트의 enum에 따라서 플레이어 상태 변경
        switch (temp.ObjectType)
        {
            case ObjectType.Grabable:
                isHolding = true;
                holdingObject = temp.transform;
                StartCoroutine(ObjectMoveToOverhead(holdingObject));
                break;

            case ObjectType.Dragable:
                // 질질 끌고 가는 거
                break;

            case ObjectType.Pickup:
                StartCoroutine(PickUpDelay());
                break;

            case ObjectType.StageObject:
                // 뭔가 작동시키는 애니메이션이 나오는 기믹
                break;
        }
    }

    // 인터랙션이 가능한지 체크하는 함수
    // 선택된 오브젝트가 바뀔 때 이벤트를 호출할 수도 있음
    // 현재는 예외 없이 근처에 상호작용 블럭이 있는지만 체크
    public bool NearObjectCheck()
    {
        if (fov.ClosestTransform != null)
        {
            return true;
        }
        return false;
    }

    // 도구 인터랙션
    public void InteractWithEquipment()
    {
        if (currentEquipment == null) return;

        StartCoroutine(EquipmentActionDelay());
        currentEquipment.Action();
    }

    // 장비에 명시된 
    private IEnumerator EquipmentActionDelay()
    {
        controller.ChangeState(PlayerStateType.Interaction);
        yield return equipWaitTime = new WaitForSeconds(currentEquipment.EquipAnimationDelay);
        controller.ChangeState(PlayerStateType.Default);
    }

    // 가까이 있는 오브젝트가 바뀔 때마다 작동시킬 Action
    /* 지금은 함수 등록도, 사용도 안 함 */
    public void UpdateInteractChange()
    {
        var temp = fov.ClosestTransform;

        if (temp != fov.ClosestTransform)
        {
            OnTargetChange?.Invoke(fov.ClosestTransform.GetComponent<InteractionObject>());
        }
    }

    // 플레이어가 뭐 들면 머리 위로 이동시키는 코루틴
    // isHolding 상태가 해소되면 알아서 꺼짐
    IEnumerator ObjectMoveToOverhead(Transform _object)
    {
        while (isHolding)
        {
            var end = transform.position + Vector3.up * 1.5f;
            _object.transform.position = Vector3.Lerp(_object.transform.position, end, Time.deltaTime * 10f);
            yield return null;
        }
    }

    // 플레이어가 들고있던 걸 Grid로 옮기는 코루틴
    /* Lerp 그래프를 코루틴으로 사용해보고 싶어서 만들어본거라 사용 안 할 듯 */
    IEnumerator ObjectMoveToGrid(Transform _object, float _lerpTime = 1.0f)
    {
        Vector3 targetPosition = Vector3.one;

        // Sine 그래프를 따라 graphValue의 값을 조정
        GraphSine(_lerpTime, (obj) =>
            {
                // 실시간으로 바뀌는 graphValue의 값을 코루틴 2개를 돌려 Lerp에 사용
                _object.transform.position = Vector3.Lerp(_object.transform.position, targetPosition, obj);
            });
        yield return new WaitForSeconds(_lerpTime);

        var obj = _object.GetComponent<InteractionObject>() as Grabable;
        obj.Init();
    }

    IEnumerator PickUpDelay()
    {
        controller.ChangeState(PlayerStateType.Interaction);
        controller.anim.ChangeAnimationState("Player_Action_Pickup");
        yield return new WaitForSeconds(0.8f);
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

    // Sine 함수 ( 점차 증가하는 그래프 )
    public void GraphSine(float lerpTime, Action<float> callback = null)
    {
        StartCoroutine(GraphSineCoroutine(lerpTime, callback));
    }

    private IEnumerator GraphSineCoroutine(float lerpTime, Action<float> callback = null)
    {
        float currentTime = 0f;
        float t;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= lerpTime)
                currentTime = lerpTime;

            t = currentTime / lerpTime;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);

            // 콜백 실행
            callback?.Invoke(t);
            yield return null;
        }

    }
}
