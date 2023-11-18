using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyOcellus : Unit
{

    public int healAllAmount;
    public int offHP;


    public int healTurns;
    public int maxHealTurns;
    public int healAmount; //NOTE MUST BE NEGATIVE NUMBER
    public int defMana; //PROBABLY THIS ONE TOO

    public override void Atk1()
    {
        if (battlesystem.state != BattleState.PLAYERTURN)
        {
            Debug.Log("Error");
            return;
        }
        StartCoroutine(AllyAttack1());
    }
    public override void Atk2()
    {
        if (battlesystem.state != BattleState.PLAYERTURN)
        {
            Debug.Log("Error");
            return;
        }

        StartCoroutine(AllyAttack2());
    }
    public override void Atk3()
    {
        if (battlesystem.state != BattleState.PLAYERTURN)
        {
            Debug.Log("Error");
            return;
        }
        StartCoroutine(AllyAttack3());
    }

    public override void Atk4()
    {
        if (battlesystem.state != BattleState.PLAYERTURN)
        {
            Debug.Log("Error");
            return;
        }
        StartCoroutine(AllyAttack4());
    }

    public IEnumerator AllyAttack1()
    {
        battlesystem.state = BattleState.ENEMYTURN;
        HUD.Log.text = "Lunk attacks!";
        yield return new WaitForSeconds(1f);
        playerAnimator.BasicAttack();
        yield return new WaitForSeconds(.5f);
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAA");
        UpdatePlayerMana(manaCostBasic);
        HUD.SetPlayerMana();
        enemyAnimator.Damaged();
        bool isDead = enemyUnit.TakeDamage(damageBasic);
        HUD.Log.text = "Enemy take " + damageBasic + " damage!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            battlesystem.state = BattleState.WON;
            Debug.Log("You win!");
            battlesystem.EndBattle();
        }
        else
        {
            battlesystem.state = BattleState.ENEMYTURN;
            enemyUnit.chooseAttack();
        }
        healIsOn();
    }


    public IEnumerator AllyAttack2()
    {
        //Debug.Log("The mushroom is healing for " + maxHealTurns + " turns");
        // Set enemy turn to prevent spam clicking
        battlesystem.state = BattleState.ENEMYTURN;
        yield return new WaitForSeconds(1f);
        playerAnimator.BasicAttack();
        yield return new WaitForSeconds(0.5f);
        // Debug.Log("Enemy loose " + manaCostDefense + "mana");
        UpdatePlayerMana(manaCostDefense);
        HUD.SetPlayerMana();
        //Mushroom called the offense attack while player was still poisioned, increase the poision damage
        if (healTurns <= 0)
        {
            healTurns = maxHealTurns;
        }
        else
        {
            healTurns++;
        }

        battlesystem.state = BattleState.ENEMYTURN;
        enemyUnit.chooseAttack();

        healIsOn();
    }

    public IEnumerator AllyAttack3()
    {
        //Big damage
        //Enemy basic attack gains 5 mana
        // Set enemy turn to prevent spam clicking
        battlesystem.state = BattleState.ENEMYTURN;
        Debug.Log("Enemy unit steal attack is big!");
        yield return new WaitForSeconds(1f);
        playerAnimator.BasicAttack();
        yield return new WaitForSeconds(.5f);
        UpdatePlayerMana(manaCostOffense);
        HUD.SetPlayerMana();
        enemyAnimator.Damaged();
        bool isDead = enemyUnit.TakeDamage(offHP);
        bool increaseHP = currentPlayerUnit.TakeDamage(offHP * -1);
        //HUD.SetPlayerHealth();

        if (isDead)
        {
            battlesystem.state = BattleState.WON;
            Debug.Log("You win!");
            battlesystem.EndBattle();
        }
        else
        {
            battlesystem.state = BattleState.ENEMYTURN;
            enemyUnit.chooseAttack();
        }

        healIsOn();
    }

    public IEnumerator AllyAttack4()
    {
        // Set enemy turn to prevent spam clicking
        battlesystem.state = BattleState.ENEMYTURN;

        yield return new WaitForSeconds(1f);
        playerAnimator.BasicAttack();
        yield return new WaitForSeconds(0.5f);
        UpdatePlayerMana(manaCostUltimate);
        HUD.SetPlayerMana();
        HUD.healDPS = true;
        HUD.healTank = true;
        //Mushroom called the offense attack while player was still poisioned, increase the poision damage
        Debug.Log("BEFORE heal: " + currentPlayerUnit.currentHP + " health");
        bool isDead = currentPlayerUnit.TakeDamage(healAllAmount);
        Debug.Log("AFTER heal: " + currentPlayerUnit.currentHP + " health");
        HUD.SetPlayerHealth();


        battlesystem.state = BattleState.ENEMYTURN;
        enemyUnit.chooseAttack();


        healIsOn();
    }

    public void healIsOn()
    {
        if (healTurns > 0)
        {
            Debug.Log("BEFORE heal: " + currentPlayerUnit.currentHP + " health");
            bool isDead = currentPlayerUnit.TakeDamage(healAmount);
            UpdatePlayerMana(defMana);
            Debug.Log("AFTER heal: " + currentPlayerUnit.currentHP + " health");
            HUD.updateAllHealth();
            HUD.SetPlayerMana();
            healTurns--;

        }
    }
}
