﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject battleBoard;
    public bool cardPlayed;
    private void Awake()
    {
        battleBoard = GameObject.FindGameObjectWithTag(Constants.BATTLE_BOARD_TAG);
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
