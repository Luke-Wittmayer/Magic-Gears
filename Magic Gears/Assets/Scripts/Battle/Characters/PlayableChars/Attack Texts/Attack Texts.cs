using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class AttackTexts : MonoBehaviour
{
    public DPSPlayer DPS;
    public AllyLunk Lunk;
    public AllyOcellus Ocellus;
    public TMP_Text text;

    public void DPSAtkText1() {
        text.text = "Basic Attack!\nDeal " + Math.Abs(DPS.damageBasic) + " damage\nGain " 
        + Math.Abs(DPS.manaCostBasic) + " mana" ;

    }
    public void DPSAtkText2() {
        text.text = "Use Mana!\nDeal " + Math.Abs(2*DPS.damageBasic) + " damage\nUse "
        + Math.Abs(DPS.manaCostOffense) + " mana";
    }
    public void DPSAtkText3() {
        text.text = "Steal Mana!\nDeal " + Math.Abs(DPS.damageBasic/2) + " damage\nGain "
        + Math.Abs(DPS.manaCostDefense) + " mana";
    }
    public void DPSAtkText4() {
        text.text = "Ultimate Attack!\nDeal " + Math.Abs(2*DPS.damageBasic) + " damage and gain 2 extra turns\nUse "
        + Math.Abs(DPS.manaCostUltimate) + " mana";
    }

    public void OcellusAtkText1() {
        text.text = "Basic Attack!\nDeal " + Math.Abs(Ocellus.damageBasic) + " damage\nGain " 
        + Math.Abs(Ocellus.manaCostBasic) + " mana" ;
    }
    public void OcellusAtkText2() {
        text.text = "Steal health!\nSteal up to " + Math.Abs(Ocellus.offHP) + " health\nUse "
        + Math.Abs(Ocellus.manaCostOffense) + " mana";

    }
    public void OcellusAtkText3() {
        text.text = "Gain health and mana over time!\nHeal " + Math.Abs(Ocellus.healAmount) + " health and gain " 
        + Math.Abs(Ocellus.defMana) + " mana for the next " + Ocellus.maxHealTurns + " turns Ocellus is active."
        + " Uses " + Math.Abs(Ocellus.manaCostDefense) + " mana to activate";
    }
    public void OcellusAtkText4() {
        text.text = "Ultimate heal!\nHeal all player party members to full health\nUse "
        + Math.Abs(Ocellus.manaCostUltimate) + " mana";
    }

    public void LunkAtkText1() {
        text.text = "Basic Attack!\nDeal " + Math.Abs(Lunk.damageBasic) + " damage\nGain " 
        + Math.Abs(Lunk.manaCostBasic) + " mana" ;
    }
    public void LunkAtkText2() {
        text.text = "Increase Damage!\nDeal " + Math.Abs(Lunk.offHP) + " damage and increases Lunk's next attack by " 
        + Lunk.increaseAmount + "x. Use " + Lunk.manaCostOffense + " mana";

    }
    public void LunkAtkText3() {
        text.text = "Shield next attack!\nReduce next attack on Lunk to " + Lunk.shieldAmount + " damage and gain "
        + Math.Abs(Lunk.defMana) + " mana if hit on his next turn. Use " + Lunk.manaCostDefense + " mana";
    }
    public void LunkAtkText4() {
        text.text = "Ultimate Shield!\nNegate all damage to Lunk for his next 2 turns\nUse "
        + Lunk.manaCostUltimate + " mana"; 
    }
}
