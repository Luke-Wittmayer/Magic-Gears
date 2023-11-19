using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, SWITCHING }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    // public Transform playerBattleStation;
    // public Transform enemyBattleStation;

    public DPSPlayer playerUnit;
    public Unit enemyUnit;
    public PlayerAnimationController playerAnimator;
    private EnemyAnimationController enemyAnimator;

    public BattleState state;

    public BattleHUD HUD;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        playerPrefab.SetActive(true);
        SetUpBattle();
    }

    void SetUpBattle()
    {
        // GameObject playerGO = Instantiate(playerPrefab);
        // GameObject enemyGO = Instantiate(enemyPrefab);

        // playerGO.SetActive(true);
        // enemyGO.SetActive(true);

        playerUnit = playerPrefab.GetComponent<DPSPlayer>();
        enemyUnit = enemyPrefab.GetComponent<Unit>();
        playerAnimator = playerPrefab.GetComponent<PlayerAnimationController>();
        enemyAnimator = enemyPrefab.GetComponent<EnemyAnimationController>();

        Time.timeScale = 1f;
        Unit.maxPlayerMana = 100;
        Unit.currentPlayerMana = 0;
        HUD.SetupHUD();

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    public void EndBattle()
    {
        if (state == BattleState.WON)
        {
            enemyAnimator.Dead();
            playerAnimator.Win();
        }
        else if (state == BattleState.LOST)
        {
            playerAnimator.Lose();
        }
    }

    public void PlayerTurn()
    {
        HUD.Log.text = "Player turn!";
        state = BattleState.PLAYERTURN;
    }
    // IEnumerator DPSBasicAttack(){
    //     // Set enemy turn to prevent spam clicking
    //     state = BattleState.ENEMYTURN;
    //     // Damage the enemy
    //     playerAnimator.BasicAttack();
    //     playerUnit.UpdateMana(2);
    //     HUD.SetPlayerMana(playerUnit.currentMana);
    //     yield return new WaitForSeconds(.5f);
    //     bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
    //     HUD.SetEnemyHealth(enemyUnit.currentHP);
    //     enemyAnimator.Damaged();

    //     Debug.Log("The attack is successful on " + enemyUnit.unitName + "!");
    //     Debug.Log(enemyUnit.unitName + " now has " + enemyUnit.currentHP + " remaining.");

    //     if(isDead){
    //         state = BattleState.WON;
    //         Debug.Log("You win!");
    //         EndBattle();
    //     } else 
    //     {
    //         if(playerAnimator.UltimateState == 0){
    //             StartCoroutine(EnemyTurn());
    //         }else {
    //             PlayerTurn();
    //         }
    //     }
    // }


    // void DPSStealManaAttack(){
    //     // Set enemy turn to prevent spam clicking
    //     state = BattleState.ENEMYTURN;
    //     // Damage the enemy
    //     playerAnimator.ManaStealAttack();
    //     playerUnit.UpdateMana(4);
    //     bool isDead = enemyUnit.TakeDamage(playerUnit.damage/2);
    //     HUD.SetPlayerMana(playerUnit.currentMana);
    //     HUD.SetEnemyHealth(enemyUnit.currentHP);
    //     enemyAnimator.Damaged();

    //     Debug.Log("The attack is successful on " + enemyUnit.unitName + "!");
    //     Debug.Log(enemyUnit.unitName + " now has " + enemyUnit.currentHP + " remaining.");

    //     if(isDead){
    //         state = BattleState.WON;
    //         Debug.Log("You win!");
    //         EndBattle();
    //     } else 
    //     {
    //         if(playerAnimator.UltimateState == 0){
    //             state = BattleState.ENEMYTURN;
    //             StartCoroutine(EnemyTurn());
    //         }else {
    //             PlayerTurn();
    //         }
    //     }
    // }

    // IEnumerator DPSSpendManaAttack(){
    //     if(playerUnit.currentMana < 4){
    //         yield break;
    //     }
    //     // Set enemy turn to prevent spam clicking
    //     state = BattleState.ENEMYTURN;
    //     // Damage the enemy
    //     bool isDead = enemyUnit.TakeDamage(playerUnit.damage*2);
    //     playerAnimator.SpendManaAttack();
    //     yield return new WaitForSeconds(.5f);
    //     playerUnit.UpdateMana(-4);
    //     HUD.SetPlayerMana(playerUnit.currentMana);
    //     HUD.SetEnemyHealth(enemyUnit.currentHP);
    //     enemyAnimator.Damaged();

    //     Debug.Log("The attack is successful on " + enemyUnit.unitName + "!");
    //     Debug.Log(enemyUnit.unitName + " now has " + enemyUnit.currentHP + " remaining.");

    //     if(isDead){
    //         state = BattleState.WON;
    //         Debug.Log("You win!");
    //         EndBattle();
    //     } else 
    //     {
    //         if(playerAnimator.UltimateState == 0){
    //             state = BattleState.ENEMYTURN;
    //             StartCoroutine(EnemyTurn());
    //         }else {
    //             PlayerTurn();
    //         }
    //     }
    // }

    //     IEnumerator DPSUltimateAttack(){
    //     if(playerUnit.currentMana < 10){
    //         yield break;
    //     }
    //     // Set enemy turn to prevent spam clicking
    //     state = BattleState.ENEMYTURN;
    //     // Damage the enemy
    //     bool isDead = enemyUnit.TakeDamage(playerUnit.damage*2);
    //     playerAnimator.UltimateAttack();
    //     yield return new WaitForSeconds(.5f);
    //     playerUnit.UpdateMana(-10);
    //     HUD.SetPlayerMana(playerUnit.currentMana);
    //     HUD.SetEnemyHealth(enemyUnit.currentHP);
    //     enemyAnimator.Damaged();

    //     Debug.Log("The attack is successful on " + enemyUnit.unitName + "!");
    //     Debug.Log(enemyUnit.unitName + " now has " + enemyUnit.currentHP + " remaining.");

    //     if(isDead){
    //         state = BattleState.WON;
    //         Debug.Log("You win!");
    //         EndBattle();
    //     } else 
    //     {
    //         PlayerTurn();
    //     }
    // }

    // public IEnumerator EnemyAttack1(){
    //     Debug.Log("Enemy unit attacks!");
    //     yield return new WaitForSeconds(1f);
    //     enemyAnimator.EnemyBasicAttack();
    //     yield return new WaitForSeconds(.5f);
    //     playerAnimator.Damaged();
    //     bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
    //     HUD.SetPlayerHealth(playerUnit.currentHP);

    //     if(isDead){
    //         state = BattleState.LOST; 
    //         Debug.Log("You lose!");
    //         EndBattle();
    //     }else {
    //         state = BattleState.PLAYERTURN;
    //         PlayerTurn();
    //     }
    // }



    // public void OnAtkOne()
    // {
    //     if (state != BattleState.PLAYERTURN){
    //         return;
    //     }
    //     StartCoroutine(DPSBasicAttack());
    // }

    // public void OnAtkTwo(){
    //     if(state != BattleState.PLAYERTURN){
    //         return;
    //     }
    //     StartCoroutine(DPSSpendManaAttack());
    // }

    // public void OnAtkThree(){
    //     if(state != BattleState.PLAYERTURN){
    //         return;
    //     }
    //     DPSStealManaAttack();
    // }

    // public void OnAtkFour(){
    //     if(state != BattleState.PLAYERTURN){
    //         return;
    //     }
    //     StartCoroutine(DPSUltimateAttack());
    // }


}
