using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerInputManagement playerInput;
    private InputAction Input;

    public delegate void PlayerControl(InputAction playerInput);
    public static PlayerControl playerControl;

    private PlayerMovement playerMovement;

    private bool isControl;

    void Awake()
    {
        playerInput = new PlayerInputManagement();
    }

    void OnEnable()
    {
        Input = playerInput.ActionMap.Move;
        Input.Enable();
    }

    private void OnDisable()
    {
        Input.Disable();
    }

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        playerControl(Input);
    }

    //void ChangeAnimationState(string p_state)
    //{
    //    if (anim_currentState == p_state)
    //        return;

    //    animator.Play(p_state);
    //    anim_currentState = p_state;
    //}
}
