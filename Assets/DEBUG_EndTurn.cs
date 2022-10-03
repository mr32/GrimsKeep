using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_EndTurn : Debugger
{
    private int playerTurnIndex = 0;
    void Start()
    {
        buttonToClick.onClick.AddListener(TaskOnClick);
        foreach(string s in gameController.playersTurn)
        {
            Debug.Log(s);
        }
    }

    void TaskOnClick()
    {
        int indexToLookAt = ++playerTurnIndex % gameController.playersTurn.Count;
        if(indexToLookAt == 0)
        {
            playerTurnIndex = 0;
        }
        gameController.currentPlayerTurn = gameController.playersTurn[indexToLookAt];
        Debug.Log("Current player turn: " + gameController.currentPlayerTurn + " " + indexToLookAt.ToString());
    }
}
