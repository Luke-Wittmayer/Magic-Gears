using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cactus : Enemy
{
    public enum ReflectState {YES, NO}

    public ReflectState reflectState;

    void Start() {
        reflectState = ReflectState.NO;
    }
    
    public int damageBig;

    public override void chooseAttack(){
        base.StateMachine3();

        if(currentAtk == CurrentAtk.BASIC) {
            Atk1();
        }
        else if(currentAtk == CurrentAtk.DEFENSE) {
            Atk2();
        }
        else if(currentAtk == CurrentAtk.OFFENSE) {
            Atk3();
        }
    }

    public override bool TakeDamage(int dmg) {
        //reflect damage
        if(reflectState == ReflectState.YES) {
            //reflected damage = damage taken
            Debug.Log("Reflecting: " + currentPlayerUnit.currentHP + " health");
            bool isDead = currentPlayerUnit.TakeDamage(dmg);
            Debug.Log("Reflecting: " + currentPlayerUnit.currentHP + " health");
            reflectState = ReflectState.NO;

            if(isDead){
                battlesystem.state = BattleState.LOST; 
                Debug.Log("You lose!");
                battlesystem.EndBattle();
                return false;
            }
        }
        return base.TakeDamage(dmg);
    }
    public override void Atk1() {
        if (battlesystem.state != BattleState.ENEMYTURN){
            Debug.Log("Error");
            return;
        }
        StartCoroutine(EnemyAttack1());
    }
    public override void Atk2() {
        if (battlesystem.state != BattleState.ENEMYTURN){
            Debug.Log("Error");
            return;
        }
        
        StartCoroutine(EnemyAttack2());
    }
    public override void Atk3() {
        if (battlesystem.state != BattleState.ENEMYTURN){
            Debug.Log("Error");
            return;
        }
        StartCoroutine(EnemyAttack3());
    }
    public IEnumerator EnemyAttack1(){
        //Enemy basic attack gains 5 mana
        HUD.Log.text = "Cactus attacks!";
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(.5f);
        Debug.Log("Enemy gains " + manaCostBasic + "mana");
        UpdateEnemyMana(manaCostBasic);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        bool isDead = currentPlayerUnit.TakeDamage(damageBasic);
        HUD.Log.text = "You take " + damageBasic + " damage!";
        yield return new WaitForSeconds(2f);

        if(isDead){
            battlesystem.state = BattleState.LOST; 
            Debug.Log("You lose!");
            battlesystem.EndBattle();
        }else {
            battlesystem.state = BattleState.PLAYERTURN;
            battlesystem.PlayerTurn();
        }
    }

    public IEnumerator EnemyAttack2(){
        Debug.Log("Enemy unit reflects attack!");
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(.5f);
        UpdateEnemyMana(manaCostDefense);
        HUD.SetEnemyMana();
        reflectState = ReflectState.YES;

        battlesystem.state = BattleState.PLAYERTURN;
        battlesystem.PlayerTurn();
            
        
    }

    public IEnumerator EnemyAttack3(){
        //Big damage
        //Enemy basic attack gains 5 mana
        Debug.Log("Enemy unit attacks big!");
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(.5f);
        UpdateEnemyMana(manaCostOffense);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        bool isDead = currentPlayerUnit.TakeDamage(damageBig);
        //HUD.SetPlayerHealth();

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
