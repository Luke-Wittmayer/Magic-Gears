using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAlly : MonoBehaviour
{
    // Used to know when to show the ally according to the character level
    [SerializeField] CharacterClass characterClass;
//    bool activated = true;

    [SerializeField] int levelToUnlock;
    [SerializeField] private GameObject characterGameObject; // Reference to the character GameObject

    // Start is called before the first frame update
    void Start()
    {
        // Make sure to check if the characterGameObject is not null before using it
        if (levelToUnlock > characterClass.levelCompleted)
        {
         //   Debug.Log("TESTING" + characterClass.levelCompleted + "    " + levelToUnlock);
            characterGameObject.SetActive(false);
        }
        else
        {
            //Debug.Log("ELSE TESTING" + characterClass.levelCompleted + "    " + levelToUnlock);
            characterGameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

}
