using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void Exit();

    protected float GetNormalizedTime(Animator animator)
    {
        AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextStateInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextStateInfo.IsTag("Attack"))
        {
            return nextStateInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentStateInfo.IsTag("Attack"))
        {
            return currentStateInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
