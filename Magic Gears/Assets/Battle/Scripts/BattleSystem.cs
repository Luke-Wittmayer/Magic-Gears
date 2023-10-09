using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, TARGETSELECT }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;
    Unit target;
    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetUpBattle();
    }

    void SetUpBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerBasicAttack(){
        // Damage the enemy
        bool isDead = target.TakeDamage(playerUnit.damage);

        Debug.Log("The attack is successful on " + target.unitName + "!");

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

    public void SelectTarget(Unit selection){
        Debug.Log("Clicked!");
        if( state != BattleState.TARGETSELECT){
            Debug.Log("Stopping!");
            Debug.Log(state);
            return;
        } else {
            Debug.Log("Continuing!");
            target = selection;
            PlayerBasicAttack();
        }
    }

    void PlayerTurn()
    {
    }

    public void OnBasicAttackButton()
    {
        if (state != BattleState.PLAYERTURN){
            return;
        }
        Debug.Log("Initiating Attack!");
        state = BattleState.TARGETSELECT;
    }


}
