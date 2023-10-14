using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Slider manaSlider;

    public void SetHUD(Unit unit){
        manaSlider.value = unit.currentMana;
    }

    public void SetMana(int mana){
        manaSlider.value += mana;
    }
}
