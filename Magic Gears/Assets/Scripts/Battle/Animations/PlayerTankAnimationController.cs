using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAlliesAnimationController : PlayerAnimationController
{
    public override void BasicAttack()
    {
        animator.SetTrigger(BASIC_ATTACK);
    }
}
