using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{

    CharacterClass characterClass;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //Assigning values to the variables
        animator = GetComponent<Animator>();
        characterClass = GetComponent<CharacterClass>();
    }

    // Update is called once per frame
    void Update()
    {

        //Play animation if moving key is pressed
        if (characterClass.direction.x == 0 && characterClass.direction.z == 0 || characterClass.canMove == false)
        {
            animator.SetBool("IsWalking", false);
        }
        else
        {
            animator.SetBool("IsWalking", true);
        }

    }
}
