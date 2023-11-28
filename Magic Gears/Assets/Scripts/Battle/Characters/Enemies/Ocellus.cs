using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ocellus : Enemy
{

    public int poisonDamage;
    private bool poisoned = false;
    public int offHP;

    public int healTurns;
    public int maxHealTurns;
    public int healAmount; //NOTE MUST BE NEGATIVE NUMBER
    public int defMana; //PROBABLY THIS ONE TOO

    public ParticleSystem healing;
    public ParticleSystem poison;

    public override void chooseAttack()
    {
        base.StateMachine4();

        if (currentAtk == CurrentAtk.BASIC)
        {
            Atk1();
        }
        else if (currentAtk == CurrentAtk.DEFENSE)
        {
            Atk2();
        }
        else if (currentAtk == CurrentAtk.OFFENSE)
        {
            Atk3();
        }
        else if (currentAtk == CurrentAtk.ULTIMATE)
        {
            Atk4();
        }
        base.chooseAttack();
    }

    public override void Atk1()
    {
        if (battlesystem.state != BattleState.ENEMYTURN)
        {
            Debug.Log("Error");
            return;
        }
        StartCoroutine(EnemyAttack1());
    }
    public override void Atk2()
    {
        if (battlesystem.state != BattleState.ENEMYTURN)
        {
            Debug.Log("Error");
            return;
        }

        StartCoroutine(EnemyAttack2());
    }
    public override void Atk3()
    {
        if (battlesystem.state != BattleState.ENEMYTURN)
        {
            Debug.Log("Error");
            return;
        }
        StartCoroutine(EnemyAttack3());
    }

    public override void Atk4()
    {
        if (battlesystem.state != BattleState.ENEMYTURN)
        {
            Debug.Log("Error");
            return;
        }
        StartCoroutine(EnemyAttack4());
    }

    public IEnumerator EnemyAttack1()
    {
        //Enemy basic attack gains 5 mana
        HUD.Log.text = "Ocellus attacks elegantly!";
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        audioSource.PlayOneShot(atk1Audio);
        yield return new WaitForSeconds(.5f);
        Debug.Log("Enemy gains " + manaCostBasic + "mana");
        UpdateEnemyMana(manaCostBasic);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        bool isDead = currentPlayerUnit.TakeDamage(damageBasic);
        HUD.Log.text = currentPlayerUnit.unitName + " takes " + damageBasic + " damage!";
        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            HUD.Log.text = "Game over!";
            battlesystem.state = BattleState.LOST;
            Debug.Log("You lose!");
            battlesystem.EndBattle();
        }
        else
        {
            battlesystem.state = BattleState.PLAYERTURN;
            battlesystem.PlayerTurn();
        }

        StartCoroutine(healIsOn());
        StartCoroutine(posionIsOn());
    }


    public IEnumerator EnemyAttack2()
    {
        HUD.Log.text ="Ocellus is looking ahead to gain health and mana for " + maxHealTurns + " turns!";
        yield return new WaitForSeconds(2f);
        enemyAnimator.EnemyDefensiveAttack();
        audioSource.PlayOneShot(atk2Audio);
        yield return new WaitForSeconds(0.5f);
        // Debug.Log("Enemy loose " + manaCostDefense + "mana");
        UpdateEnemyMana(manaCostDefense);
        HUD.SetEnemyMana();
        //Mushroom called the offense attack while player was still poisioned, increase the poision damage
        if (healTurns <= 0)
        {
            healTurns = maxHealTurns;
        }
        else
        {
            healTurns++;
        }
        healing.Play();
        battlesystem.state = BattleState.PLAYERTURN;
        battlesystem.PlayerTurn();

        StartCoroutine(healIsOn());
        StartCoroutine(posionIsOn());
    }

    public IEnumerator EnemyAttack3()
    {
        //Big damage
        //Enemy basic attack gains 5 mana
        HUD.Log.text = "Ocellus borrowed " + offHP +" health point from " + currentPlayerUnit.unitName + "!";
        yield return new WaitForSeconds(2f);
        enemyAnimator.EnemyOffensiveAttack();
        audioSource.PlayOneShot(atk3Audio);
        yield return new WaitForSeconds(.5f);
        UpdateEnemyMana(manaCostOffense);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        bool isDead = currentPlayerUnit.TakeDamage(offHP);
        bool increaseHP = enemyUnit.TakeDamage(offHP*-1);
        //HUD.SetPlayerHealth();

        if (isDead)
        {
            HUD.Log.text = "Game over!";
            battlesystem.state = BattleState.LOST;
            Debug.Log("You lose!");
            battlesystem.EndBattle();
        }
        else
        {
            battlesystem.state = BattleState.PLAYERTURN;
            battlesystem.PlayerTurn();
        }
        StartCoroutine(healIsOn());
        StartCoroutine(posionIsOn());
    }

    public IEnumerator EnemyAttack4()
    {
        HUD.Log.text = "Ocellus poisioned " + currentPlayerUnit.unitName + " for being annoying the rest of combat!";
        yield return new WaitForSeconds(1.5f);
        enemyAnimator.EnemyUltimateAttack();
        audioSource.PlayOneShot(ultAudio);
        yield return new WaitForSeconds(0.5f);
        UpdateEnemyMana(manaCostUltimate);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        //Mushroom called the offense attack while player was still poisioned, increase the poision damage
        if (poisoned)
        {
            //Stack the posion
            poisonDamage = poisonDamage + poisonDamage;
        }
        poison.Play();
        battlesystem.state = BattleState.PLAYERTURN;
        battlesystem.PlayerTurn();
        poisoned = true;

        StartCoroutine(healIsOn());
        StartCoroutine(posionIsOn());
    }

    public IEnumerator posionIsOn()
    {
        if (poisoned)
        {

     //       Debug.Log("BEFORE Poisioned: " + currentPlayerUnit.currentHP + " health");
            bool isDead = currentPlayerUnit.TakeDamage(poisonDamage);
   //         Debug.Log("AFTER Poisioned: " + currentPlayerUnit.currentHP + " health");
            HUD.Log.text = currentPlayerUnit.unitName + " takes " + poisonDamage + " damage from poison!";
            playerAnimator.Damaged();
            yield return new WaitForSeconds(2f);
            HUD.Log.text = "Player turn!";
            if (isDead)
            {
                HUD.Log.text = "Game over!";
                battlesystem.state = BattleState.LOST;
                Debug.Log("You lose!");
                battlesystem.EndBattle();
            }
        }
    }

    public IEnumerator healIsOn()
    {
        if (healTurns > 0)
        {
            HUD.Log.text = "Ocellus gains " + (-1 * healAmount) + " of health points and " + (-1*defMana) + " of mana!";
            yield return new WaitForSeconds(2f);
            Debug.Log("BEFORE heal: " + enemyUnit.currentHP + " health");
            bool isDead = enemyUnit.TakeDamage(healAmount);
            UpdateEnemyMana(defMana);
            Debug.Log("AFTER heal: " + enemyUnit.currentHP + " health");
            HUD.Log.text = "Player turn!";
            HUD.updateAllHealth();
            HUD.SetEnemyMana();
            healTurns--;
            if(healTurns == 0) {
                healing.Stop();
            }
        }
        yield return new WaitForSeconds(0f);
    }
}
