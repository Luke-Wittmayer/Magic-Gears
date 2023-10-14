using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private const string BASIC_ATTACK = "BasicAttack";
    public GameObject pistol;
    public GameObject rifle;

    [SerializeField] private GameObject player;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
        
    }

    public void BasicAttack() {
        pistol.SetActive(false);
        rifle.SetActive(true);
        animator.SetTrigger(BASIC_ATTACK);
        // pistol.SetActive(true);
        // rifle.SetActive(false);
    }

    public void Damaged(){
    }
}

