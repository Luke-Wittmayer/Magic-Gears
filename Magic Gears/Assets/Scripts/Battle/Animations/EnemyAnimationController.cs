using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private const string DAMAGED = "GetHit";
    private const string BASIC_ATTACK = "BasicAttack";
    private const string DEAD = "Dead";
    private const string OFF = "Of";
    private const string DEF = "Def";
    private const string ULT = "Ult";
    [SerializeField] private GameObject Enemy;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
        
    }

    public void EnemyBasicAttack() {
        animator.SetTrigger(BASIC_ATTACK);
    }

    public void EnemyOffensiveAttack()
    {
        animator.SetTrigger(OFF);
    }

    public void EnemyDefensiveAttack()
    {
        animator.SetTrigger(DEF);
    }

    public void EnemyUltimateAttack()
    {
        animator.SetTrigger(ULT) ;
    }

    public void Damaged(){
        animator.SetTrigger(DAMAGED);
    }

    public void Dead(){
        animator.SetTrigger(DEAD);
    }
}
