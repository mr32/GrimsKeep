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
            if(!battleSquare.squareOccupied)
                this.transform.GetChild(i).GetComponent<Image>().color = originalColor;

            if (battleSquare.AnySquareModifiers())
            {
                this.transform.GetChild(i).GetComponent<Image>().color = Color.yellow;
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
