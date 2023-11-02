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
    public Unit enemyUnit;
    public Unit playerUnit;
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
        playerMana.fillAmount = manaf/playerUnit.maxMana;
    }

    public void SetPlayerHealth(int health) {
        float healthf = (float)health;
        playerHealth.fillAmount = healthf/playerUnit.maxHP;
    }

    public void SetEnemyMana(int mana){
        float manaf = (float)mana;
        enemyMana.fillAmount = manaf/enemyUnit.maxMana;
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
    
}
