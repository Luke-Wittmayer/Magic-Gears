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

    public ParticleSystem regShield;
    public ParticleSystem extraDmg;
    public ParticleSystem ultShield;



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
        HUD.Log.text = "Lunk attacks slowly but strongly!";
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyBasicAttack();
        yield return new WaitForSeconds(1f);
        audioSource.PlayOneShot(atk1Audio);
        yield return new WaitForSeconds(.5f);
        Debug.Log("Enemy gains " + manaCostBasic + "mana");
        UpdateEnemyMana(manaCostBasic);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        bool isDead;
        if (increaseDamage)
        {
            isDead = currentPlayerUnit.TakeDamage((int)(damageBasic* increaseAmount));
            HUD.Log.text = currentPlayerUnit.unitName+ " takes " + (int)(damageBasic * increaseAmount) + " damage!";
            extraDmg.Stop();
        }
        else
        {
            isDead = currentPlayerUnit.TakeDamage(damageBasic);
            HUD.Log.text = currentPlayerUnit.unitName + " takes " + damageBasic + " damage!";
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
        if(regShield.isPlaying) {
            regShield.Stop();
        }
        increaseDamage = false;
        ultimateOn--;
        if(ultimateOn == 0 && ultShield.isPlaying) {
            ultShield.Stop();
        }
    }


    public IEnumerator EnemyAttack2()
    {
        HUD.Log.text = "Lunk will shield next attack!";
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyDefensiveAttack();
        audioSource.PlayOneShot(atk2Audio);
        yield return new WaitForSeconds(.5f);
        UpdateEnemyMana(manaCostDefense);
        HUD.SetEnemyMana();
        regShield.Play();
        shieldOn = true;
        battlesystem.state = BattleState.PLAYERTURN;
        battlesystem.PlayerTurn();
        ultimateOn--;
        if(ultimateOn == 0 && ultShield.isPlaying) {
            ultShield.Stop();
        }
    }

    public IEnumerator EnemyAttack3()
    {
        //Big damage
        //Enemy basic attack gains 5 mana
        HUD.Log.text = "Lunk's attack is hard as metal!";
        yield return new WaitForSeconds(1f);
        enemyAnimator.EnemyOffensiveAttack();
        audioSource.PlayOneShot(atk3Audio);
        yield return new WaitForSeconds(.5f);
        UpdateEnemyMana(manaCostOffense);
        HUD.SetEnemyMana();
        playerAnimator.Damaged();
        bool isDead;
        extraDmg.Play();
        if (increaseDamage)
        {
            isDead = currentPlayerUnit.TakeDamage((int)(offHP * increaseAmount));
            HUD.Log.text = currentPlayerUnit.unitName + " takes " + (int)(offHP * increaseAmount) + " damage!";
        }
        else
        {
            isDead = currentPlayerUnit.TakeDamage(offHP);
            HUD.Log.text = currentPlayerUnit.unitName + " takes " + offHP + " damage!";
        }
        //HUD.SetPlayerHealth();

        yield return new WaitForSeconds(2f);

        HUD.Log.text = "Lunk is upset and his attack increased for next turn!";
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
        if(regShield.isPlaying) {
            regShield.Stop();
        }
        increaseDamage = true;
        ultimateOn--;
        if(ultimateOn == 0 && ultShield.isPlaying) {
            ultShield.Stop();
        }
    }

    public IEnumerator EnemyAttack4()
    {
        HUD.Log.text = "Lunk decided to be inmune of damage for 2 turns";
        yield return new WaitForSeconds(2f);
        enemyAnimator.EnemyUltimateAttack();
        audioSource.PlayOneShot(ultAudio);
        yield return new WaitForSeconds(0.5f);
        UpdateEnemyMana(manaCostUltimate);
        HUD.SetEnemyMana();
        ultimateOn = 2;
        if(regShield.isPlaying) {
            regShield.Stop();
        }
        ultShield.Play();
        battlesystem.state = BattleState.PLAYERTURN;
        battlesystem.PlayerTurn();
        shieldOn = false;
        if(regShield.isPlaying) {
            regShield.Stop();
        }

    }


}
