using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class OptionsMenuCombat : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public static bool IsPaused = false;
    public GameObject Menu;

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(IsPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }
    public void SetVolume(float volume) {
        AudioMixer.SetFloat("Volume", volume);
    }

    public void Resume() {
        Menu.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }
    public void Pause() {
        Menu.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene(0);
    }

    public void QuitGame() {
        Debug.Log("Quitting...");
        Application.Quit();
    }
}
