using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Unit
{
    public enum CurrentAtk {BASIC, DEFENSE, OFFENSE, ULTIMATE}
    public int maxEnemyMana;
    public int currentEnemyMana;
    public int prevMana;

    public CurrentAtk currentAtk;

    void Start() {
        currentAtk = CurrentAtk.BASIC;
    }

    public virtual void chooseAttack() {
        prevMana = currentEnemyMana;
    }
    public void StateMachine3(){
        if(currentAtk == CurrentAtk.BASIC) {
            //if enemy health > player health, stay in BASIC
            if(enemyUnit.currentHP > currentPlayerUnit.currentHP) {
               // Debug.Log("Go Basic");
            }

            //if enemy health < 50% & mana check, do defense
            else if(enemyUnit.currentEnemyMana >= manaCostDefense &&
                    enemyUnit.currentHP < enemyUnit.maxHP/2) {
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

    public void StateMachine4(){
        if(currentAtk == CurrentAtk.BASIC) {
            //if mana >= ultimate cost, go ultimate
            if (currentEnemyMana >= manaCostUltimate)
            {
                currentAtk = CurrentAtk.ULTIMATE;
            }
            //if mana < defense cost, stay basic
           else  if (enemyUnit.currentEnemyMana < manaCostDefense) {

          }
          
          //if mana > offense cost, go offense
          else if(enemyUnit.currentEnemyMana >= manaCostOffense) {
            currentAtk = CurrentAtk.OFFENSE;
          }

          //if enemy health < player health & mana > defense cost, go defense
          else if(enemyUnit.currentHP < currentPlayerUnit.currentHP &&
                  enemyUnit.currentEnemyMana >= manaCostDefense) {
                    currentAtk = CurrentAtk.DEFENSE;
          }
        }

        else if(currentAtk == CurrentAtk.DEFENSE) {
            //if mana >= ultimate cost, go ultimate
            if(currentEnemyMana >= manaCostUltimate) {
              currentAtk = CurrentAtk.ULTIMATE;
            }

            //if mana >= prevMana & mana > defense cost, stay defense
            else if(currentEnemyMana >= prevMana && currentEnemyMana >= manaCostDefense) {

            }

            //if mana < prevMana, go basic
            else if(currentEnemyMana < prevMana) {
              currentAtk = CurrentAtk.BASIC;
            }
        }

        else if(currentAtk == CurrentAtk.OFFENSE) {
            //if mana >= offense cost, stay offense
            if(currentEnemyMana >= manaCostOffense) {

            }

            //if mana < offense cost, go basic
            if(currentEnemyMana < manaCostOffense) {
              currentAtk = CurrentAtk.BASIC;
            }
        }

        else if(currentAtk == CurrentAtk.ULTIMATE) {
            //always to basic after ultimate
            currentAtk = CurrentAtk.BASIC;
        }
    }

    public void UpdateEnemyMana(int mana){
        currentEnemyMana -= mana;
        if(currentEnemyMana < 0){
            currentEnemyMana = 0;
        }
        if(currentEnemyMana > maxEnemyMana)
        {
            currentEnemyMana = maxEnemyMana;
        }
    }


}
