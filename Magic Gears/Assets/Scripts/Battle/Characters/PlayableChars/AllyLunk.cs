using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyLunk : Unit
{

    public int defMana; //PROBABLY THIS ONE TOO
    public int offHP;

    private bool shieldOn = false;
    public int shieldAmount;
    private bool increaseDamage = false;
    public float increaseAmount;
    public int ultimateOn = 0;

    public override bool TakeDamage(int dmg)
    {
        Debug.Log("Shield on is:" + shieldOn);
        Debug.Log("Ultimate on is:" + ultimateOn);
        if (ultimateOn > 0)
        {
            return base.TakeDamage(0);
        }
        else if (shieldOn)
        {
            UpdatePlayerMana(defMana);
            HUD.SetPlayerMana();
            return base.TakeDamage(shieldAmount);
        }
        else
        {
            return base.TakeDamage(dmg);
        }


    }

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
        //Enemy basic attack gains 5 mana
        battlesystem.state = BattleState.ENEMYTURN;
        HUD.Log.text = "Lunk attacks!";
        yield return new WaitForSeconds(1f);
        playerAnimator.BasicAttack();
        yield return new WaitForSeconds(.5f);
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAA");
        UpdatePlayerMana(manaCostBasic);
        HUD.SetPlayerMana();
        enemyAnimator.Damaged();
        bool isDead;
        if (increaseDamage)
        {
            isDead = enemyUnit.TakeDamage((int)(damageBasic * increaseAmount));
            HUD.Log.text = "Enemy take " + (int)(damageBasic * increaseAmount) + " damage!";
        }
        else
        {
            isDead = enemyUnit.TakeDamage(damageBasic);
            HUD.Log.text = "Enemy take " + damageBasic + " damage!";
        }

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

        shieldOn = false;
        increaseDamage = false;
        ultimateOn--;
    }


    public IEnumerator AllyAttack2()
    {
        Debug.Log("BBBBBBBBBBBBBBBBBB");
        Debug.Log("Enemy unit shields the attack!");
        battlesystem.state = BattleState.ENEMYTURN;
        yield return new WaitForSeconds(1f);
        playerAnimator.BasicAttack();
        yield return new WaitForSeconds(.5f);
        UpdatePlayerMana(manaCostDefense);
        HUD.SetPlayerMana();
        shieldOn = true;
        battlesystem.state = BattleState.ENEMYTURN;
        enemyUnit.chooseAttack();
        ultimateOn--;
    }

    public IEnumerator AllyAttack3()
    {
        //Big damage
        //Enemy basic attack gains 5 mana
        Debug.Log("CCCCCCCCCCCCCCCCCCC");
        battlesystem.state = BattleState.ENEMYTURN;
        Debug.Log("Enemy unit attacks big!");
        yield return new WaitForSeconds(1f);
        playerAnimator.BasicAttack();
        yield return new WaitForSeconds(.5f);
        UpdatePlayerMana(manaCostOffense);
        HUD.SetPlayerMana();
        enemyAnimator.Damaged();
        bool isDead;
        if (increaseDamage)
        {
            isDead = enemyUnit.TakeDamage((int)(offHP * increaseAmount));
        }
        else
        {
            isDead = enemyUnit.TakeDamage(offHP);
        }
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
        shieldOn = false;
        increaseDamage = true;
        ultimateOn--;
    }

    public IEnumerator AllyAttack4()
    {
        Debug.Log("DDDDDDDDDDDDDDDDDDDDDDD");
        battlesystem.state = BattleState.ENEMYTURN;
        Debug.Log("Tank is inmune for 2 turns");
        yield return new WaitForSeconds(1f);
        playerAnimator.BasicAttack();
        yield return new WaitForSeconds(0.5f);
        UpdatePlayerMana(manaCostUltimate);
        HUD.SetPlayerMana();
        ultimateOn = 2;

        battlesystem.state = BattleState.ENEMYTURN;
        enemyUnit.chooseAttack();
        shieldOn = false;

    }


}
