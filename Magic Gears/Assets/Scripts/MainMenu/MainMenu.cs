using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    
    public void PlayGame() {
        if(storeLevel.hasOpened) {
            SceneManager.LoadScene("Hub");
        }
        else {
            SceneManager.LoadScene("Intro");
        }
    }

    public void QuitGame() {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
