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
            bool isDead = currentPlayerUnit.TakeDamage(dmg);
            reflectState = ReflectState.NO;
            playerAnimator.Damaged();
            if (isDead){
                HUD.Log.text = "Game over!";
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
        HUD.Log.text = "Cactus attacks wanting some meat!";
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        audioSource.PlayOneShot(atk1Audio);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Enemy gains " + manaCostBasic + "mana");
        UpdateEnemyMana(manaCostBasic);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        bool isDead = currentPlayerUnit.TakeDamage(damageBasic);
        HUD.Log.text = currentPlayerUnit.unitName+ " takes " + damageBasic + " damage!";
        yield return new WaitForSeconds(2f);

        if(isDead){
            HUD.Log.text = "Game over!";
            battlesystem.state = BattleState.LOST; 
            Debug.Log("You lose!");
            battlesystem.EndBattle();
        }else {
            battlesystem.state = BattleState.PLAYERTURN;
            battlesystem.PlayerTurn();
        }
    }

    public IEnumerator EnemyAttack2(){
        HUD.Log.text = "Cactus is ready to reflect any attack!";
        enemyAnimator.EnemyDefensiveAttack();
        audioSource.PlayOneShot(atk2Audio);
        yield return new WaitForSeconds(2f);
        UpdateEnemyMana(manaCostDefense);
        HUD.SetEnemyMana();

        reflectState = ReflectState.YES;

        battlesystem.state = BattleState.PLAYERTURN;
        battlesystem.PlayerTurn();
            
        
    }

    public IEnumerator EnemyAttack3(){
        //Big damage
        //Enemy basic attack gains 5 mana
        HUD.Log.text = "Cactus wants to eat quickly, so his attack is big!";
        yield return new WaitForSeconds(2f);
        audioSource.PlayOneShot(atk3Audio);
        enemyAnimator.EnemyOffensiveAttack();
        yield return new WaitForSeconds(.5f);
        UpdateEnemyMana(manaCostOffense);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        bool isDead = currentPlayerUnit.TakeDamage(damageBig);
        HUD.Log.text = currentPlayerUnit.unitName + " takes " + damageBig + " damage!";
        yield return new WaitForSeconds(2f);
        //HUD.SetPlayerHealth();

        if (isDead){
            HUD.Log.text = "Game over!";
            battlesystem.state = BattleState.LOST; 
            Debug.Log("You lose!");
            battlesystem.EndBattle();
        }else {
            battlesystem.state = BattleState.PLAYERTURN;
            battlesystem.PlayerTurn();
        }
    }
}
