using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    //[Header("PlayerInteraction")]
    //[Tooltip("접촉한 오브젝트와 자동으로 상호작용하기 까지 TriggerTime 초 걸립니다.")]
    //public float TriggerTime = 0.3f;

    IInteractBehaviour interactBehaviour;

    public Action<InteractionObject> OnTargetChange;
    public Action<PlayerEquipment> OnEquipmentChange;

    private bool isInteracting = false;
    public bool IsInteracting
    {
        get => isInteracting;
        private set => isInteracting = value;
    }

    private InteractionObject interactionObj;
    public InteractionObject InteractionObj     // 다른 곳에서 필요한 경우 사용할 것
    {
        get => interactionObj;
        set => interactionObj = value;
    }

    [SerializeField]
    private FOVSystem fov;
    private PlayerController controller;
    private PreviewPrefab preview;

    private bool isBuildMode = false;

    // 내부 변수
    Vector3 equipActionPos;

    // private float t;                         // Lerp 같은 거 할 때 범용적으로 쓸 변수
    [SerializeField]
    private Equipment currentEquipment;         // 현재 장착한 장비

    [SerializeField]
    private Building currentBuilding;         // 현재 장착한 장비

    [SerializeField]
    private Transform equipModelRoot;           // 장비 장착될 손 (오른손이 기준)

    [SerializeField]
    private Transform ShovelRoot;             // 삽 애니메이션 왼손이라 왼손

    private GameObject equipModel;
    private WaitForSeconds equipWaitTime;

    // 리팩토링 필요

    public string throwingObjectPath;

    public void Setup()
    {
        fov = GetComponent<FOVSystem>();
        controller = GetComponent<PlayerController>();
        preview = GetComponent<PreviewPrefab>();
        
        // 나중에 리팩토링 할 거임
        Manager.Instance.Inventory.OnChangeEquipment += ChangeEquipment;
        Manager.Instance.Inventory.OnChangeEquipment.Invoke(0);

        Debug.Log($"3. Setup - {this}");
    }

    public void ChangeBuildMode()
    {
        isBuildMode = !isBuildMode;
    }

    public void Interact()
    {
        if (isInteracting) return;

        // 손에 무언가 들고 있는 상태일 경우 처리
        if (interactionObj != null)
        {
            // 오브젝트의 enum에 따라서 플레이어 상태 변경
            switch (interactionObj.ObjectType)
            {
                case ObjectType.Grabable:
                    // controller.anim.ChangeAnimationState("Throw");
                    // 던지기 코드 처리
                    /* 지금은 그냥 보여주기용 */
                    ReleaseGrabable(interactionObj);
                    break;

                case ObjectType.Dragable:
                    controller.ChangeState(PlayerStateType.Default);
                    break;
            }

            interactionObj = null;
            return;
        }

        // 건물 생성 모드
        //if (isBuildMode)
        //{
        //    Manager.Instance.Build.Construct();
        //    isBuildMode = false;
        //    return;
        //}

        // 근처에 상호작용 가능한 오브젝트가 없을 경우 종료
        if (!NearObjectCheck()) return;

        // var temp = fov.ClosestTransform.GetComponent<InteractionObject>();

        if (fov.ClosestTransform.TryGetComponent(out InteractionObject temp))
        {
            // 오브젝트의 인터랙션 기능이 있으면 활성화
            temp.InteractWithPlayer(controller);

            // 오브젝트의 enum에 따라서 플레이어 상태 변경
            switch (temp.ObjectType)
            {
                case ObjectType.Grabable:
                    interactionObj = temp;
                    StartCoroutine(ObjectMoveToOverhead(interactionObj));
                    break;

                case ObjectType.Dragable:
                    interactionObj = temp;
                    StartCoroutine(GrabDragable((Dragable)interactionObj));
                    break;

                case ObjectType.Pickup:
                    StartCoroutine(PickUpDelay());
                    break;

                case ObjectType.StageObject:
                    Debug.Log($"{temp}와 상호작용");
                    break;
            }
        }
        else
            Debug.Log("LayerMask : Interactable 이지만 <InteractionObject> 컴포넌트가 없습니다");
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

    private void Update()
    {
        equipActionPos = transform.position + transform.forward;
    }

    public void InteractWithEquipment()
    {
        if (isInteracting) return;
        if (currentEquipment == null) return;

        isInteracting = true;

        switch (currentEquipment.EquipActionType)
        {
            case EquipmentActionType.Melee:
                MeleeAction();
                break;

            case EquipmentActionType.Ranged:
                RangedAction();
                break;
        }
    }

    // 근접 도구 함수
    public void MeleeAction()
    {
        // 애니메이션 재생
        StartCoroutine(MeleeActionDelay());
    }

    // 장비에 명시된 기능대로 작동
    private IEnumerator MeleeActionDelay()
    {
        if (equipModel != null)
        {
            if (equipModel.name != currentEquipment.name)
            {
                Destroy(equipModel);
                equipModel = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(currentEquipment.EquipPrefabPath), equipModelRoot);
                equipModel.transform.localRotation = Quaternion.Euler(currentEquipment.EquipHandOffset);
                equipModel.name = currentEquipment.name;

                // 으아악
                if (currentEquipment.name == "Shovel")
                {
                    equipModel.transform.SetParent(ShovelRoot);
                    equipModel.transform.localPosition = Vector3.zero;
                }

                equipModel.SetActive(true);
            }
            else
            {
                equipModel.SetActive(true);
            }
        }
        else
        {
            equipModel = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(currentEquipment.EquipPrefabPath), equipModelRoot);
            equipModel.transform.localRotation = Quaternion.Euler(currentEquipment.EquipHandOffset);
            equipModel.name = currentEquipment.name;

            if (currentEquipment.name == "Shovel")
            {
                equipModel.transform.SetParent(ShovelRoot);
                equipModel.transform.localPosition = Vector3.zero;
            }

            equipModel.SetActive(true);
        }


        // 인터랙션 중에는 아무것도 못하는 인터랙션 State로 변경
        controller.ChangeState(PlayerStateType.Interaction);

        // 애니메이션 적용
        controller.anim.ChangeAnimationState($"{currentEquipment.EquipAnimName}");

        // 장비 기능까지 선딜레이 처리
        yield return equipWaitTime = new WaitForSeconds(currentEquipment.EquipActionDelay);

        // 해당 트랜스폼의 forward 값을 장비 작동 위치로
        //equipActionPos = transform.position + transform.forward;

        // 범위 지정
        var colliders = Physics.OverlapCapsule(equipActionPos, equipActionPos + currentEquipment.EquipOffset, currentEquipment.EquipRange);
        float tempHeight = -10f;

        // 상호작용했을 경우
        if (colliders != null)
        {
            GameObject target = null;

            for (int i = 0; i < colliders.Length; i++)
            {
                // 그 중에 제일 상위에 있는 오브젝트 찾기
                if (colliders[i].transform.position.y > tempHeight)
                {
                    // Breakable 태그가 달려있다면 해당 오브젝트를 target으로
                    if (colliders[i].CompareTag("Breakable"))
                    {
                        target = colliders[i].gameObject;
                        tempHeight = target.transform.position.y;
                    }
                }
            }
            // 검색한 것중에 가장 높은 곳에 있는 오브젝트 파괴
            if (target != null)
            {
                target.GetComponent<IngredientObject>().AffectedByEquipment(currentEquipment.EquipEffectiveType);

                //if (Mathf.Abs(tempHeight - transform.position.y) > 0.7f)
                //{
                //    try { controller.anim.ChangeAnimationState($"{currentEquipment.EquipAnimName}Top"); }
                //    catch { Debug.LogError("애니메이션이 없으므로 기본을 마저 재생합니다."); }
                //}
                //else
                //{
                //    try { controller.anim.ChangeAnimationState($"{currentEquipment.EquipAnimName}Bottom"); }
                //    catch { Debug.LogError("애니메이션이 없으므로 기본을 마저 재생합니다."); }
                //}

                try { controller.anim.ChangeAnimationState($"{currentEquipment.EquipAnimName}Top"); }
                catch { Debug.LogError("애니메이션이 없으므로 기본을 마저 재생합니다."); }

                // 장비의 후딜레이 처리
                yield return equipWaitTime = new WaitForSeconds(currentEquipment.EquipActionEndDelay - currentEquipment.EquipActionDelay);
                isInteracting = false;
                equipModel.SetActive(false);
                controller.anim.ChangeAnimationState("Default");
                controller.ChangeState(PlayerStateType.Default);
            }
            else
            {
                try { controller.anim.ChangeAnimationState($"{currentEquipment.EquipAnimName}None"); }
                catch { Debug.LogError("애니메이션이 없으므로 기본을 마저 재생합니다."); }

                // 장비의 후딜레이 처리
                yield return equipWaitTime = new WaitForSeconds(currentEquipment.EquipActionEndDelay - currentEquipment.EquipActionDelay);
                isInteracting = false;
                equipModel.SetActive(false);
                controller.anim.ChangeAnimationState("Default");
                controller.ChangeState(PlayerStateType.Default);
            }
        }
        else
        {
            try { controller.anim.ChangeAnimationState($"{currentEquipment.EquipAnimName}None"); }
            catch { Debug.LogError("애니메이션이 없으므로 기본을 마저 재생합니다."); }

            // 장비의 후딜레이 처리
            yield return equipWaitTime = new WaitForSeconds(currentEquipment.EquipActionEndDelay - currentEquipment.EquipActionDelay);
            isInteracting = false;
            equipModel.SetActive(false);
            controller.anim.ChangeAnimationState("Default");
            controller.ChangeState(PlayerStateType.Default);
        }
    }

    // 원거리 도구 함수
    public void RangedAction() { }

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
    IEnumerator ObjectMoveToOverhead(InteractionObject _object)
    {
        while (interactionObj != null)
        {
            //GraphSine(0.5f, (obj) =>
            //{
            //    _object.transform.position = Vector3.Lerp(_object.transform.position, end, obj);
            //});

            var end = transform.position + Vector3.up * 1.5f;

            _object.transform.rotation = transform.rotation;
            _object.transform.position = end;

            yield return null;
        }
    }

    private IEnumerator GrabDragable(Dragable _drag)
    {
        var dragObj = _drag;

        Vector3 playerPos = dragObj.AnchoredPosition(transform.position, out Vector3 anchoredRot);

        transform.SetPositionAndRotation(new Vector3(playerPos.x, transform.position.y, playerPos.z), Quaternion.Euler(anchoredRot));

        // 테스트로 Move() 써봤다가 하늘로 날아감. 물리엔진과 함께 적용되는 모양
        // gameObject.GetComponent<CharacterController>().Move(anchoredRot);
        // transform.rotation = Quaternion.Euler(anchoredRot);

        controller.ChangeState(PlayerStateType.Dragging);

        // 상호작용 중인 오브젝트가 사라졌을 경우
        while (dragObj != null)
        {
            if (dragObj == null)
            {
                // 원래 상태로 돌아가게 하는 코드
                controller.ChangeState(PlayerStateType.Default);
                break;
            }
            yield return null;
        }
    }

    // 플레이어가 들고있던 걸 Grid로 옮기는 코루틴
    /* Lerp 그래프를 코루틴으로 사용해보고 싶어서 만들어본거라 사용 안 할 듯 */
    IEnumerator ObjectMoveToGrid(InteractionObject _object, Vector3 targetPosition, float _lerpTime = 1.0f)
    {
        // Sine 그래프를 따라 graphValue의 값을 조정
        GraphSine(_lerpTime, (obj) =>
            {
                // 실시간으로 바뀌는 graphValue의 값을 코루틴 2개를 돌려 Lerp에 사용
                _object.transform.position = Vector3.Lerp(_object.transform.position, targetPosition, obj);
            });
        yield return new WaitForSeconds(_lerpTime);

        var obj = _object.GetComponent<InteractionObject>() as Grabable;
        obj.OnRelease();
    }

    public void ReleaseGrabable(InteractionObject _object)
    {
        var obj = _object.GetComponent<InteractionObject>() as Grabable;
        obj.OnRelease();
    }

    // 아이템 줍기 처리
    IEnumerator PickUpDelay()
    {
        controller.ChangeState(PlayerStateType.Interaction);
        controller.anim.ChangeAnimationState("Player_Action_Pickup");
        yield return new WaitForSeconds(0.8f);
        controller.ChangeState(PlayerStateType.Default);
    }

    public void ThrowConsumable()
    {
        throwingObjectPath = "Prefabs/DropDirt";
        //// 초원
        //if(SceneManager.GetActiveScene().buildIndex <= 4)
        //{
        //    throwingObjectPath = "Prefabs/DropDirt";
        //}
        //else if(SceneManager.GetActiveScene().buildIndex <= 9)
        //{
        //    throwingObjectPath = "Prefabs/DropSnow";
        //}
        //else if (SceneManager.GetActiveScene().buildIndex <= 13)
        //{

        //}
        //else if (SceneManager.GetActiveScene().buildIndex <= 17)
        //{

        //}
        //else if (SceneManager.GetActiveScene().buildIndex <= 21)
        //{

        //}

        controller.anim.ChangeAnimationState("Throw");
        GameObject obj = Instantiate(Resources.Load<GameObject>(throwingObjectPath), transform.forward, Quaternion.identity, null);
        // var temp = obj.GetComponent<ThrowingObject>();
    }

    //private void SnapPlayerPos()
    //{
    //    var ladder = ladderObj.transform.GetComponent<Ladder>();
    //    movement.SetVerticalPoint(ladder.Pivot, ladder.ReachHeight);
    //    ladderObj = null;
    //    controller.ChangeMoveState(PlayerStateType.Climbing);
    //}

    // Sine 함수 콜백 ( 점차 증가하는 속도가 빨라지는 그래프 )
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

    public void ChangeEquipment(int index)
    {
        try
        {
            currentEquipment = Manager.Instance.Inventory.equipments[index] as Equipment;
        }
        catch
        {
            currentEquipment = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (currentEquipment != null)
        {
            Gizmos.DrawSphere(equipActionPos, currentEquipment.EquipRange);
            Gizmos.DrawSphere(equipActionPos + currentEquipment.EquipOffset, currentEquipment.EquipRange);
        }

        else
            return;
    }
}
