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

    public ParticleSystem Ultimate;
    public ParticleSystem Shield;


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
            HUD.Log.text = "Dragon attacks!";
            yield return new WaitForSeconds(1f);
            enemyAnimator.EnemyBasicAttack();
            audioSource.PlayOneShot(atk1Audio);
            yield return new WaitForSeconds(.5f);
            Debug.Log("Enemy gains " + manaCostBasic + "mana");
            UpdateEnemyMana(manaCostBasic);
            HUD.SetEnemyMana();
            playerAnimator.Damaged();
            bool isDead;
            isDead = currentPlayerUnit.TakeDamage(damageBasic);
        HUD.Log.text = currentPlayerUnit.unitName + " takes " + damageBasic + " damage!";
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
        HUD.Log.text = "Dragon shields the attack and something else...!";
            enemyAnimator.EnemyDefensiveAttack();
            audioSource.PlayOneShot(atk2Audio);
            yield return new WaitForSeconds(2f);
            UpdateEnemyMana(manaCostDefense);
            HUD.SetEnemyMana();
            Shield.Play();
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
            HUD.Log.text = "Dragon wants some mana!";
            yield return new WaitForSeconds(1f);
            enemyAnimator.EnemyOffensiveAttack();
            audioSource.PlayOneShot(atk3Audio);
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
            HUD.Log.text = "Dragon stole " + currentPlayerUnit.unitName + "'s mana and converted it to damage";
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
        HUD.Log.text = "Dragon is charging for something never seen before!";
        enemyAnimator.EnemyDefensiveAttack();
        audioSource.PlayOneShot(ultAudio);
        Ultimate.Play();
        yield return new WaitForSeconds(1.5f);
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

            if(healTurns == 0) {
                Shield.Stop();
            }

        }
    }

    public IEnumerator checkCharge()
    {
        HUD.Log.text = "Dragon is done charging!";
        yield return new WaitForSeconds(1f);
        UpdateEnemyMana(manaCostUltimate);
            HUD.SetEnemyMana();
            bool isDead;
        enemyAnimator.EnemyUltimateAttack();
        audioSource.PlayOneShot(ultAudio);
        yield return new WaitForSeconds(0.5f);
        playerAnimator.Damaged();
        Ultimate.Stop();
        isDead = currentPlayerUnit.TakeDamage(9999);
            HUD.Log.text = currentPlayerUnit.unitName + " takes 9999 damage!";

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
            HUD.Log.text = "But Lunk resisted the attack!";
            yield return new WaitForSeconds(1f);
            battlesystem.state = BattleState.PLAYERTURN;
                battlesystem.PlayerTurn();
            }
            shieldOn = false;
            healIsOn();
            charging = false;
        
    }

}
