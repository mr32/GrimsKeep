using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCard : CreatureCard
{
    private int modifyAmount = 3;

    public CardGraphics cardGraphics;

    public override MoveDirections[] moveDirections => new MoveDirections[] {
        MoveDirections.UP,
        MoveDirections.DOWN,
        MoveDirections.LEFT,
        MoveDirections.RIGHT,
        MoveDirections.BOTTOM_RIGHT,
        MoveDirections.BOTTOM_LEFT,
        MoveDirections.TOP_LEFT,
        MoveDirections.TOP_RIGHT
    };

    public override string CardName => "Test Card A";
    public override string CardDescription => $"If the card is in the front\nit will get a power boost of {modifyAmount}";
    protected override int BaseCreaturePower => 5;
    protected override int BaseCreatureDefense => 0;
    public override string CardFlair => "LARGE CREATURE";
    public override uint CardCost { get => 3; set => CardCost = value; }

    protected override int BaseCreatureHealth => 3;

    public override void OnPlayConditions(GameObject target)
    {
        BattleSquare battleSquare = target.GetComponent<BattleSquare>();

        if(battleSquare != null && battleSquare.col == 0)
        {
            Utils.AddToCardModifiers(
                cardModifierDictionary: cardModifiers,
                cardToAdd: this,
                valueToAdd: modifyAmount
            );

            cardModified = true;
        }
    }

    public override bool CanPlayCardOnTarget(GameObject target)
    {
        BattleSquare targetBattleSquare = target.GetComponent<BattleSquare>();

        // If we played it on a BattleSquare and there is no creature on the square
        return targetBattleSquare && !targetBattleSquare.IsCreatureOnSquare();
    }

    public override string ToString()
    {
        return string.Format("cardName: {0} -- cardModified: {1}", CardName, cardModified);
    }

    public override void PlayCard(GameObject target)
    {
        SoftResetCardValues();
        base.PlayCard(target);
    }
}
