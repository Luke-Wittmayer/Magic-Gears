using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy
{

    public int posionTurns;
    public int maxPoisonTurns;
    public int poisonAttack;
    public int poisonPlus;

    public int healTurns;
    public int maxHealTurns;
    public int healAmount; //NOTE MUST BE NEGATIVE NUMBER
    public int healPlus;

    public override void chooseAttack()
    {
        base.StateMachine3();

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
    }

    //Player is still poisioned. Take damage
    public void posionIsOn()
    {
        if (posionTurns > 0)
        {

            Debug.Log("BEFORE Poisioned: " + currentPlayerUnit.currentHP + " health");
            bool isDead = currentPlayerUnit.TakeDamage(poisonAttack);
            Debug.Log("AFTER Poisioned: " + currentPlayerUnit.currentHP + " health");
            if (isDead)
            {
                battlesystem.state = BattleState.LOST;
                Debug.Log("You lose!");
                battlesystem.EndBattle();
            }
            posionTurns--;
            playerAnimator.Damaged();
        }
    }

    //Mushroom still have the defense move active. Increase health
    public void healIsOn()
    {
        if (healTurns > 0)
        {
            Debug.Log("BEFORE heal: " + enemyUnit.currentHP + " health");
            bool isDead = enemyUnit.TakeDamage(healAmount);
            Debug.Log("AFTER heal: " + enemyUnit.currentHP + " health");
            HUD.updateAllHealth();
            healTurns--;

        }
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

    public IEnumerator EnemyAttack1()
    {
        Debug.Log("Enemy unit attacks!");
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Enemy gains " + manaCostBasic + "mana");
        UpdateEnemyMana(manaCostBasic);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        bool isDead = currentPlayerUnit.TakeDamage(damageBasic);
        //HUD.SetPlayerHealth();
        if (isDead)
        {
            battlesystem.state = BattleState.LOST;
            Debug.Log("You lose!");
            battlesystem.EndBattle();
        }
        else
        {
            battlesystem.state = BattleState.PLAYERTURN;
            battlesystem.PlayerTurn();
        }

        healIsOn();
        posionIsOn();
    }

    public IEnumerator EnemyAttack2()
    {
        //Debug.Log("The mushroom is healing for " + maxHealTurns + " turns");
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(0.5f);
      // Debug.Log("Enemy loose " + manaCostDefense + "mana");
        UpdateEnemyMana(manaCostDefense);
        HUD.SetEnemyMana();
        //Mushroom called the offense attack while player was still poisioned, increase the poision damage
        if (healTurns > 0)
        {
            healAmount = healAmount + healPlus;
        }
        healTurns = maxHealTurns;
        battlesystem.state = BattleState.PLAYERTURN;
        battlesystem.PlayerTurn();

        posionIsOn();
        healIsOn();
    }

    public IEnumerator EnemyAttack3()
    {
       Debug.Log("The mushroom has posioned you for " + maxPoisonTurns + " turns");
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(0.5f);
       Debug.Log("Enemy loose " + manaCostOffense + "mana");
        UpdateEnemyMana(manaCostOffense);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        //Mushroom called the offense attack while player was still poisioned, increase the poision damage
        if (posionTurns > 0)
        {
            poisonAttack = poisonAttack + poisonPlus;
        }

        bool isDead = currentPlayerUnit.TakeDamage(damageBasic);
        //HUD.SetPlayerHealth();

        if (isDead)
        {
            battlesystem.state = BattleState.LOST;
            Debug.Log("You lose!");
            battlesystem.EndBattle();
        }
        else
        {
            battlesystem.state = BattleState.PLAYERTURN;
            battlesystem.PlayerTurn();
        }
        posionTurns = maxPoisonTurns;

        posionIsOn();
        healIsOn();
    }
}
