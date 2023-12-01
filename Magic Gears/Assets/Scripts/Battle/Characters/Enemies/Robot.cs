using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : Enemy
{
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
        //Enemy basic attack gains 5 mana
        HUD.Log.text = "Robot attacks!";
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
    }

    public IEnumerator EnemyAttack2()
    {
        //Enemy basic attack gains 5 mana
        HUD.Log.text = "Something looks odd on Robot's attack!";
        yield return new WaitForSeconds(1.5f);
        enemyAnimator.EnemyDefensiveAttack();
        audioSource.PlayOneShot(atk2Audio);
        yield return new WaitForSeconds(.5f);
        Debug.Log("Enemy used " + manaCostDefense + "mana");
        UpdateEnemyMana(manaCostDefense);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();

        HUD.Log.text = "Robot takes " + (int)(Unit.currentPlayerMana*1.5) + " health points from " + currentPlayerUnit.unitName + "'s mana!";
        bool gainHealth = TakeDamage((int)(Unit.currentPlayerMana * -1 * 1.5));
        yield return new WaitForSeconds(2f);

        battlesystem.state = BattleState.PLAYERTURN;
        battlesystem.PlayerTurn();        

    }

    public IEnumerator EnemyAttack3()
    {
        //Enemy basic attack gains 5 mana
        HUD.Log.text = "Robot is out of control!";
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyOffensiveAttack();
        audioSource.PlayOneShot(atk3Audio);
        yield return new WaitForSeconds(.5f);
        Debug.Log("Enemy used " + manaCostOffense + "mana");
        UpdateEnemyMana(manaCostOffense);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();

        HUD.Log.text = "Robot makes " + (int)(Unit.currentPlayerMana * 1.5) + " of damage";
        yield return new WaitForSeconds(2f);
        bool isDead = currentPlayerUnit.TakeDamage((int)(Unit.currentPlayerMana * 1.5));
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
    }

}
