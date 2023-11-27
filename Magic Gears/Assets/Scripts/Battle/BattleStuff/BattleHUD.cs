using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    public ParticleSystem switchParticles;
    public TMP_Text playerManaText;
    public TMP_Text playerHealthText;
    public TMP_Text enemyManaText;
    public TMP_Text enemyHealthText;
    public TMP_Text Log;
    public GameObject DPSButton;
    public GameObject HealerButton;
    public GameObject TankButton;
    public bool healTank;
    public bool healDPS;
    public int healAllAmount;

    

    void Update() {
        if(battle.state == BattleState.WON) {
            victoryObj.SetActive(true);
        }
        else if(battle.state == BattleState.LOST) {
            lostObj.SetActive(true);
        }
    }
    public void SetupHUD(){
        Log.text = "Begin Battle!";
        Debug.Log(storeLevel.level);
        if(storeLevel.level >= 1) {
            DPSButton.SetActive(true);
            HealerButton.SetActive(true);
        }
        else {
            DPSButton.SetActive(false);
            HealerButton.SetActive(false);
            TankButton.SetActive(false);
        }
        if(storeLevel.level >= 2) {
            TankButton.SetActive(true);
        }
        else {
            TankButton.SetActive(false);
        }
        updateAllValues();
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
        playerManaText.text = Unit.currentPlayerMana.ToString();
    }

    public void SetPlayerHealth() {
        float healthf = (float)currentPlayerUnit.currentHP;
        playerHealth.fillAmount = healthf/currentPlayerUnit.maxHP;
        playerHealthText.text = currentPlayerUnit.currentHP.ToString();
    }

    public void SetEnemyMana(){
        float manaf = (float)enemyUnit.currentEnemyMana;
        enemyMana.fillAmount = manaf/enemyUnit.maxEnemyMana;
        enemyManaText.text = enemyUnit.currentEnemyMana.ToString();
    }

    public void SetEnemyHealth() {
        float healthf = (float)enemyUnit.currentHP;
        enemyHealth.fillAmount = healthf/enemyUnit.maxHP;
        enemyHealthText.text = enemyUnit.currentHP.ToString();
    }

    public void GoToHUB() {
        SceneManager.LoadScene("Hub");
    }

    public void ReloadScene() {
        Scene curScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(curScene.buildIndex);
    }

    public void LoadNextScene() {
        Scene curScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(curScene.buildIndex + 1);
    }

    public void switchToDPS(Unit DPS) {
        //For the chest enemy case. Prevent player from switching to this character if the character is swallowed
        if (DPS.playerIsSwallowed)
        {
            Debug.Log("Cannot switch to a swallowed ally");
            return;
        }
        if (DPS == currentPlayerUnit || battle.state != BattleState.PLAYERTURN) {
            return;
        }
        else {
            Debug.Log("Switching");
            battle.state = BattleState.SWITCHING;
            StartCoroutine(switchToDPSE(DPS));
        }
    }

    IEnumerator switchToDPSE(Unit DPS) {
        Debug.Log("Switching");
        switchParticles.Stop();
        if(!switchParticles.isPlaying) {
            switchParticles.Play();
            Debug.Log("Particle");
        }
        yield return new WaitForSeconds(1.6f);
        //add animation
        currentPlayerUnit.gameObject.SetActive(false);
        currentPlayerUnit.AttackButtons.SetActive(false);
        currentPlayerUnit = DPS;
        DPS.gameObject.SetActive(true);
        DPS.AttackButtons.SetActive(true);
        currentPlayerUnit.currentPlayerUnit = currentPlayerUnit;
        enemyUnit.currentPlayerUnit = currentPlayerUnit;
        enemyUnit.playerAnimator = currentPlayerUnit.playerAnimator;
        battle.playerAnimator = currentPlayerUnit.playerAnimator;
        if (healDPS)
        {
            currentPlayerUnit.TakeDamage(healAllAmount);
           // currentPlayerUnit.currentPlayerUnit = currentPlayerUnit;
            updateAllHealth();
            SetPlayerHealth();
            healDPS = false; 
        }
        updateAllValues();
        battle.state = BattleState.PLAYERTURN;
    }

    public void switchToHealer(Unit Healer) {
        //For the chest enemy case. Prevent player from switching to this character if the character is swallowed
        if (Healer.playerIsSwallowed)
        {
            Debug.Log("Cannot switch to a swallowed ally");
            return;
        }
        if (Healer == currentPlayerUnit || battle.state != BattleState.PLAYERTURN) {
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
        switchParticles.Stop();
        if(!switchParticles.isPlaying) {
            switchParticles.Play();
            Debug.Log("Particle");
        }
        
        yield return new WaitForSeconds(1.6f);
        //add animation
        currentPlayerUnit.gameObject.SetActive(false);
        currentPlayerUnit.AttackButtons.SetActive(false);
        currentPlayerUnit = Healer;
        Healer.gameObject.SetActive(true);
        Healer.AttackButtons.SetActive(true);
        currentPlayerUnit.currentPlayerUnit = currentPlayerUnit;
        enemyUnit.currentPlayerUnit = currentPlayerUnit;
        enemyUnit.playerAnimator = currentPlayerUnit.playerAnimator;
        battle.playerAnimator = currentPlayerUnit.playerAnimator;
        updateAllValues();
        battle.state = BattleState.PLAYERTURN;
    }

    public void switchToTank(Unit Tank) {
        //For the chest enemy case. Prevent player from switching to this character if the character is swallowed
        if (Tank.playerIsSwallowed)
        {
            Debug.Log("Cannot switch to a swallowed ally");
            return;
        }
        if (Tank == currentPlayerUnit || battle.state != BattleState.PLAYERTURN) {
            Debug.Log("No");
            return;
        }
        else {
            Debug.Log("Switch");
            battle.state = BattleState.SWITCHING;
            StartCoroutine(switchToTankE(Tank));
        }
    }
    IEnumerator switchToTankE(Unit Tank) {

        Debug.Log("Switching");
        switchParticles.Stop();
        if(!switchParticles.isPlaying) {
            switchParticles.Play();
            Debug.Log("Particle");
        }
        yield return new WaitForSeconds(1.6f);
        //add animation
        currentPlayerUnit.gameObject.SetActive(false);
        currentPlayerUnit.AttackButtons.SetActive(false);
        currentPlayerUnit = Tank;
        Tank.gameObject.SetActive(true);
        Tank.AttackButtons.SetActive(true);
        currentPlayerUnit.currentPlayerUnit = currentPlayerUnit;
        enemyUnit.currentPlayerUnit = currentPlayerUnit;
        enemyUnit.playerAnimator = currentPlayerUnit.playerAnimator;
        battle.playerAnimator = currentPlayerUnit.playerAnimator;
        if (healTank)
        {
            currentPlayerUnit.TakeDamage(healAllAmount);
            updateAllHealth();
            SetPlayerHealth();
            healTank = false;
        }
        updateAllValues();
        battle.state = BattleState.PLAYERTURN;
    }
    
}
