using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cactus : Enemy
{
    public enum ReflectState {YES, NO}

    public ReflectState reflectState;
    private int atk;

    void Start() {
        reflectState = ReflectState.NO;
        atk = 1;
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
            currentPlayerUnit.TakeDamage(dmg);
            Debug.Log("Reflecting: " + currentPlayerUnit.currentHP + " health");
            reflectState = ReflectState.NO;
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
        Debug.Log("Enemy unit attacks!");
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(.5f);
        Debug.Log("Enemy gains " + manaCostBasic + "mana");
        UpdateEnemyMana(manaCostBasic);
        HUD.SetEnemyMana(currentEnemyMana);
        playerAnimator.Damaged();
        bool isDead = currentPlayerUnit.TakeDamage(damageBasic);
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

    public IEnumerator EnemyAttack2(){
        Debug.Log("Enemy unit reflects attack!");
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(.5f);
        UpdateEnemyMana(manaCostDefense);
        HUD.SetEnemyMana(currentEnemyMana);
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
        HUD.SetEnemyMana(currentEnemyMana);
        playerAnimator.Damaged();
        bool isDead = currentPlayerUnit.TakeDamage(damageBig);
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
