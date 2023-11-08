using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public PlayerAnimationController playerAnimator;
    public EnemyAnimationController enemyAnimator;
    public BattleSystem battlesystem;
    public Enemy enemyUnit;
    public Unit currentPlayerUnit;
    public string unitName;

    public int damage; 
    public int maxHP;
    public int currentHP;

    public int maxMana;
    public int currentMana;

    public BattleHUD HUD;

    public virtual void Atk1() {

    }

    public virtual void Atk2() {

    }

    public virtual void Atk3() {
        
    }

    public virtual void Atk4() {

    }

    public bool TakeDamage(int dmg){
        Debug.Log(dmg);
        currentHP -= dmg;

        if(currentHP <= 0){
            return true;
        } else {
            return false;
        }
    }

    public void UpdateMana(int mana){
        currentMana += mana;
        if(currentMana < 0){
            currentMana = 0;
        }
    }

}
