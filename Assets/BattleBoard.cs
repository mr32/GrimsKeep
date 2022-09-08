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
            if(!this.transform.GetChild(i).GetComponent<BattleSquare>().squareOccupied)
                this.transform.GetChild(i).GetComponent<Image>().color = originalColor;
        }
        battleSquareIndiciesLit.Clear();

        base.ResetSelf();
    }
}
