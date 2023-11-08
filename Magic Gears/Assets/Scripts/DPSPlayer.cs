using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DPSPlayer : Unit
{
    public BattleSystem battlesystem;
    
    public override void Atk1() {
        if (battlesystem.state != BattleState.PLAYERTURN){
            return;
        }
        StartCoroutine(DPSBasicAttack());
    }

    IEnumerator DPSBasicAttack() {
        // Set enemy turn to prevent spam clicking
        battlesystem.state = BattleState.ENEMYTURN;
        // Damage the enemy
        playerAnimator.BasicAttack();
        this.UpdateMana(2);
        HUD.SetPlayerMana(this.currentMana);
        yield return new WaitForSeconds(.5f);
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        HUD.SetEnemyHealth(enemyUnit.currentHP);
        enemyAnimator.Damaged();

        Debug.Log("The attack is successful on " + enemyUnit.unitName + "!");
        Debug.Log(enemyUnit.unitName + " now has " + enemyUnit.currentHP + " remaining.");

        if(isDead){
            battlesystem.state = BattleState.WON;
            Debug.Log("You win!");
            EndBattle();
        } else 
        {
            if(playerAnimator.UltimateState == 0){
                StartCoroutine(EnemyTurn());
            }else {
                PlayerTurn();
            }
        }
    }
}
