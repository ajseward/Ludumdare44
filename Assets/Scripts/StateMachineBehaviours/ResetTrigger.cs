using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTrigger : StateMachineBehaviour
{
    public string Trigger;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.ResetTrigger(Trigger);
    }
}
