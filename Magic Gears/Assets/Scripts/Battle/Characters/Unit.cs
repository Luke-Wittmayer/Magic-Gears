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
 
    public int damageBasic;
    public int maxHP;
    public int currentHP;

    public static int maxPlayerMana;
    public static int currentPlayerMana;

    public int manaCostBasic;
    public int manaCostDefense;
    public int manaCostOffense;
    public int manaCostUltimate;

    public BattleHUD HUD;
    public GameObject AttackButtons;

    public AudioSource audioSource;
    public AudioClip atk1Audio;
    public AudioClip atk2Audio;
    public AudioClip atk3Audio;
    public AudioClip ultAudio;

    public bool playerIsSwallowed = false;

public virtual void Atk1() {

    }

    public virtual void Atk2() {

    }

    public virtual void Atk3() {
        
    }

    public virtual void Atk4() {
        
    }

    public virtual bool TakeDamage(int dmg){
        //Debug.Log(dmg);
        currentHP -= dmg;
        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        if (currentHP < 0)
        {
            currentHP = 0;
        }
        HUD.updateAllHealth();
        if(currentHP <= 0){
            return true;
        } else {
            return false;
        }
    }

    public void UpdatePlayerMana(int mana){
        currentPlayerMana -= mana;
        //check for negative mana values
        if(currentPlayerMana < 0){
            currentPlayerMana = 0;
        }
        //check for upper mana limit
        if(currentPlayerMana > maxPlayerMana) {
            currentPlayerMana = maxPlayerMana;
        }
    }

}
