using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private const string BASIC_ATTACK = "BasicAttack";
    private const string MANA_STEAL = "StealManaAttack";
    private const string SPEND_MANA = "SpendManaAttack";
    private const string DAMAGE = "GetHit";
    private const string VICTORY = "Win";
    private const string DEFEAT = "Dead";
    private const string ULTIMATE = "UltimateAttack";

    //private const string ULT_STATE = "UltimateState";
    public int UltimateState = 0;
    public GameObject pistol;
    public GameObject rifle;

    [SerializeField] private GameObject player;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
        
    }

    public void BasicAttack() {
        if(UltimateState == 0){
            pistol.SetActive(true);
            rifle.SetActive(false);
            //animator.SetBool(ULT_STATE, false);
        } else {
            UltimateState -= 1;
        }
        animator.SetTrigger(BASIC_ATTACK);
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
    }

    public void UltimateAttack(){
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
}

