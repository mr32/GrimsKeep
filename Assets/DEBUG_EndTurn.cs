using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_EndTurn : Debugger
{
    private int playerTurnIndex = 0;
    void Start()
    {
        buttonToClick.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        int indexToLookAt = playerTurnIndex++ % gameManager.playersTurn.Count;
        gameManager.currentPlayerTurn = gameManager.playersTurn[indexToLookAt == 0 ? gameManager.playersTurn.Count - 1 : indexToLookAt];
        Debug.Log("Current player turn: " + gameManager.currentPlayerTurn + " " + indexToLookAt.ToString());
    }
}
