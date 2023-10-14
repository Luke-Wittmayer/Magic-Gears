using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private const string DAMAGED = "GetHit";
    [SerializeField] private GameObject Enemy;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
        
    }

    public void BasicAttack() {
        // pistol.SetActive(true);
        // rifle.SetActive(false);
    }

    public void Damaged(){
        animator.SetTrigger(DAMAGED);
    }
}
