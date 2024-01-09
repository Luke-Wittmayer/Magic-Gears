using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, SWITCHING }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    // public Transform playerBattleStation;
    // public Transform enemyBattleStation;

    public DPSPlayer playerUnit;
    public Unit enemyUnit;
    public PlayerAnimationController playerAnimator;
    private EnemyAnimationController enemyAnimator;

    public BattleState state;

    public BattleHUD HUD;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        playerPrefab.SetActive(true);
        SetUpBattle();
    }

    void SetUpBattle()
    {
        // GameObject playerGO = Instantiate(playerPrefab);
        // GameObject enemyGO = Instantiate(enemyPrefab);

        // playerGO.SetActive(true);
        // enemyGO.SetActive(true);

        playerUnit = playerPrefab.GetComponent<DPSPlayer>();
        enemyUnit = enemyPrefab.GetComponent<Unit>();
        playerAnimator = playerPrefab.GetComponent<PlayerAnimationController>();
        enemyAnimator = enemyPrefab.GetComponent<EnemyAnimationController>();

        Time.timeScale = 1f;
        Unit.maxPlayerMana = 100;
        Unit.currentPlayerMana = 0;
        HUD.SetupHUD();

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    public void EndBattle()
    {
        if (state == BattleState.WON)
        {
            enemyAnimator.Dead();
            playerAnimator.Win();
        }
        else if (state == BattleState.LOST)
        {
            playerAnimator.Lose();
        }
    }

    public void PlayerTurn()
    {
        HUD.Log.text = "Player turn!";
        state = BattleState.PLAYERTURN;
    }


}
