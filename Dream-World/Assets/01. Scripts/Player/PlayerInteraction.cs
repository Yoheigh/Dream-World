using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.VFX;
using Unity.VisualScripting;

public class PlayerInteraction : MonoBehaviour
{
    //[Header("PlayerInteraction")]
    //[Tooltip("접촉한 오브젝트와 자동으로 상호작용하기 까지 TriggerTime 초 걸립니다.")]
    //public float TriggerTime = 0.3f;

    public Action<InteractionObject> OnInteractChange;
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

    public int equipmentIndex = 0;

    // 내부 변수
    private bool isHolding = false;
    private Transform holdingObject;
    private float graphValue;           // Lerp 같은 거 할 때 범용적으로 쓸 변수

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
        if(isHolding )
        {
            StartCoroutine(ObjectMoveToGrid(holdingObject));
            isHolding = false;
            Debug.Log("그런 위험한 건 내려놓고 얘기하자구");
            return;

        }

        // 인벤토리에서 오브젝트를 선택해서 사용한다면
        // 해당 상태를 소모
        // return;

        if (!NearObjectCheck()) return;

        var temp = fov.ClosestTransform.GetComponent<InteractionObject>();

        // 오브젝트의 인터랙션 활성화
        temp.InteractWithPlayer();

        // 오브젝트의 enum에 따라서 플레이어 상태 변경
        switch(temp.objectType)
        {
            case ObjectType.Grabable:
                isHolding = true;
                holdingObject = temp.transform;
                StartCoroutine(ObjectMoveToOverhead(holdingObject));
                break;

            case ObjectType.Dragable:

                break;

            case ObjectType.Pickup:
                StartCoroutine(PickUpDelay());
                break;

            case ObjectType.StageObject:
                // 작동시키는 애니메이션이 없는걸!!!!
                StartCoroutine(PickUpDelay());
                break;
        }
    }

    // 인터랙션 가능한 오브젝트를 fov에서 넣고
    // 그게 바뀔 경우에 PlayerInteraction에도 처리하고
    // UI도 새로운 UI를 띄워야 한다면...
    // 이벤트를 하나 만들어서 하죠
    // fov에서 등록시킵시다

    // 인터랙션이 가능한지 체크하는 함수
    // 선택된 오브젝트
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
        // 인벤토리에서 아이템 가져오기
        // 필드에서 아이템을 주울 수도 있으니
        // 잠깐... 도구는 아이템을 주워 쓰는 쪽이 아니잖아?
        // 오히려 Interact() 함수에서 집어던지는 처리를 해야하는구나
    }

    public void UpdateInteractChange()
    {
        var temp = fov.ClosestTransform;

        if ( temp != fov.ClosestTransform)
        {
            OnInteractChange?.Invoke(fov.ClosestTransform.GetComponent<InteractionObject>());
        }
    }

    IEnumerator ObjectMoveToOverhead(Transform _object)
    {
        while (isHolding)
        {
            var end = transform.position + Vector3.up * 1.5f;
            _object.transform.position = Vector3.Lerp(_object.transform.position, end, Time.deltaTime * 10f);
            yield return null;
        }
    }

    IEnumerator ObjectMoveToGrid(Transform _object)
    {
        Vector3 targetPosition = Vector3.one;
        
        // Sine 그래프를 따라 graphValue의 값을 조정
        GraphSine(1.0f);
        while (_object.transform.position != targetPosition)
        {
            // 실시간으로 바뀌는 graphValue의 값을 코루틴 2개를 돌려 Lerp에 사용
            _object.transform.position = Vector3.Lerp(_object.transform.position, targetPosition, graphValue);
            yield return null;
        }

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

    public void GraphSine(float lerpTime)
    {
        StartCoroutine(GraphSineCoroutine(lerpTime));
    }

    private IEnumerator GraphSineCoroutine(float lerpTime)
    {
        float currentTime = 0f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= lerpTime)
                currentTime = lerpTime;

            graphValue = currentTime / lerpTime;
            graphValue = Mathf.Sin(graphValue * Mathf.PI * 0.5f);

            yield return null;
        }

    }
}
