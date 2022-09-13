using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleBoard : UserGraphicController
{
    public List<int> battleSquareIndiciesLit = new List<int>();

    private Color originalColor = Color.white;

    public override bool ResetCondition()
    {
        return true;
    }

    public override void ResetSelf()
    {
        foreach (int i in battleSquareIndiciesLit)
        {
            BattleSquare battleSquare = this.transform.GetChild(i).GetComponent<BattleSquare>();
            
            if(battleSquare.AnySquareModifiers() && !battleSquare.AnyEnemyCardsOnSquare())
            {
                this.transform.GetChild(i).GetComponent<Image>().color = Color.yellow;
            }
            else if(battleSquare.AnyEnemyCardsOnSquare())
            {
                this.transform.GetChild(i).GetComponent<Image>().color = battleSquare.currentColor;
            }
            else
            {
                this.transform.GetChild(i).GetComponent<Image>().color = originalColor;
            }
        }
        battleSquareIndiciesLit.Clear();

        base.ResetSelf();
    }

    public void ResetBoard()
    {
        foreach(BattleSquare battleSquare in this.gameObject.GetComponentsInChildren<BattleSquare>())
        {
            battleSquare.ResetBattleSquareToDefaultState(true);
        }
    }
}
