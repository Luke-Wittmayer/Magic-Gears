using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DPSPlayer : Unit
{
    
    public override void Atk1() {
        if (battlesystem.state != BattleState.PLAYERTURN){
            Debug.Log("Error");
            return;
        }
        StartCoroutine(DPSBasicAttack());
    }
    public override void Atk2() {
        if (battlesystem.state != BattleState.PLAYERTURN){
            Debug.Log("Error");
            return;
        }
        Debug.Log("Running");
        StartCoroutine(DPSSpendManaAttack());
    }
    public override void Atk3() {
        if (battlesystem.state != BattleState.PLAYERTURN){
            return;
        }
        DPSStealManaAttack();        
    }
    public override void Atk4() {
        if (battlesystem.state != BattleState.PLAYERTURN){
            return;
        }
        StartCoroutine(DPSUltimateAttack());
    }

    IEnumerator DPSBasicAttack() {
        // Set enemy turn to prevent spam clicking
        battlesystem.state = BattleState.ENEMYTURN;
        // Damage the enemy
        playerAnimator.BasicAttack();
        UpdatePlayerMana(2);
        HUD.SetPlayerMana(currentPlayerMana);
        yield return new WaitForSeconds(.5f);
        bool isDead = enemyUnit.TakeDamage(this.damage);
        HUD.SetEnemyHealth(enemyUnit.currentHP);
        enemyAnimator.Damaged();

        Debug.Log("The attack is successful on " + enemyUnit.unitName + "!");
        Debug.Log(enemyUnit.unitName + " now has " + enemyUnit.currentHP + " remaining.");

        if(isDead){
            battlesystem.state = BattleState.WON;
            Debug.Log("You win!");
            battlesystem.EndBattle();
        } else 
        {
            if(playerAnimator.UltimateState == 0){
                enemyUnit.Attacker();
            }else {
                battlesystem.PlayerTurn();
            }
        }
    }

    void DPSStealManaAttack(){
        Debug.Log("In the function");
        // Set enemy turn to prevent spam clicking
        battlesystem.state = BattleState.ENEMYTURN;
        // Damage the enemy
        playerAnimator.ManaStealAttack();
        UpdatePlayerMana(4);
        bool isDead = enemyUnit.TakeDamage(this.damage/2);
        HUD.SetPlayerMana(currentPlayerMana);
        HUD.SetEnemyHealth(enemyUnit.currentHP);
        enemyAnimator.Damaged();
        //yield return new WaitForSeconds(0f);

        Debug.Log("The attack is successful on " + enemyUnit.unitName + "!");
        Debug.Log(enemyUnit.unitName + " now has " + enemyUnit.currentHP + " remaining.");

        if(isDead){
            battlesystem.state = BattleState.WON;
            Debug.Log("You win!");
            battlesystem.EndBattle();
        } else 
        {
            if(playerAnimator.UltimateState == 0){
                battlesystem.state = BattleState.ENEMYTURN;
                enemyUnit.Attacker();
            }else {
                battlesystem.PlayerTurn();
            }
        }
    }

    IEnumerator DPSSpendManaAttack(){
        if(currentPlayerMana < 4){
            yield break;
        }
        // Set enemy turn to prevent spam clicking
        battlesystem.state = BattleState.ENEMYTURN;
        // Damage the enemy
        bool isDead = enemyUnit.TakeDamage(this.damage*2);
        playerAnimator.SpendManaAttack();
        yield return new WaitForSeconds(.5f);
        UpdatePlayerMana(-4);
        HUD.SetPlayerMana(currentPlayerMana);
        HUD.SetEnemyHealth(enemyUnit.currentHP);
        enemyAnimator.Damaged();

        Debug.Log("The attack is successful on " + enemyUnit.unitName + "!");
        Debug.Log(enemyUnit.unitName + " now has " + enemyUnit.currentHP + " remaining.");

        if(isDead){
            battlesystem.state = BattleState.WON;
            Debug.Log("You win!");
            battlesystem.EndBattle();
        } else 
        {
            if(playerAnimator.UltimateState == 0){
                battlesystem.state = BattleState.ENEMYTURN;
                enemyUnit.Attacker();
            }else {
                battlesystem.PlayerTurn();
            }
        }
    }

    IEnumerator DPSUltimateAttack(){
        if(currentPlayerMana < 10){
            yield break;
        }
        // Set enemy turn to prevent spam clicking
        battlesystem.state = BattleState.ENEMYTURN;
        // Damage the enemy
        bool isDead = enemyUnit.TakeDamage(this.damage*2);
        playerAnimator.UltimateAttack();
        yield return new WaitForSeconds(.5f);
        UpdatePlayerMana(-10);
        HUD.SetPlayerMana(currentPlayerMana);
        HUD.SetEnemyHealth(enemyUnit.currentHP);
        enemyAnimator.Damaged();

        Debug.Log("The attack is successful on " + enemyUnit.unitName + "!");
        Debug.Log(enemyUnit.unitName + " now has " + enemyUnit.currentHP + " remaining.");

        if(isDead){
            battlesystem.state = BattleState.WON;
            Debug.Log("You win!");
            battlesystem.EndBattle();
        } else 
        {
            battlesystem.PlayerTurn();
        }
    }
}
