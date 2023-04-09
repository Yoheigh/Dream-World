using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private string anim_currentState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void ChangeAnimationState(string p_state)
    {
        if (anim_currentState == p_state)
            return;
        animator.Play(p_state);
        anim_currentState = p_state;
    }
}
