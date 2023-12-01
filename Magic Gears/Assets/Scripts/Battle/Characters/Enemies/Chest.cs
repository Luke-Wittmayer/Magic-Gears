using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Enemy
{

    public Unit tank;
    public Unit healer;
    public Unit DPS;
    public int halfDamageTurns;
    public int halfDamageMaxTurns;
    public int swallowDmg;
    public int swallowTurnsDPS;
    public int swallowTurnsHealer;
    public int swallowTurnsTank;
    public int swallowMaxTurns;

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

    public override bool TakeDamage(int dmg)
    {
        //Player makes half damage damage
        if (halfDamageTurns > 0)
        {
            halfDamageTurns--;
            return base.TakeDamage(dmg/2);
        }
        return base.TakeDamage(dmg);
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
        HUD.Log.text = "Chest bites upset!";
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        audioSource.PlayOneShot(atk1Audio);
        yield return new WaitForSeconds(.5f);
        Debug.Log("Enemy gains " + manaCostBasic + "mana");
        UpdateEnemyMana(manaCostBasic);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        bool isDead = currentPlayerUnit.TakeDamage(damageBasic);
        HUD.Log.text = currentPlayerUnit.unitName+ " takes " + damageBasic + " damage!";
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
            StartCoroutine(checkSwallow());
        }


    }

    public IEnumerator EnemyAttack2()
    {
        HUD.Log.text = "Chest will recive only half of damage for "+halfDamageMaxTurns + " turns!";
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyDefensiveAttack();
        audioSource.PlayOneShot(atk2Audio);
        yield return new WaitForSeconds(.5f);
        UpdateEnemyMana(manaCostDefense);
        HUD.SetEnemyMana();

        halfDamageTurns = halfDamageMaxTurns;

        battlesystem.state = BattleState.PLAYERTURN;
        battlesystem.PlayerTurn();

        StartCoroutine(checkSwallow());
    }

    public IEnumerator EnemyAttack3()
    {
        //Big damage
        //Enemy basic attack gains 5 mana
        HUD.Log.text = "Chest got hungry and swallowed " + currentPlayerUnit.unitName + " for " + swallowMaxTurns +" turns!";
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyOffensiveAttack();
        audioSource.PlayOneShot(atk3Audio);
        yield return new WaitForSeconds(.5f);
        HUD.Log.text = currentPlayerUnit.unitName + " recieved " + swallowDmg + " damage";
        playerAnimator.Damaged();
        yield return new WaitForSeconds(2f);
        UpdateEnemyMana(manaCostOffense);
        HUD.SetEnemyMana();

        bool isDead = currentPlayerUnit.TakeDamage(swallowDmg);
        HUD.updateAllValues();
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
            //Note: It should be impossible to swallow the three allies at the same time
            //Check if the current player is the DPS
         //   Debug.Log("REACHED FIRST IF STATEMENT");
            if (currentPlayerUnit == DPS)
            {
              //  Debug.Log("NOTICED THAT PLAYER IS DPS");
                //Check if the healer was also swallowed. If not, switch to it
                if (swallowTurnsHealer <= 0)
                {
                 //   Debug.Log("WANTS TO SWTICH");
                    HUD.switchToHealer(healer);
                }
                else
                {
                    HUD.switchToTank(tank);
                }
                swallowTurnsDPS = swallowMaxTurns;
                DPS.playerIsSwallowed = true;
            }
            //Check if the player is the tank
            else if(currentPlayerUnit == tank)
            {
                //Check if the DPS was also swallowed. If not, switch to it
                if (swallowTurnsDPS <= 0)
                {
                    HUD.switchToDPS(DPS);
                }
                else
                {
                    HUD.switchToHealer(healer);
                }
                swallowTurnsTank = swallowMaxTurns;
                tank.playerIsSwallowed = true;
            }
            else if (currentPlayerUnit == healer)
            {
                //Check if the DPS was also swallowed. If not, switch to it
                if (swallowTurnsDPS <= 0)
                {
                    HUD.switchToDPS(DPS);
                }
                else
                {
                    HUD.switchToTank(tank);
                }
                swallowTurnsHealer = swallowMaxTurns;
                healer.playerIsSwallowed = true;
            }


            battlesystem.PlayerTurn();
            StartCoroutine(checkSwallow());
        }
    }

    public IEnumerator checkSwallow()
    {
        //DPS Swallow turn is over, put her back on combat. It should never swtich 2 allies at the same time.
        battlesystem.state = BattleState.PLAYERTURN;
        if (DPS.playerIsSwallowed)
        {
            if (swallowTurnsDPS <= 0)
            {
                DPS.playerIsSwallowed = false;
                HUD.switchToDPS(DPS);
                HUD.Log.text = "Chest did not like Harper's taste and spit her back!";
                yield return new WaitForSeconds(2f);
                HUD.Log.text = "Player turn!";
            }
            swallowTurnsDPS--;
        }
        if (healer.playerIsSwallowed)
        {
            if (swallowTurnsHealer <= 0)
            {
                healer.playerIsSwallowed = false;
                HUD.switchToHealer(healer);
                HUD.Log.text = "Chest did not like Ocellus's taste and spit him back!";
                yield return new WaitForSeconds(2f);
                HUD.Log.text = "Player turn!";
            }
            swallowTurnsHealer--;
        }

        if (tank.playerIsSwallowed)
        {
            if (swallowTurnsTank <= 0)
            {
                tank.playerIsSwallowed = false;
                HUD.switchToTank(tank);
                HUD.Log.text = "Chest did not like Lunk's taste and spit him back!";
                yield return new WaitForSeconds(2f);
                HUD.Log.text = "Player turn!";
            }
            swallowTurnsTank--;

        }
}

}
