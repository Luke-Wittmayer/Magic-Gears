using System;
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
        HUD.Log.text = "The attack is successful on " + enemyUnit.unitName + "!\n";
        HUD.Log.text += "Ocellus deal " + damageBasic + " damage and gain " + Math.Abs(manaCostBasic) + " mana!";
        playerAnimator.BasicAttack();
        UpdatePlayerMana(manaCostBasic);
        HUD.SetPlayerMana();
        yield return new WaitForSeconds(0.5f);
        enemyAnimator.Damaged();
        bool isDead = enemyUnit.TakeDamage(damageBasic);
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
        StartCoroutine(healIsOn());
    }


    public IEnumerator AllyAttack2()
    {
        //Debug.Log("The mushroom is healing for " + maxHealTurns + " turns");
        // Set enemy turn to prevent spam clicking
        battlesystem.state = BattleState.ENEMYTURN;
        playerAnimator.BasicAttack();
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
        HUD.Log.text = "Ocellus made a spell to increase health and mana over time for " + healTurns + " turns!\n";
        StartCoroutine( healIsOn());
        yield return new WaitForSeconds(2f);

        battlesystem.state = BattleState.ENEMYTURN;
        enemyUnit.chooseAttack();

    }

    public IEnumerator AllyAttack3()
    {
        //Big damage
        //Enemy basic attack gains 5 mana
        // Set enemy turn to prevent spam clicking
        battlesystem.state = BattleState.ENEMYTURN;
        playerAnimator.BasicAttack();
        UpdatePlayerMana(manaCostOffense);
        HUD.SetPlayerMana();
        yield return new WaitForSeconds(.5f);
        enemyAnimator.Damaged();
        bool isDead = enemyUnit.TakeDamage(offHP);
        bool increaseHP = currentPlayerUnit.TakeDamage(offHP * -1);
        //HUD.SetPlayerHealth();
        HUD.Log.text = "Ocellus stole " + offHP + " heal points from the enemy!\n";
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

        StartCoroutine(healIsOn());
    }

    public IEnumerator AllyAttack4()
    {
        // Set enemy turn to prevent spam clicking
        battlesystem.state = BattleState.ENEMYTURN;

        playerAnimator.BasicAttack();

        UpdatePlayerMana(manaCostUltimate);
        HUD.SetPlayerMana();
        HUD.healDPS = true;
        HUD.healTank = true;
        //Mushroom called the offense attack while player was still poisioned, increase the poision damage
        Debug.Log("BEFORE heal: " + currentPlayerUnit.currentHP + " health");
        bool isDead = currentPlayerUnit.TakeDamage(healAllAmount);
        Debug.Log("AFTER heal: " + currentPlayerUnit.currentHP + " health");
        HUD.SetPlayerHealth();

        HUD.Log.text = "Ocellus has healed all party!\n";
        yield return new WaitForSeconds(2f);
        StartCoroutine(healIsOn());
        battlesystem.state = BattleState.ENEMYTURN;
        enemyUnit.chooseAttack();



    }

    public IEnumerator healIsOn()
    {
        if (healTurns > 0)
        {
            yield return new WaitForSeconds(2f);
            bool isDead = currentPlayerUnit.TakeDamage(healAmount);
            UpdatePlayerMana(defMana);
            HUD.updateAllHealth();
            HUD.SetPlayerMana();
            yield return new WaitForSeconds(2f);
            HUD.Log.text = "Ocellus gained " + (healAmount*-1) + " heal!\n";
            HUD.Log.text += "Ocellus gained " + (defMana*-1)+" mana!";
            yield return new WaitForSeconds(2f);

            healTurns--;

        yield return new WaitForSeconds(2f);
        }
    }
}
