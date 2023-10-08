using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation conversation;

    private void OnTriggerStay(Collider other){
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ConversationManager.Instance.StartConversation(conversation);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
