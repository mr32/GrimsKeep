using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Debug_ADD_ENEMY : Debugger
{
    // Start is called before the first frame update
    void Start()
    {
        buttonToClick.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        List<int> openBattleSquares = new List<int>();

        BattleSquare[] battleSquares = battleBoard.GetComponentsInChildren<BattleSquare>();

        Card c = deckLoader.CreateCardInstanceByName("TestCard");
        c.cardOwner = Card.PlayTypes.ENEMY;

        for (int i = 0; i < battleSquares.Length; i++)
        {
            int rnd = (int)Random.Range(0, battleSquares.Length);
            BattleSquare tempGO = battleSquares[rnd];
            battleSquares[rnd] = battleSquares[i];
            battleSquares[i] = tempGO;
        }
        
        foreach(BattleSquare bs in battleSquares)
        {
            if (c.CanPlayCardOnTarget(bs.gameObject))
            {
                c.PlayCard(bs.gameObject);
                bs.UpdateAttackAndDefenseGraphics();
                break;
            }
        }
    }
}
