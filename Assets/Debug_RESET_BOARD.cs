using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debug_RESET_BOARD : Debugger
{
    private void Start()
    {
        buttonToClick.onClick.AddListener(battleBoard.ResetBoard);
    }

}
