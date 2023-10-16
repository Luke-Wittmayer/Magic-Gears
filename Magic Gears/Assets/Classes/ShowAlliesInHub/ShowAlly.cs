using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAlly : MonoBehaviour
{
    // Used to know when to show the ally according to the character level
    CharacterClass characterClass;
    bool activated = false;

    [SerializeField] int levelToUnlock;
    public GameObject characterGameObject; // Reference to the character GameObject

    // Start is called before the first frame update
    void Start()
    {
        // Make sure to check if the characterGameObject is not null before using it
        if (characterGameObject != null)
        {
            Debug.Log("Reaches this point");
            characterGameObject.SetActive(false);
        }
        characterClass = GetComponent<CharacterClass>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShowInHub()
    {
        // Activate the character GameObject after defeating the level
        if (activated || characterGameObject == null)
        {
            Debug.Log("Reaches this point");
            return;
        }

        if (characterClass.levelCompleted >= levelToUnlock)
        {
            activated = true;
            characterGameObject.SetActive(true);
        }
        return;
    }
}
