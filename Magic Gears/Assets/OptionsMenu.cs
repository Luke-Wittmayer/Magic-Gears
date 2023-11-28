using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
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

    void Resume() {
        Menu.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }
    void Pause() {
        Cursor.visible = true;
        Menu.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }
}
