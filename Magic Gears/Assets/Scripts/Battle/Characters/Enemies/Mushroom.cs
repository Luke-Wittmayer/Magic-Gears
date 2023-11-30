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

    public ParticleSystem poisonCloud;
    public ParticleSystem healCloud;

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
    public IEnumerator posionIsOn()
    {
        if (posionTurns > 0)
        {
            bool isDead = currentPlayerUnit.TakeDamage(poisonAttack);
            HUD.Log.text = currentPlayerUnit.unitName + " takes " + poisonAttack + " damage from poison!";
            playerAnimator.Damaged();
            yield return new WaitForSeconds(2f);
      //      Debug.Log("BEFORE Poisioned: " + currentPlayerUnit.currentHP + " health");
      //      Debug.Log("AFTER Poisioned: " + currentPlayerUnit.currentHP + " health");
            HUD.Log.text = "Player turn!";
            if (isDead)
            {
                HUD.Log.text = "Game over!";
                battlesystem.state = BattleState.LOST;
                Debug.Log("You lose!");
                battlesystem.EndBattle();
            }
            posionTurns--;
            if(posionTurns == 0) {
                poisonCloud.Stop();
            }
        }
        yield return new WaitForSeconds(0f);
    }

    //Mushroom still have the defense move active. Increase health
    public IEnumerator healIsOn()
    {
        if (healTurns > 0)
        {
            HUD.Log.text = "Happy mushroom gains " + (-1*healAmount) + " health points!";
            yield return new WaitForSeconds(1f);
            Debug.Log("BEFORE heal: " + enemyUnit.currentHP + " health");
            HUD.Log.text = "Player turn!";
            bool isDead = enemyUnit.TakeDamage(healAmount);
            Debug.Log("AFTER heal: " + enemyUnit.currentHP + " health");
            HUD.updateAllHealth();
            healTurns--;
            if(healTurns == 0) {
                healCloud.Stop();
            }

        }
        yield return new WaitForSeconds(0f);
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
        HUD.Log.text = "Happy mushroom attacks joyfully!";
        yield return new WaitForSeconds(1f);
        audioSource.PlayOneShot(atk1Audio);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Enemy gains " + manaCostBasic + "mana");
        UpdateEnemyMana(manaCostBasic);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        bool isDead = currentPlayerUnit.TakeDamage(damageBasic);
        //HUD.SetPlayerHealth();
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

        StartCoroutine( healIsOn());
        StartCoroutine(posionIsOn());
    }

    public IEnumerator EnemyAttack2()
    {
        //Debug.Log("The mushroom is healing for " + maxHealTurns + " turns");
        HUD.Log.text = "Happy mushroom uses his root to gain health and keep his mood!";
        yield return new WaitForSeconds(2f);
        audioSource.PlayOneShot(atk2Audio);
        enemyAnimator.EnemyDefensiveAttack();
        yield return new WaitForSeconds(0.5f);
      // Debug.Log("Enemy loose " + manaCostDefense + "mana");
        UpdateEnemyMana(manaCostDefense);
        HUD.SetEnemyMana();
        //Mushroom called the offense attack while player was still poisioned, increase the poision damage
        if (healTurns > 0)
        {
            healAmount = healAmount + healPlus;
        }
        healCloud.Play();
        healTurns = maxHealTurns;
        battlesystem.state = BattleState.PLAYERTURN;
        battlesystem.PlayerTurn();
        
        StartCoroutine(healIsOn());
        StartCoroutine(posionIsOn());
    }

    public IEnumerator EnemyAttack3()
    {
        HUD.Log.text = "Happy mushroom posioned " + currentPlayerUnit.unitName + " for " + maxPoisonTurns + " turns to relax!";
        Debug.Log("The mushroom has posioned you for " + maxPoisonTurns + " turns");
        yield return new WaitForSeconds(2f);
        audioSource.PlayOneShot(atk3Audio);
        enemyAnimator.EnemyOffensiveAttack();
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
        poisonCloud.Play();
        posionTurns = maxPoisonTurns;
        StartCoroutine(healIsOn());
        StartCoroutine(posionIsOn());
        battlesystem.state = BattleState.PLAYERTURN;
       battlesystem.PlayerTurn();

    }
}
