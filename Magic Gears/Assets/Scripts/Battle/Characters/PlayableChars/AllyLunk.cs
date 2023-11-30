using System;
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

    public ParticleSystem regShield;
    public ParticleSystem extraDmg;
    public ParticleSystem ultShield;

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

        playerAnimator.BasicAttack();
        yield return new WaitForSeconds(1f);
        audioSource.PlayOneShot(atk1Audio);
        UpdatePlayerMana(manaCostBasic);
        HUD.SetPlayerMana();
        yield return new WaitForSeconds(0.5f);
        enemyAnimator.Damaged();
        HUD.Log.text = "The attack is successful on " + enemyUnit.unitName + "!\n";
        bool isDead;
        if (increaseDamage)
        {
            HUD.Log.text += "Lunk deals " + ((int)(damageBasic * increaseAmount)) + " damage and gain " + Math.Abs(manaCostBasic) + " mana!";
            isDead = enemyUnit.TakeDamage((int)(damageBasic * increaseAmount));
            extraDmg.Stop();
        }
        else
        {
            HUD.Log.text += "Lunk deals " + damageBasic + " damage and gain " + Math.Abs(manaCostBasic) + " mana!";
            isDead = enemyUnit.TakeDamage(damageBasic);
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
        if(regShield.isPlaying) {
            regShield.Stop();
        }
        increaseDamage = false;
        ultimateOn--;
        if(ultimateOn == 0 && ultShield.isPlaying) {
            ultShield.Stop();
        }
    }


    public IEnumerator AllyAttack2()
    {
        if (currentPlayerMana < manaCostDefense)
        {
            Debug.Log("Not enough mana!");
            yield break;
        }
        battlesystem.state = BattleState.ENEMYTURN;
        playerAnimator.DefensiveAttack();
        audioSource.PlayOneShot(atk2Audio);
        UpdatePlayerMana(manaCostDefense);
        HUD.SetPlayerMana();
        yield return new WaitForSeconds(.5f);
        regShield.Play();
        HUD.Log.text = "Lunk will shield the next attack!\n";
        yield return new WaitForSeconds(2f);
        shieldOn = true;
        battlesystem.state = BattleState.ENEMYTURN;
        enemyUnit.chooseAttack();
        ultimateOn--;
        if(ultimateOn == 0 && ultShield.isPlaying) {
            ultShield.Stop();
        }
    }

    public IEnumerator AllyAttack3()
    {
        if (currentPlayerMana < manaCostOffense)
        {
            Debug.Log("Not enough mana!");
            yield break;
        }
        //Big damage
        //Enemy basic attack gains 5 mana
        battlesystem.state = BattleState.ENEMYTURN;
        UpdatePlayerMana(manaCostOffense);
        HUD.SetPlayerMana();
        playerAnimator.OffensiveAttack();
        audioSource.PlayOneShot(atk3Audio);
        yield return new WaitForSeconds(.5f);

        enemyAnimator.Damaged();
        bool isDead;
        extraDmg.Play();
        if (increaseDamage)
        {
            isDead = enemyUnit.TakeDamage((int)(offHP * increaseAmount));
            HUD.Log.text = "Lunk deals " + ((int)(offHP * increaseAmount)) + " damage and next attack is " + increaseAmount + "x more damage!";
        }
        else
        {
            isDead = enemyUnit.TakeDamage(offHP);
            HUD.Log.text = "Lunk deals " + offHP + " damage and next attack is " + increaseAmount + "x more damage!";
        }

        yield return new WaitForSeconds(2f);


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
        if(regShield.isPlaying) {
            regShield.Stop();
        }
        increaseDamage = true;
        ultimateOn--;
        if(ultimateOn == 0 && ultShield.isPlaying) {
            ultShield.Stop();
        }
    }

    public IEnumerator AllyAttack4()
    {
        if (currentPlayerMana < manaCostUltimate)
        {
            Debug.Log("Not enough mana!");
            yield break;
        }
        battlesystem.state = BattleState.ENEMYTURN;
        playerAnimator.UltimateAttack();
        audioSource.PlayOneShot(ultAudio);
        UpdatePlayerMana(manaCostUltimate);
        HUD.SetPlayerMana();
        yield return new WaitForSeconds(0.5f);
        HUD.Log.text = "Lunk is immune of damage for 2 turns!";
        yield return new WaitForSeconds(2f);
        ultimateOn = 2;
        if(regShield.isPlaying) {
            regShield.Stop();
        }
        ultShield.Play();
        battlesystem.state = BattleState.ENEMYTURN;
        enemyUnit.chooseAttack();
        shieldOn = false;
        if(regShield.isPlaying) {
            regShield.Stop();
        }

    }


}
