using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lunk : Enemy
{

    public int defMana; //PROBABLY THIS ONE TOO
    public int offHP;

    private bool shieldOn = false;
    public int shieldAmount;
    private bool increaseDamage = false;
    public float increaseAmount;
    private int  ultimateOn = 0;



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

    public override bool TakeDamage(int dmg)
    {
        Debug.Log("Shield on is:" + shieldOn);
        Debug.Log("Ultimate on is:" + ultimateOn);
        if (ultimateOn > 0)
        {
            return base.TakeDamage(0);
        }
       else  if (shieldOn)
        {
            UpdateEnemyMana(defMana);
            HUD.SetEnemyMana();
            return base.TakeDamage(shieldAmount);
        }
        else
        {
            return base.TakeDamage(dmg);
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
        HUD.Log.text = "Lunk attacks!";
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(.5f);
        Debug.Log("Enemy gains " + manaCostBasic + "mana");
        UpdateEnemyMana(manaCostBasic);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        bool isDead;
        if (increaseDamage)
        {
            isDead = currentPlayerUnit.TakeDamage((int)(damageBasic* increaseAmount));
            HUD.Log.text = "You take " + (int)(damageBasic * increaseAmount) + " damage!";
        }
        else
        {
            isDead = currentPlayerUnit.TakeDamage(damageBasic);
            HUD.Log.text = "You take " + damageBasic + " damage!";
        }

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

        shieldOn = false;
        increaseDamage = false;
        ultimateOn--;
    }


    public IEnumerator EnemyAttack2()
    {
        Debug.Log("Enemy unit shields the attack!");
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(.5f);
        UpdateEnemyMana(manaCostDefense);
        HUD.SetEnemyMana();
        shieldOn = true;
        battlesystem.state = BattleState.PLAYERTURN;
        battlesystem.PlayerTurn();
        ultimateOn--;
    }

    public IEnumerator EnemyAttack3()
    {
        //Big damage
        //Enemy basic attack gains 5 mana
        Debug.Log("Enemy unit attacks big!");
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(.5f);
        UpdateEnemyMana(manaCostOffense);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        bool isDead;
        if (increaseDamage)
        {
            isDead = currentPlayerUnit.TakeDamage((int)(offHP * increaseAmount));
        }
        else
        {
            isDead = currentPlayerUnit.TakeDamage(offHP);
        }
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
        shieldOn = false;
        increaseDamage = true;
        ultimateOn--;
    }

    public IEnumerator EnemyAttack4()
    {
        Debug.Log("Tank is inmune for 2 turns");
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(0.5f);
        UpdateEnemyMana(manaCostUltimate);
        HUD.SetEnemyMana();
        ultimateOn = 2;

        battlesystem.state = BattleState.PLAYERTURN;
        battlesystem.PlayerTurn();
        shieldOn = false;

    }


}
