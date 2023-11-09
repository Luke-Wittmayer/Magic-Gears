using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : Enemy
{

    public override void Attacker(){
        Atk1();
    }
    public override void Atk1() {
        if (battlesystem.state != BattleState.ENEMYTURN){
            Debug.Log("Error");
            return;
        }
        StartCoroutine(EnemyAttack1());
    }
    public IEnumerator EnemyAttack1(){
        //Enemy basic attack gains 5 mana
        Debug.Log("Enemy unit attacks!");
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(.5f);
        UpdateEnemyMana(5);
        HUD.SetEnemyMana(currentEnemyMana);
        playerAnimator.Damaged();
        bool isDead = currentPlayerUnit.TakeDamage(enemyUnit.damage);
        HUD.SetPlayerHealth(currentPlayerUnit.currentHP);

        if(isDead){
            battlesystem.state = BattleState.LOST; 
            Debug.Log("You lose!");
            battlesystem.EndBattle();
        }else {
            battlesystem.state = BattleState.PLAYERTURN;
            battlesystem.PlayerTurn();
        }
    }
}
