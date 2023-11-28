using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Enemy
{

    void Start()
    {
        returnManaTurn = 999;  //Set to a high number since it will be returned back to 1 when the defensive attack is acalled again.
    }

    public int nightmareTurns;
    public int nightmareDamage;
    public int maxNightmareTurns;

    private int manaConsumed;
    private int returnManaTurn;
    public ParticleSystem Nightmares;

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
        HUD.Log.text = "Wizard attacks with magic!";
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        audioSource.PlayOneShot(atk1Audio);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Enemy gains " + manaCostBasic + "mana");
        UpdateEnemyMana(manaCostBasic);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        bool isDead = currentPlayerUnit.TakeDamage(damageBasic);
        HUD.Log.text = currentPlayerUnit.unitName + " takes " + damageBasic + " damage!";
        yield return new WaitForSeconds(2f);
        //HUD.SetPlayerHealth();
        if (isDead)
        {
            HUD.Log.text = "Game over";
            battlesystem.state = BattleState.LOST;
            Debug.Log("You lose!");
            battlesystem.EndBattle();
        }
        else
        {
            battlesystem.state = BattleState.PLAYERTURN;
            battlesystem.PlayerTurn();
        }
        StartCoroutine(checkManaToReturn());
        StartCoroutine(nigthmareIsOn());
    }

    public IEnumerator EnemyAttack2()
    {
        //Debug.Log("The mushroom is healing for " + maxHealTurns + " turns");
        HUD.Log.text = "Wizard took all " + currentPlayerUnit.unitName + "'s mana!";
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyDefensiveAttack();
        audioSource.PlayOneShot(atk2Audio);
        yield return new WaitForSeconds(0.5f);
        // Debug.Log("Enemy loose " + manaCostDefense + "mana");
        UpdateEnemyMana(manaCostDefense);
        HUD.SetEnemyMana();

        Debug.Log("BEFORE DEFENSE: " + Unit.currentPlayerMana + " health");
        //Consume half of the mana of the player. (Made as a bool because that's what the function returns)
        bool consumeHP = TakeDamage(Unit.currentPlayerMana * -1 / 2);
        HUD.Log.text = "Wizard drank half of " + currentPlayerUnit.unitName + "'s mana!";
        yield return new WaitForSeconds(2f);
        Debug.Log("AFTER DEFENSE: " + Unit.currentPlayerMana + " health");



        //Store amount of mana to be returned next turn;
        manaConsumed = Unit.currentPlayerMana / 2;

        //Every turn will decrease the turn.
        returnManaTurn = 1;

        //Take all the mana of the player for that turn. Return half of it in the next turn
        currentPlayerUnit.UpdatePlayerMana(Unit.currentPlayerMana);

        //Update the HUD from taking the mana
        HUD.SetPlayerMana();
        HUD.SetEnemyHealth();

        battlesystem.state = BattleState.PLAYERTURN;
        battlesystem.PlayerTurn();
        StartCoroutine(checkManaToReturn());
        StartCoroutine(nigthmareIsOn());
    }

    public IEnumerator EnemyAttack3()
    {
        HUD.Log.text = "Wizard found " + currentPlayerUnit.unitName + "'s biggest fear and nightmared them for " + maxNightmareTurns + " turns!";
        yield return new WaitForSeconds(2f);
        enemyAnimator.EnemyOffensiveAttack();
        audioSource.PlayOneShot(atk3Audio);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Enemy loose " + manaCostOffense + "mana");
        UpdateEnemyMana(manaCostOffense);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        Nightmares.Play();
        if (nightmareTurns > 0)
        {
            //Wizard called the offense attack while player was still nightmare mode, increase by one turn the nigthmares
            nightmareTurns++;
        }
        else
        {
            nightmareTurns = maxNightmareTurns;
        }

        Debug.Log("The wizard has nightmare you for " + nightmareTurns + " turns");

        //NightmareIsOn is the function that will decrease the turns left and damage the player.
        StartCoroutine(nigthmareIsOn());
        StartCoroutine(checkManaToReturn());

    }

    //Player is still Nightmared. Take damage
    public IEnumerator nigthmareIsOn()
    {
        if (nightmareTurns > 0)
        {
            HUD.Log.text = currentPlayerUnit.unitName + " cannot attack due to nightmares!";
            yield return new WaitForSeconds(2f);
            playerAnimator.Damaged();
            HUD.Log.text = currentPlayerUnit.unitName + " got scared and recieved " + nightmareDamage + " from nightmares!";
            yield return new WaitForSeconds(2f);
            Debug.Log("BEFORE Nightmare: " + currentPlayerUnit.currentHP + " health");
            bool isDead = currentPlayerUnit.TakeDamage(nightmareDamage);
            Debug.Log("AFTER Nightmare: " + currentPlayerUnit.currentHP + " health");
            if (isDead)
            {
                HUD.Log.text = "Game over";
                battlesystem.state = BattleState.LOST;
                Debug.Log("You lose!");
                battlesystem.EndBattle();
            }
            nightmareTurns--;
            if(nightmareTurns == 0) {
                Nightmares.Stop();
            }
            battlesystem.state = BattleState.ENEMYTURN;
            enemyUnit.chooseAttack();
        }
    }

    public IEnumerator checkManaToReturn()
    {
        if(returnManaTurn == 0)
        {
            HUD.Log.text = "Wizard felt pity for " + currentPlayerUnit.unitName + " and returned the other half of the mana!";
            yield return new WaitForSeconds(2f);
            if(nightmareTurns <= 0)
            {
                HUD.Log.text = "Player turn!";
            }
            UpdatePlayerMana(manaConsumed*-1);
            HUD.SetPlayerMana();
            returnManaTurn = 999; //Set to a high number since it will be returned back to 1 when the defensive attack is acalled again.
        }
        returnManaTurn--;
    }
}
