using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public SimpleShoot gun;
    public const string BASIC_ATTACK = "BasicAttack";
    public const string MANA_STEAL = "StealManaAttack";
    public const string SPEND_MANA = "SpendManaAttack";
    public const string DAMAGE = "GetHit";
    public const string VICTORY = "Win";
    public const string DEFEAT = "Dead";
    public const string ULTIMATE = "UltimateAttack";

    //private const string ULT_STATE = "UltimateState";
    public int UltimateState = 0;
    public GameObject pistol;
    public GameObject rifle;

    [SerializeField] private GameObject player;
    public Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
        
    }

    public virtual void BasicAttack() {
        if(UltimateState == 0){
            pistol.SetActive(true);
            rifle.SetActive(false);
            //animator.SetBool(ULT_STATE, false);
        } else {
            UltimateState -= 1;
        }
        animator.SetTrigger(BASIC_ATTACK);
        gun.gunAnimator.SetTrigger("Fire");
    }

    public void ManaStealAttack() {
        if(UltimateState == 0){
            pistol.SetActive(true);
            rifle.SetActive(false);
            //animator.SetBool(ULT_STATE, false);
        } else {
            UltimateState -= 1;
        }
        animator.SetTrigger(MANA_STEAL);
    }

    public void SpendManaAttack() {
        if(UltimateState == 0){
            pistol.SetActive(true);
            rifle.SetActive(false);
            //animator.SetBool(ULT_STATE, false);
        } else {
            UltimateState -= 1;
        }
        animator.SetTrigger(SPEND_MANA);
        gun.gunAnimator.SetTrigger("Fire");
        gun.gunAnimator.SetTrigger("Fire");
    }

    public virtual void UltimateAttack(){
        pistol.SetActive(false);
        rifle.SetActive(true);
        UltimateState = 2;
        animator.SetTrigger(ULTIMATE);
        //animator.SetBool(ULT_STATE, true);
    }

    public void Damaged(){
        animator.SetTrigger(DAMAGE);
    }

    public void Win(){
        animator.SetTrigger(VICTORY);
    }
    public void Lose(){
        animator.SetTrigger(DEFEAT);
    }


    public virtual void OffensiveAttack()
    {
        return;
    }
    public virtual void DefensiveAttack()
    {
        return;
    }
}

