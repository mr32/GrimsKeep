using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCard : CreatureCard
{
    private int modifyAmount = 3;
    public CardGraphics cardGraphics;

    public override MoveDirections[] moveDirections => new MoveDirections[] { 
        MoveDirections.TOP_RIGHT,
        MoveDirections.TOP_LEFT
    };

    public override string CardName => "Test Card A";
    public override string CardDescription => $"If the card is in the front\nit will get a power boost of {modifyAmount}";
    public override int BaseCreaturePower => 5;
    public override int BaseCreatureDefense => 3;
    public override string CardFlair => "LARGE CREATURE";
    public override uint CardCost { get => 3; set => cardCost = value; }

    public override int BaseCreatureHealth => 2;

    public override void CardRules(GameObject target)
    {
        BattleSquare battleSquare = target.GetComponent<BattleSquare>();

        if(battleSquare != null && battleSquare.col == 0)
        {
            powerModifier = modifyAmount;
            cardModified = true;
        }
    }

    public override bool CanPlayCardOnTarget(GameObject target)
    {
        BattleSquare targetBattleSquare = target.GetComponent<BattleSquare>();

        // If we played it on a BattleSquare and there is no creature on the square
        return targetBattleSquare && !targetBattleSquare.IsCreatureOnSquare();
    }

    public override void PlayCard(GameObject target)
    {
        ResetCardValues();
        base.PlayCard(target);
    }
}
