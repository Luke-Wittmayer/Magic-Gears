using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private const string DAMAGED = "GetHit";
    private const string BASIC_ATTACK = "BasicAttack";
    [SerializeField] private GameObject Enemy;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
        
    }

    public void EnemyBasicAttack() {
        animator.SetTrigger(BASIC_ATTACK);
    }

    public void Damaged(){
        animator.SetTrigger(DAMAGED);
    }
}
