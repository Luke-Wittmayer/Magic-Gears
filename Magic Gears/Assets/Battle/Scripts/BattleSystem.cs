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
    private PlayerAnimationController playerAnimator;
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

        playerUnit = playerPrefab.GetComponent<Unit>();
        enemyUnit = enemyPrefab.GetComponent<Unit>();
        playerAnimator = playerPrefab.GetComponent<PlayerAnimationController>();
        enemyAnimator = enemyPrefab.GetComponent<EnemyAnimationController>();

        HUD.SetupHUD();

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerBasicAttack(){
        // Damage the enemy
        playerAnimator.BasicAttack();
        playerUnit.UpdateMana(2);
        HUD.SetMana(playerUnit.currentMana);
        yield return new WaitForSeconds(.5f);
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        HUD.SetEnemyHealth(enemyUnit.currentHP);
        enemyAnimator.Damaged();

        Debug.Log("The attack is successful on " + enemyUnit.unitName + "!");
        Debug.Log(enemyUnit.unitName + " now has " + enemyUnit.currentHP + " remaining.");

        if(isDead){
            state = BattleState.WON;
            Debug.Log("You win!");
            EndBattle();
        } else 
        {
            if(playerAnimator.UltimateState == 0){
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }else {
                PlayerTurn();
            }
        }
    }


    void PlayerStealManaAttack(){
        // Damage the enemy
        playerAnimator.ManaStealAttack();
        playerUnit.UpdateMana(4);
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage/2);
        HUD.SetMana(playerUnit.currentMana);
        HUD.SetEnemyHealth(enemyUnit.currentHP);
        enemyAnimator.Damaged();

        Debug.Log("The attack is successful on " + enemyUnit.unitName + "!");
        Debug.Log(enemyUnit.unitName + " now has " + enemyUnit.currentHP + " remaining.");

        if(isDead){
            state = BattleState.WON;
            Debug.Log("You win!");
            EndBattle();
        } else 
        {
            if(playerAnimator.UltimateState == 0){
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }else {
                PlayerTurn();
            }
        }
    }

    IEnumerator PlayerSpendManaAttack(){
        if(playerUnit.currentMana < 4){
            yield break;
        }
        // Damage the enemy
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage*2);
        playerAnimator.SpendManaAttack();
        yield return new WaitForSeconds(.5f);
        playerUnit.UpdateMana(-4);
        HUD.SetMana(playerUnit.currentMana);
        HUD.SetEnemyHealth(enemyUnit.currentHP);
        enemyAnimator.Damaged();

        Debug.Log("The attack is successful on " + enemyUnit.unitName + "!");
        Debug.Log(enemyUnit.unitName + " now has " + enemyUnit.currentHP + " remaining.");

        if(isDead){
            state = BattleState.WON;
            Debug.Log("You win!");
            EndBattle();
        } else 
        {
            if(playerAnimator.UltimateState == 0){
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }else {
                PlayerTurn();
            }
        }
    }

        IEnumerator PlayerUltimateAttack(){
        if(playerUnit.currentMana < 10){
            yield break;
        }
        // Damage the enemy
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage*2);
        playerAnimator.UltimateAttack();
        yield return new WaitForSeconds(.5f);
        playerUnit.UpdateMana(-10);
        HUD.SetMana(playerUnit.currentMana);
        HUD.SetEnemyHealth(enemyUnit.currentHP);
        enemyAnimator.Damaged();

        Debug.Log("The attack is successful on " + enemyUnit.unitName + "!");
        Debug.Log(enemyUnit.unitName + " now has " + enemyUnit.currentHP + " remaining.");

        if(isDead){
            state = BattleState.WON;
            Debug.Log("You win!");
            EndBattle();
        } else 
        {
            PlayerTurn();
        }
    }

    IEnumerator EnemyTurn(){
        Debug.Log("Enemy unit attacks!");
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(.5f);
        playerAnimator.Damaged();
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        HUD.SetPlayerHealth(playerUnit.currentHP);

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
            enemyAnimator.Dead();
            playerAnimator.Win();
        } else if (state == BattleState.LOST){
            playerAnimator.Lose();
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
        StartCoroutine(PlayerBasicAttack());
    }

    public void OnStealManaButton(){
        if(state != BattleState.PLAYERTURN){
            return;
        }
        PlayerStealManaAttack();
    }

    public void OnSpendManaButton(){
        if(state != BattleState.PLAYERTURN){
            return;
        }
        StartCoroutine(PlayerSpendManaAttack());
    }

    public void OnUltimateAttack(){
        if(state != BattleState.PLAYERTURN){
            return;
        }
        StartCoroutine(PlayerUltimateAttack());
    }


}
