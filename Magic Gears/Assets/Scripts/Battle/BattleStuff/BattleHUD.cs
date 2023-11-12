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
        playerMana.fillAmount = (float)Unit.currentPlayerMana/Unit.maxPlayerMana;
        playerHealth.fillAmount = (float)currentPlayerUnit.currentHP/currentPlayerUnit.maxHP;
        enemyMana.fillAmount = (float)enemyUnit.currentEnemyMana/enemyUnit.maxEnemyMana;
        enemyHealth.fillAmount = (float)enemyUnit.currentHP/enemyUnit.maxHP;
    }

    public void updateAllHealth() {
        SetPlayerHealth();
        SetEnemyHealth();
    }

    public void updateAllMana() {
        SetEnemyMana();
        SetPlayerMana();
    }

    public void updateAllValues() {
        SetEnemyHealth();
        SetEnemyMana();
        SetPlayerHealth();
        SetPlayerMana();
    }


    public void SetPlayerMana(){
        float manaf = (float)Unit.currentPlayerMana;
        playerMana.fillAmount = manaf/Unit.maxPlayerMana;
    }

    public void SetPlayerHealth() {
        float healthf = (float)currentPlayerUnit.currentHP;
        playerHealth.fillAmount = healthf/currentPlayerUnit.maxHP;
    }

    public void SetEnemyMana(){
        float manaf = (float)enemyUnit.currentEnemyMana;
        enemyMana.fillAmount = manaf/enemyUnit.maxEnemyMana;
    }

    public void SetEnemyHealth() {
        float healthf = (float)enemyUnit.currentHP;
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
            updateAllValues();
        }
    }

    public void switchToHealer(Unit Healer) {
        if(Healer == currentPlayerUnit || battle.state != BattleState.PLAYERTURN) {
            Debug.Log("No");
            return;
        }
        else {
            Debug.Log("Switch");
            battle.state = BattleState.SWITCHING;
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
        updateAllValues();
        battle.state = BattleState.PLAYERTURN;
    }
    
}
