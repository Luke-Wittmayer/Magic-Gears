using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Enemy
{
    public int healTurns;
    public int maxHealTurns;
    public int healAmount; //NOTE MUST BE NEGATIVE NUMBER

    public int defMana; //PROBABLY THIS ONE TOO

    private bool shieldOn = false;
    public int shieldAmount;

    public bool charging = false;


    public override void chooseAttack()
    {
        base.StateMachine4();
        if (charging)
        {
            if (battlesystem.state != BattleState.ENEMYTURN)
            {
                Debug.Log("Error");
                return;
            }
            StartCoroutine(checkCharge());
       
        }
        else if (currentAtk == CurrentAtk.BASIC)
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
         if (shieldOn)
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
            isDead = currentPlayerUnit.TakeDamage(damageBasic);
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

            shieldOn = false;
            healIsOn();

    }


    public IEnumerator EnemyAttack2()
    {
            Debug.Log("Enemy unit shields the attack!");
            yield return new WaitForSeconds(1f);
            enemyAnimator.EnemyBasicAttack();
            UpdateEnemyMana(manaCostDefense);
            HUD.SetEnemyMana();
            yield return new WaitForSeconds(.5f);

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

            shieldOn = true;
            healIsOn();
    }

    public IEnumerator EnemyAttack3()
    {
            //Enemy basic attack gains 5 mana
            HUD.Log.text = "Cactus attacks!";
            yield return new WaitForSeconds(1f);
            enemyAnimator.EnemyBasicAttack();
            yield return new WaitForSeconds(.5f);
            Debug.Log("Enemy used " + manaCostOffense + "mana");
            UpdateEnemyMana(manaCostOffense);
            HUD.SetEnemyMana();
            playerAnimator.Damaged();
            bool isDead = currentPlayerUnit.TakeDamage(Unit.currentPlayerMana);
            //HUD.SetPlayerHealth();
            UpdateEnemyMana(Unit.currentPlayerMana * -1);
            HUD.SetEnemyMana();

            UpdatePlayerMana(Unit.currentPlayerMana);
            HUD.SetPlayerMana();
            HUD.Log.text = "Enemy makes " + Unit.currentPlayerMana + " of damage";
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
            healIsOn();
            shieldOn = false;
    }

    public IEnumerator EnemyAttack4()
    {
        //Enemy basic attack gains 5 mana
        HUD.Log.text = "Lunk attacks!";
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(.5f);
        charging = true;
        Debug.Log("Enemy is charging");
        battlesystem.state = BattleState.PLAYERTURN;
        battlesystem.PlayerTurn();
        shieldOn = false;
        healIsOn();
    }


    public void healIsOn()
    {
        if (healTurns > 0)
        {
            if (shieldOn)
            {
                shieldOn = false;
                Debug.Log("BEFORE heal: " + enemyUnit.currentHP + " health");
                bool isDead = enemyUnit.TakeDamage(healAmount);
                Debug.Log("AFTER heal: " + enemyUnit.currentHP + " health");
                HUD.updateAllHealth();
                HUD.SetEnemyMana();
                healTurns--;
                shieldOn = true;
            }
            else
            {
                Debug.Log("BEFORE heal: " + enemyUnit.currentHP + " health");
                bool isDead = enemyUnit.TakeDamage(healAmount);
                Debug.Log("AFTER heal: " + enemyUnit.currentHP + " health");
                HUD.updateAllHealth();
                HUD.SetEnemyMana();
                healTurns--;
            }


        }
    }

    public IEnumerator checkCharge()
    {
        Debug.Log("Enemy finished charging");
            UpdateEnemyMana(manaCostUltimate);
            HUD.SetEnemyMana();
            playerAnimator.Damaged();
            bool isDead;
        enemyAnimator.EnemyBasicAttack();
        isDead = currentPlayerUnit.TakeDamage(9999);
            HUD.Log.text = "You take 9999 damage!";

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
            healIsOn();
            charging = false;
        
    }

}
