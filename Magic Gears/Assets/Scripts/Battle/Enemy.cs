using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public int maxEnemyMana;
    public int currentEnemyMana;
    public virtual void Attacker(){

    }

    public void UpdateEnemyMana(int mana){
        currentEnemyMana += mana;
        if(currentEnemyMana < 0){
            currentEnemyMana = 0;
        }
    }

}
