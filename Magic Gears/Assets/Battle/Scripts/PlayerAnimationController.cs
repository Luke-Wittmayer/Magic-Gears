using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private const string BASIC_ATTACK = "BasicAttack";
    private const string MANA_STEAL = "StealManaAttack";
    private const string SPEND_MANA = "SpendManaAttack";
    private const string DAMAGE = "GetHit";
    public GameObject pistol;
    public GameObject rifle;

    [SerializeField] private GameObject player;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
        
    }

    public void BasicAttack() {
        pistol.SetActive(true);
        rifle.SetActive(false);
        animator.SetTrigger(BASIC_ATTACK);
    }

    public void ManaStealAttack() {
        animator.SetTrigger(MANA_STEAL);
    }

    public void SpendManaAttack() {
        animator.SetTrigger(SPEND_MANA);
    }

    public void Damaged(){
        animator.SetTrigger(DAMAGE);
    }
}

