using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleHUD : MonoBehaviour
{
    public Image playerMana;
    public Image playerHealth;
    public Image enemyMana;
    public Image enemyHealth;
    public Enemy enemyUnit;
    public Unit currentPlayerUnit;
    public GameObject victoryObj;
    public GameObject lostObj;
    public BattleSystem battle;
    

    

    void Update() {
        if(battle.state == BattleState.WON) {
            victoryObj.SetActive(true);
        }
        else if(battle.state == BattleState.LOST) {
            lostObj.SetActive(true);
        }
    }
    public void SetupHUD(){
        playerMana.fillAmount = 0f;
        playerHealth.fillAmount = 1f;
        enemyMana.fillAmount = 0f;
        enemyHealth.fillAmount = 1f;
    }


    public void SetPlayerMana(int mana){
        float manaf = (float)mana;
        playerMana.fillAmount = manaf/Unit.maxPlayerMana;
    }

    public void SetPlayerHealth(int health) {
        float healthf = (float)health;
    playerHealth.fillAmount = healthf/currentPlayerUnit.maxHP;
    }

    public void SetEnemyMana(int mana){
        float manaf = (float)mana;
        enemyMana.fillAmount = manaf/enemyUnit.maxEnemyMana;
    }

    public void SetEnemyHealth(int health) {
        float healthf = (float)health;
        enemyHealth.fillAmount = healthf/enemyUnit.maxHP;
    }

    public void GoToHUB() {
        SceneManager.LoadScene("Hub");
    }

    public void ReloadScene() {
        Scene curScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(curScene.name);
    }

    public void LoadNextScene() {
        Scene curScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(curScene.buildIndex + 1);
    }

    public void switchToDPS(Unit DPS) {
        if(DPS == currentPlayerUnit) {
            return;
        }
        else {
            Debug.Log("Switching");
            currentPlayerUnit.gameObject.SetActive(false);
            currentPlayerUnit = DPS;
            DPS.gameObject.SetActive(true);
        }
    }

    public void switchToHealer(Unit Healer) {
        if(Healer == currentPlayerUnit) {
            Debug.Log("No");
            return;
        }
        else {
            Debug.Log("Switch");
            StartCoroutine(switchToHealerE(Healer));
        }
    }
    IEnumerator switchToHealerE(Unit Healer) {
        
        Debug.Log("Switching");
        currentPlayerUnit.switchParticles.Stop();
        if(!currentPlayerUnit.switchParticles.isPlaying) {
            currentPlayerUnit.switchParticles.Play();
            Debug.Log("Particle");
        }
        yield return new WaitForSeconds(1.5f);
        //add animation
        currentPlayerUnit.gameObject.SetActive(false);
        currentPlayerUnit = Healer;
        Healer.gameObject.SetActive(true);
        
    }
    
}
