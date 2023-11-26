using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DialogueEditor;
using static storeLevel;
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
            SceneManager.LoadScene("Wave 1.1");
        }
        else if (teleportTo == 0)
        {
            storeLevel.level = 0;
            SceneManager.LoadScene("Hub");
        }
        else if (teleportTo == 2)
        {
            SceneManager.LoadScene("Wave 1.2");
        }
        else if (teleportTo == 3)
        {
            SceneManager.LoadScene("Wave 1.3");
        }
        else if (teleportTo == 4)
        {
            storeLevel.level = 1;
            SceneManager.LoadScene("Hub");
        }
        else if (teleportTo == 5)
        {
            SceneManager.LoadScene("Wave 2.1");
        }
        else if (teleportTo == 6)
        {
            SceneManager.LoadScene("Wave 2.2");
        }
        else if (teleportTo == 7)
        {
            SceneManager.LoadScene("Wave 2.3");
        }
        else if (teleportTo == 8)
        {
            storeLevel.level = 2;
            SceneManager.LoadScene("Hub");
        }
        else if (teleportTo == 9)
        {
            SceneManager.LoadScene("Wave 3.1");
        }
        else if (teleportTo == 10)
        {
            SceneManager.LoadScene("Wave 3.2");
        }
        else if (teleportTo == 11)
        {
            SceneManager.LoadScene("Wave 3.3");
        }
        else if (teleportTo == 12)
        {
            storeLevel.level = 3;
            SceneManager.LoadScene("Hub");
        }

    }
}