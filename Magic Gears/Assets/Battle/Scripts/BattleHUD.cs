using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Image manaFill;
    public Image playerHealth;
    public Image enemyHealth;
    public Unit enemyUnit;
    public Unit playerUnit;

    public void SetupHUD(){
        manaFill.fillAmount = 0f;
        playerHealth.fillAmount = 1f;
        enemyHealth.fillAmount = 1f;
    }


    public void SetMana(int mana){
        float manaf = (float)mana;
        manaFill.fillAmount = manaf/playerUnit.maxMana;
    }

    public void SetPlayerHealth(int health) {
        float healthf = (float)health;
        playerHealth.fillAmount = healthf/playerUnit.maxHP;
    }

    public void SetEnemyHealth(int health) {
        float healthf = (float)health;
        enemyHealth.fillAmount = healthf/enemyUnit.maxHP;
    }
    
}
