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
        private set => interactionObj = value;
    }

    [SerializeField]
    private Transform BlockPointer;             /* 이거 언제 쓸거야 */
    private FOVSystem fov;
    private PlayerController controller;

    // 내부 변수
    
    // private float t;                         // Lerp 같은 거 할 때 범용적으로 쓸 변수
    [SerializeField]
    private Equipment currentEquipment;         // 현재 장착한 장비
    private WaitForSeconds equipWaitTime;

    public void Setup()
    {
        fov = GetComponent<FOVSystem>();
        controller = GetComponent<PlayerController>();

        Debug.Log($"3. Setup - {this}");
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
                    controller.anim.ChangeAnimationState("Throw");
                    // 던지기 코드 처리
                    /* 지금은 그냥 보여주기용 */
                    StartCoroutine(ObjectMoveToGrid(interactionObj));
                    
                    break;

                case ObjectType.Dragable:
                    controller.ChangeState(PlayerStateType.Default);
                    break;
            }

            interactionObj = null;

            Debug.Log("그런 위험한 건 내려놓고 얘기하자구");
            return;
        }

        // 인벤토리에서 오브젝트를 선택해서 사용한다면
        // 이것도 isHolding에서 처리
        // 해당 상태를 소모
        // return;

        // 근처에 상호작용 가능한 오브젝트가 없을 경우 종료
        if (!NearObjectCheck()) return;

        // var temp = fov.ClosestTransform.GetComponent<InteractionObject>();

        if (fov.ClosestTransform.TryGetComponent(out InteractionObject temp))
        {
            // 오브젝트의 인터랙션 기능이 있으면 활성화
            temp.InteractWithPlayer();

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
                    // 뭔가 작동시키는 애니메이션이 나오는 기믹
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

    // 도구 인터랙션
    public void InteractWithEquipment()
    {
        if (isInteracting) return;
        if (currentEquipment == null) return;

        isInteracting = true;
        Debug.Log("콱");

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
        // 애니메이션 적용
        controller.anim.ChangeAnimationState(currentEquipment.EquipAnimName);

        // 인터랙션 중에는 아무것도 못하는 인터랙션 State로 변경
        controller.ChangeState(PlayerStateType.Interaction);

        // 장비 기능까지 선딜레이 처리
        yield return equipWaitTime = new WaitForSeconds(currentEquipment.EquipActionDelay);

        // 범위 지정
        var temp = Physics.OverlapSphere(gameObject.transform.position + gameObject.transform.forward, currentEquipment.EquipRange);

        if (temp != null)
        {
            for (int i = 0; i < temp.Length; i++)
            {
                // Breakable 태그 && 현재 장비의 효과 타입과 같을 경우 효과 진행
                if (temp[i].CompareTag("Breakable"))
                    temp[i].GetComponent<IngredientObject>().AffectedByEquipment(currentEquipment.EquipEffectiveType);
            }
        }
        // 장비의 후딜레이 처리
        yield return equipWaitTime = new WaitForSeconds(currentEquipment.EquipActionEndDelay - currentEquipment.EquipActionDelay);
        controller.ChangeState(PlayerStateType.Default);
        isInteracting = false;
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
    IEnumerator ObjectMoveToGrid(InteractionObject _object, float _lerpTime = 1.0f)
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

    // 아이템 줍기 처리
    IEnumerator PickUpDelay()
    {
        controller.ChangeState(PlayerStateType.Interaction);
        controller.anim.ChangeAnimationState("Player_Action_Pickup");
        yield return new WaitForSeconds(0.8f);
        controller.ChangeState(PlayerStateType.Default);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        try
        {
            Gizmos.DrawSphere(gameObject.transform.position + gameObject.transform.forward, currentEquipment.EquipRange);
        }
        catch (System.Exception)
        {
            Debug.LogError("도구가 없는데요 형님");
        }
    }
}
