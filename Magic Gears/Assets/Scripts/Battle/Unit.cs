using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;

    public int damage; 
    public int maxHP;
    public int currentHP;

    public int maxMana;
    public int currentMana;

    public bool TakeDamage(int dmg){
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
