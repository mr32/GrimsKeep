using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject battleBoard;
    public bool cardPlayed;

    public List<string> playersTurn = new List<string>() { Constants.PLAYER1, Constants.ENEMY1, "HELLO" };

    public string currentPlayerTurn;

    private void Awake()
    {
        battleBoard = GameObject.FindGameObjectWithTag(Constants.BATTLE_BOARD_TAG);
        currentPlayerTurn = Constants.PLAYER1;
    }

    void Update()
    {
        if (cardPlayed)
        {
            TriggerAllOnBoardConditions();
            cardPlayed = false;
        }
    }

    private void TriggerAllOnBoardConditions()
    {
        foreach(BattleSquare battleSquare in battleBoard.GetComponentsInChildren<BattleSquare>())
        {
            if(battleSquare.cardsPlayedOnObject.Count > 0)
            {
                foreach (Card c in battleSquare.cardsPlayedOnObject)
                {
                    c.OnBoardConditions();
                }
            }
            
        }
    }
}
