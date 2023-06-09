using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation
{
    private Animator animator;
    private string animCurrentState;

    public void Setup(Animator _animator)
    {
        animator = _animator;
        Debug.Log($"4. Setup - {this}");
    }

    public void ChangeAnimationState(string p_state)
    {
        //if (animCurrentState == p_state)
        //    return;

        animator.Play(p_state);
        animCurrentState = p_state;
    }

    private IEnumerator AnimationDelay(string animState, float delay)
    {
        ChangeAnimationState(animState);

        // float temp = animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(delay);
        // 현재 재생중인 애니메이션의 길이 만큼 대기...인데 급해서 좀;
        animCurrentState = null;
    }
}
