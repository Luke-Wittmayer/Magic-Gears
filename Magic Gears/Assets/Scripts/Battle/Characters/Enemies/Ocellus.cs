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
        HUD.Log.text = "Cactus attacks!";
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(.5f);
        Debug.Log("Enemy gains " + manaCostBasic + "mana");
        UpdateEnemyMana(manaCostBasic);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        bool isDead = currentPlayerUnit.TakeDamage(damageBasic);
        HUD.Log.text = "You take " + damageBasic + " damage!";
        yield return new WaitForSeconds(2f);

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

        posionIsOn();
        healIsOn();
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
        if (healTurns <= 0)
        {
            healTurns = maxHealTurns;
        }
        else
        {
            healTurns++;
        }

        battlesystem.state = BattleState.PLAYERTURN;
        battlesystem.PlayerTurn();

        posionIsOn();
        healIsOn();
    }

    public IEnumerator EnemyAttack3()
    {
        //Big damage
        //Enemy basic attack gains 5 mana
        Debug.Log("Enemy unit steal attack is big!");
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(.5f);
        UpdateEnemyMana(manaCostOffense);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        bool isDead = currentPlayerUnit.TakeDamage(offHP);
        bool increaseHP = enemyUnit.TakeDamage(offHP*-1);
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
        posionIsOn();
        healIsOn();
    }

    public IEnumerator EnemyAttack4()
    {
        Debug.Log("The mushroom has posioned you forever");
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
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

        battlesystem.state = BattleState.PLAYERTURN;
        battlesystem.PlayerTurn();
        poisoned = true;

        posionIsOn();
        healIsOn();
    }

    public void posionIsOn()
    {
        if (poisoned)
        {

            Debug.Log("BEFORE Poisioned: " + currentPlayerUnit.currentHP + " health");
            bool isDead = currentPlayerUnit.TakeDamage(poisonDamage);
            Debug.Log("AFTER Poisioned: " + currentPlayerUnit.currentHP + " health");
            if (isDead)
            {
                battlesystem.state = BattleState.LOST;
                Debug.Log("You lose!");
                battlesystem.EndBattle();
            }
            playerAnimator.Damaged();
        }
    }

    public void healIsOn()
    {
        if (healTurns > 0)
        {
            Debug.Log("BEFORE heal: " + enemyUnit.currentHP + " health");
            bool isDead = enemyUnit.TakeDamage(healAmount);
            UpdateEnemyMana(defMana);
            Debug.Log("AFTER heal: " + enemyUnit.currentHP + " health");
            HUD.updateAllHealth();
            HUD.SetEnemyMana();
            healTurns--;

        }
    }
}
