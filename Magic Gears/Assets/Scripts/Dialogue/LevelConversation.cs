using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DialogueEditor;
using UnityEngine.SceneManagement;

public class LevelConversation : MonoBehaviour
{
    [SerializeField] private NPCConversation conversation;
    [SerializeField] private int teleportTo;

    private CharacterClass characterClass; //Used to stop player from moving

    // Start is called before the first frame update
    void Start()
    {
        //Start conversation and stop player from moving
        ConversationManager.Instance.StartConversation(conversation);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        ConversationManager.OnConversationStarted += ConversationStart;
        ConversationManager.OnConversationEnded += ConversationEnd;
    }

    private void OnDisable()
    {
        ConversationManager.OnConversationStarted -= ConversationStart;
        ConversationManager.OnConversationEnded -= ConversationEnd;
    }
    private void ConversationStart()
    {
        Debug.Log("A conversation has began.");
    }

    private void ConversationEnd()
    {
        if (teleportTo == 1)
        {
            SceneManager.LoadScene("Combat Scene - Luke");
        }
        else if (teleportTo == 2)
        {
            SceneManager.LoadScene("Hub");
        }
        else if (teleportTo == 3)
        {
            SceneManager.LoadScene("Main Menu");
        }
        else if (teleportTo == 3)
        {
            SceneManager.LoadScene("Hub");
        }
        else if (teleportTo == 4)
        {
            SceneManager.LoadScene("Main Menu");
        }
        else if (teleportTo == 5)
        {
            SceneManager.LoadScene("Hub");
        }
        else if (teleportTo == 6)
        {
            SceneManager.LoadScene("Main Menu");
        }
        else if (teleportTo == 7)
        {
            SceneManager.LoadScene("Hub");
        }
        else if (teleportTo == 8)
        {
            SceneManager.LoadScene("Main Menu");
        }
        else if (teleportTo == 9)
        {
            SceneManager.LoadScene("Hub");
        }
        else if (teleportTo == 10)
        {
            SceneManager.LoadScene("Main Menu");
        }
        else if (teleportTo == 11)
        {
            SceneManager.LoadScene("Hub");
        }
        else if (teleportTo == 12)
        {
            SceneManager.LoadScene("Main Menu");
        }

    }
}