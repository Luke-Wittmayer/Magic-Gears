using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, TARGETSELECT }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    // public Transform playerBattleStation;
    // public Transform enemyBattleStation;

    public Unit playerUnit;
    public Unit enemyUnit;

    public BattleState state;

    public BattleHUD playerHUD;

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

        playerUnit = playerPrefab.GetComponent<Unit>();
        enemyUnit = enemyPrefab.GetComponent<Unit>();

        playerHUD.SetHUD(playerUnit);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerBasicAttack(){
        // Damage the enemy
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        playerHUD.SetMana(2);

        Debug.Log("The attack is successful on " + enemyUnit.unitName + "!");
        Debug.Log(enemyUnit.unitName + " now has " + enemyUnit.currentHP + " remaining.");

        if(isDead){
            state = BattleState.WON;
            Debug.Log("You win!");
            EndBattle();
        } else 
        {
            state = BattleState.ENEMYTURN;
            EnemyTurn();
        }
    }

    void EnemyTurn(){
        Debug.Log("Enemy unit attacks!");

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        if(isDead){
            state = BattleState.LOST;
            Debug.Log("You lose!");
            EndBattle();
        }else {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle(){
        if(state == BattleState.WON){

        } else if (state == BattleState.LOST){

        }
    }

    // public void SelectTarget(Unit selection){
    //     Debug.Log("Clicked!");
    //     if( state != BattleState.TARGETSELECT){
    //         Debug.Log("Stopping!");
    //         Debug.Log(state);
    //         return;
    //     } else {
    //         Debug.Log("Continuing!");
    //         //target = selection;
    //         PlayerBasicAttack();
    //     }
    // }

    void PlayerTurn()
    {
    }

    public void OnBasicAttackButton()
    {
        if (state != BattleState.PLAYERTURN){
            return;
        }
        Debug.Log("Initiating Attack!");
        PlayerBasicAttack();
    }


}
