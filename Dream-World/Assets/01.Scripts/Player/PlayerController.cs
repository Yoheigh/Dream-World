using Cinemachine;
using PlayerOwnedStates;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public enum PlayerStateType : sbyte
{
    Default = 0,
    Falling,
    Dragging,
    Climbing,
    Interaction,
    Damaged,
    Cinematic
}

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public CustomInput Input => Manager.Instance.Input;

    public Action OnDamage;

    public PlayerInteraction interaction;
    public PlayerMovement movement;
    public PlayerAnimation anim;

    public FOVSystem fov;
    public Animator animator;

    private StateMachine<PlayerController> PlayerFSM;
    private State<PlayerController>[] PlayerStates;

    private void Start()
    {
        this.SetUp();
        movement.Setup();
        interaction.Setup();
        anim.Setup(animator);

        // 인풋에 함수 등록
        Input.RegisterInteractPerformed(Interact);
        Input.RegisterInteractWithEquipmentPerformed(InteractWithEquipment);

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void Update()
    {
        PlayerFSM.Execute();
    }

    private void SetUp()
    {
        movement = GetComponent<PlayerMovement>();
        interaction = GetComponent<PlayerInteraction>();
        anim = new PlayerAnimation();
        animator = GetComponentInChildren<Animator>();
        fov = GetComponent<FOVSystem>();

        // FSM 셋업
        PlayerStates = new State<PlayerController>[Enum.GetValues(typeof(PlayerStateType)).Length];
        PlayerStates[(int)PlayerStateType.Default] = new DefaultState();
        PlayerStates[(int)PlayerStateType.Falling] = new FallingState();
        PlayerStates[(int)PlayerStateType.Dragging] = new DraggingState();
        PlayerStates[(int)PlayerStateType.Climbing] = new ClimbingState();
        PlayerStates[(int)PlayerStateType.Interaction] = new InteractionState();
        PlayerStates[(int)PlayerStateType.Damaged] = new DamagedState();
        PlayerStates[(int)PlayerStateType.Cinematic] = new CinematicState();

        PlayerFSM = new StateMachine<PlayerController>();
        PlayerFSM.Setup(this, PlayerStates[(int)PlayerStateType.Default]);

        Debug.Log($"1. Setup - {this}");
    }

    public void Hit()
    {
        StartCoroutine(DamageCoroutine());
    }

    private IEnumerator DamageCoroutine()
    {
        ChangeState(PlayerStateType.Damaged);
        Debug.Log("아야");

        var temp = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        var waitTime = new WaitForSeconds(0.05f);

        anim.ChangeAnimationState("Damage");

        temp.enabled = false;
        yield return waitTime;
        temp.enabled = true;
        yield return waitTime;
        temp.enabled = false;
        yield return waitTime;
        temp.enabled = true;
        yield return waitTime;
        temp.enabled = false;
        yield return waitTime;
        temp.enabled = true;
        yield return waitTime;

        anim.ChangeAnimationState("Default");

        Debug.Log("이제 아프지 않아요오");
        ChangeState(PlayerStateType.Default);

        // 체력 감소 이벤트
        OnDamage?.Invoke();
    }

    public void ChangeState(PlayerStateType _type)
    {
        PlayerFSM.ChangeState(PlayerStates[(int)_type]);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        interaction.Interact();
    }

    public void InteractWithEquipment(InputAction.CallbackContext context)
    {
        interaction.InteractWithEquipment();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var tempbool = hit.gameObject.TryGetComponent<TriggerObject>(out var triggerObj);

        if (tempbool == true)
            triggerObj.PlayerEntered(this);
    }
}