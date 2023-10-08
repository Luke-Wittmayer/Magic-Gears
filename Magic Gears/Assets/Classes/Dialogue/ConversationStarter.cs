using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ConversationStarter : MonoBehaviour
{
    bool inside = false;
    [SerializeField] private NPCConversation conversation;
    private CharacterClass characterClass; //Used to stop player from moving


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inside = true;
            characterClass = other.GetComponent<CharacterClass>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inside = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (inside == true && Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Start conversation and stop player from moving
            characterClass.canMove = false;
            characterClass.direction.x = 0;
            characterClass.direction.z = 0;
            ConversationManager.Instance.StartConversation(conversation);
            Cursor.lockState = CursorLockMode.None;
        }
        if (characterClass != null)
        {
            if(ConversationManager.Instance == null || !ConversationManager.Instance.IsConversationActive)
            {
                characterClass.canMove = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
