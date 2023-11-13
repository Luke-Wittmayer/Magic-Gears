using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public enum CurrentAtk {BASIC, DEFENSE, OFFENSE}
    public int maxEnemyMana;
    public int currentEnemyMana;

    public CurrentAtk currentAtk;

    void Start() {
        currentAtk = CurrentAtk.BASIC;
    }

    public virtual void chooseAttack() {

    }
    public void StateMachine3(){
        if(currentAtk == CurrentAtk.BASIC) {
            //if enemy health > player health, stay in BASIC
            if(enemyUnit.currentHP > currentPlayerUnit.currentHP) {
               // Debug.Log("Go Basic");
            }

            //if enemy health < 50% & mana check, do defense
            else if(enemyUnit.currentEnemyMana >= manaCostDefense &&
                    enemyUnit.currentHP < (enemyUnit.maxHP/2.0f)) {
                //        Debug.Log("half of max heaklth is " + enemyUnit.maxHP/2.0f);
                //        Debug.Log("Go Defense");
                        currentAtk = CurrentAtk.DEFENSE;
            }

            //if mana greater than offense cost, do offense
            else if(enemyUnit.currentEnemyMana >= manaCostOffense) {
              //  Debug.Log("Go Offense");
                currentAtk = CurrentAtk.OFFENSE;
            }

            else {
              //  Debug.Log("Stay Basic");
            }
        }

        else if(currentAtk == CurrentAtk.DEFENSE) {
            //if mana > offense cost, do offense
            if(enemyUnit.currentEnemyMana >= manaCostOffense) {
              //  Debug.Log("Go Offense");
                currentAtk = CurrentAtk.OFFENSE;
            }

            //if mana < defense go basic
            else if(enemyUnit.currentEnemyMana < manaCostDefense) {
              //  Debug.Log("Go Basic");
                currentAtk = CurrentAtk.BASIC;
            }

            else {
              //  Debug.Log("Stay Defense");
            }
        }

        else if(currentAtk == CurrentAtk.OFFENSE) {
            //if mana > defense cost, do defense
            if(enemyUnit.currentEnemyMana >= manaCostDefense) {
              //  Debug.Log("Go Defense");
                currentAtk = CurrentAtk.DEFENSE;
            }

            //if mana < offense cost, do basic
            else if(enemyUnit.currentEnemyMana < manaCostOffense) {
             //   Debug.Log("Go Basic");
                currentAtk = CurrentAtk.BASIC;
            }

            else {
              //  Debug.Log("Stay Offense");
            }
        }
    }

    public void StateMachine4(){}

    public void UpdateEnemyMana(int mana){
        currentEnemyMana -= mana;
        if(currentEnemyMana < 0){
            currentEnemyMana = 0;
        }
    }


}
