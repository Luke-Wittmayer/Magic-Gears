using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankAnimationController : PlayerAnimationController
{
    public const string OFF = "Of";
    public const string DEF = "Def";
    public const string ULT = "Ult";
    public override void BasicAttack()
    {
        animator.SetTrigger(BASIC_ATTACK);
    }

    public  override void OffensiveAttack()
    {
        animator.SetTrigger(OFF);
    }
    public override void DefensiveAttack()
    {
        animator.SetTrigger(DEF);
    }
    public override void UltimateAttack()
    {
        animator.SetTrigger(ULT);
    }
}

