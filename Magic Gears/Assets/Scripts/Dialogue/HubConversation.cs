using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DialogueEditor;

public class ConversationStarter : MonoBehaviour
{
    bool inside = false;
     private bool imageIsActive;
    [SerializeField] private bool notLoreGuy;
    [SerializeField] private NPCConversation conversation;

    //Images for the lore guy to display at the end of the conversation
    [SerializeField] private Image image1;
    [SerializeField] private Image image2;
    [SerializeField] private Image image3;
    [SerializeField] private Image image4;
    [SerializeField] private Image image5;
    [SerializeField] private Image image6;
    [SerializeField] private Image image7;
    [SerializeField] private Image image8;
    [SerializeField] private Image image9;

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
        imageIsActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (notLoreGuy)
        {
            imageIsActive = false;
        }
        //Images for the Lore guy
        if (inside)
        {
            if (characterClass != null)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && !ConversationManager.Instance.IsConversationActive && !imageIsActive)
                {
                    //Start conversation and stop player from moving
                    ConversationManager.Instance.StartConversation(conversation);
                }

                else if (ConversationManager.Instance == null || ConversationManager.Instance.IsConversationActive || imageIsActive)
                {
                    characterClass.canMove = false;
                    characterClass.direction.x = 0;
                    characterClass.direction.z = 0;
                    Cursor.lockState = CursorLockMode.None;

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        showImage(false, image1);
                        showImage(false, image2);
                        showImage(false, image3);
                        showImage(false, image4);
                        showImage(false, image5);
                        showImage(false, image6);
                        showImage(false, image7);
                        showImage(false, image8);
                        showImage(false, image9);
                    }
                }
                else
                {
                    characterClass.canMove = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    imageIsActive = false;

                }


            }
        }
       
        
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
        if (characterClass != null)
        {
            ConversationManager.Instance.SetInt("CurrentLevel", characterClass.levelCompleted);
        }
    }

      private void ConversationEnd()
     {
        if (ConversationManager.Instance.GetInt("EnemySelected") != 0)
        {
            imageIsActive = true;
            if (ConversationManager.Instance.GetInt("EnemySelected") == 1)
            {
                showImage(true, image1);
            }
            else if (ConversationManager.Instance.GetInt("EnemySelected") == 2)
            {
                showImage(true, image2);
            }
            else if (ConversationManager.Instance.GetInt("EnemySelected") == 3)
            {
                showImage(true, image3);
            }
            else if (ConversationManager.Instance.GetInt("EnemySelected") == 4)
            {
                showImage(true, image4);
            }
            else if (ConversationManager.Instance.GetInt("EnemySelected") == 5)
            {
                showImage(true, image5);
            }
            else if (ConversationManager.Instance.GetInt("EnemySelected") == 6)
            {
                showImage(true, image6);
            }
            else if (ConversationManager.Instance.GetInt("EnemySelected") == 7)
            {
                showImage(true, image7);
            }
            else if (ConversationManager.Instance.GetInt("EnemySelected") == 8)
            {
                showImage(true, image8);
            }
            else if (ConversationManager.Instance.GetInt("EnemySelected") == 9)
            {
                showImage(true, image9);
            }
        }
     }

    private void showImage(bool activate, Image image)
    {
        if (image != null)
        {
            image.gameObject.SetActive(activate);
        }
        imageIsActive = activate;
    }
}
