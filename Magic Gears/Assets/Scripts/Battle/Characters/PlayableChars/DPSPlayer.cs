using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DPSPlayer : Unit
{
    /*
    DPS Character moves:
    Basic
    Defense: Steal mana
    Offense: Use mana
    Ultimate: extra turn
    */

    public override void Atk1() {
        if (battlesystem.state != BattleState.PLAYERTURN){
            Debug.Log("Error");
            return;
        }
        StartCoroutine(DPSBasicAttack());
    }
    public override void Atk2() {
        if (battlesystem.state != BattleState.PLAYERTURN){
            Debug.Log("Error");
            return;
        }
        Debug.Log("Running");
        StartCoroutine(DPSSpendManaAttack());
    }
    public override void Atk3() {
        if (battlesystem.state != BattleState.PLAYERTURN){
            return;
        }
        StartCoroutine(DPSStealManaAttack());        
    }
    public override void Atk4() {
        if (battlesystem.state != BattleState.PLAYERTURN){
            return;
        }
        StartCoroutine(DPSUltimateAttack());
    }

    IEnumerator DPSBasicAttack() {
        // Set enemy turn to prevent spam clicking
        battlesystem.state = BattleState.ENEMYTURN;
        // Damage the enemy
        playerAnimator.BasicAttack();
        UpdatePlayerMana(manaCostBasic);
        HUD.SetPlayerMana();
        yield return new WaitForSeconds(.5f);
        audioSource.PlayOneShot(atk1Audio);
        bool isDead = enemyUnit.TakeDamage(this.damageBasic);
        //HUD.SetEnemyHealth();
        enemyAnimator.Damaged();

        HUD.Log.text = "The attack is successful on " + enemyUnit.unitName + "!\n";
        HUD.Log.text += "Harper deals " + damageBasic + " damage and gains " + Math.Abs(manaCostBasic) + " mana!";
        Debug.Log(enemyUnit.unitName + " now has " + enemyUnit.currentHP + " remaining.");
        yield return new WaitForSeconds(2f);
        if (isDead){
            battlesystem.state = BattleState.WON;
            Debug.Log("You win!");
            battlesystem.EndBattle();
        } else 
        {
            if(playerAnimator.UltimateState == 0){
                enemyUnit.chooseAttack();
            }else {
                battlesystem.PlayerTurn();
            }
        }
    }

    IEnumerator DPSStealManaAttack(){
        Debug.Log("In the function");
        // Set enemy turn to prevent spam clicking
        battlesystem.state = BattleState.ENEMYTURN;
        // Damage the enemy
        playerAnimator.ManaStealAttack();
        audioSource.PlayOneShot(atk3Audio);
        UpdatePlayerMana(manaCostDefense);
        enemyUnit.UpdateEnemyMana(-manaCostDefense);
        bool isDead = enemyUnit.TakeDamage(this.damageBasic/2);
        HUD.updateAllMana();
        //HUD.SetEnemyHealth();
        enemyAnimator.Damaged();
        //yield return new WaitForSeconds(0f);

        HUD.Log.text = "The attack is successful on " + enemyUnit.unitName + "!\n";
        HUD.Log.text += "Harper deals " + damageBasic/2 + " damage and gain " + Math.Abs(manaCostDefense) + " mana!";
        Debug.Log(enemyUnit.unitName + " now has " + enemyUnit.currentHP + " remaining.");
        yield return new WaitForSeconds(2f);

        if(isDead){
            battlesystem.state = BattleState.WON;
            Debug.Log("You win!");
            battlesystem.EndBattle();
        } else 
        {
            if(playerAnimator.UltimateState == 0){
                battlesystem.state = BattleState.ENEMYTURN;
                enemyUnit.chooseAttack();
            }else {
                battlesystem.PlayerTurn();
            }
        }
    }

    IEnumerator DPSSpendManaAttack(){
        if(currentPlayerMana < manaCostOffense){
            Debug.Log("Not enough mana!");
            yield break;
        }
        // Set enemy turn to prevent spam clicking
        battlesystem.state = BattleState.ENEMYTURN;
        // Damage the enemy
        bool isDead = enemyUnit.TakeDamage(this.damageBasic*2);
        playerAnimator.SpendManaAttack();
        yield return new WaitForSeconds(.5f);
        audioSource.PlayOneShot(atk2Audio);
        UpdatePlayerMana(manaCostOffense);
        HUD.SetPlayerMana();
        //HUD.SetEnemyHealth();
        enemyAnimator.Damaged();

        HUD.Log.text = "The attack is successful on " + enemyUnit.unitName + "!\n";
        HUD.Log.text += "Harper deals " + damageBasic*2 + " damage and spend " + manaCostOffense + " mana!";
        yield return new WaitForSeconds(2f);

        Debug.Log("The attack is successful on " + enemyUnit.unitName + "!");
        Debug.Log(enemyUnit.unitName + " now has " + enemyUnit.currentHP + " remaining.");

        if(isDead){
            battlesystem.state = BattleState.WON;
            Debug.Log("You win!");
            battlesystem.EndBattle();
        } else 
        {
            if(playerAnimator.UltimateState == 0){
                battlesystem.state = BattleState.ENEMYTURN;
                enemyUnit.chooseAttack();
            }else {
                battlesystem.PlayerTurn();
            }
        }
    }

    IEnumerator DPSUltimateAttack(){
        if(currentPlayerMana < manaCostUltimate){
            Debug.Log("Not enough mana!");
            yield break;
        }
        // Set enemy turn to prevent spam clicking
        battlesystem.state = BattleState.ENEMYTURN;
        // Damage the enemy
        bool isDead = enemyUnit.TakeDamage(this.damageBasic*2);
        playerAnimator.UltimateAttack();
        yield return new WaitForSeconds(.5f);
        audioSource.PlayOneShot(ultAudio);
        UpdatePlayerMana(manaCostUltimate);
        HUD.SetPlayerMana();
        //HUD.SetEnemyHealth();
        enemyAnimator.Damaged();

        HUD.Log.text = "The attack is successful on " + enemyUnit.unitName + "!\n";
        HUD.Log.text += "Harper deals " + Math.Abs(damageBasic*2) + " damage and spend " + manaCostUltimate + " mana!\n";
        

        Debug.Log("The attack is successful on " + enemyUnit.unitName + "!");
        Debug.Log(enemyUnit.unitName + " now has " + enemyUnit.currentHP + " remaining.");

        if(isDead){
            battlesystem.state = BattleState.WON;
            Debug.Log("You win!");
            battlesystem.EndBattle();
        } else 
        {
            HUD.Log.text += "Harper gets an extra turn!";
            yield return new WaitForSeconds(2f);
            battlesystem.PlayerTurn();
        }
    }
}
