using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug_RESET_GAME : Debugger
{
    // Start is called before the first frame update
    void Start()
    {
        buttonToClick.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        battleBoard.ResetBoard();

        foreach(Transform child in playerHand)
        {
            Destroy(child.gameObject);
        }

        deckLoader.LoadDeck();
        playerStats.playerMana = 20;
        playerStats.UpdateGraphics();
    }
}
