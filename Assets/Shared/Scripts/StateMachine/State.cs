using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void Exit();

    protected float GetNormalizedTime(Animator animator, string tagToCheck = "Attack")
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);
        if (animator.IsInTransition(0) && nextInfo.IsTag(tagToCheck))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tagToCheck))
        {
            return currentInfo.normalizedTime;
        }
        return 0;
    }
}
