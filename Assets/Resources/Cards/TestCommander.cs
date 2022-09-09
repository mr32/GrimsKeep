using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCommander : CommanderCard
{
    public override int BaseCreaturePower => 100;

    public override int BaseCreatureDefense => 5;

    public override int BaseCreatureHealth => 10;

    public override string CardName => "Test Commander";

    public override string CardDescription => "Commander of the deck";

    public override uint CardCost { get => 6; set => cardCost = value; }

    public override string CardFlair => "My Life";

    public override bool CanPlayCardOnTarget(GameObject target)
    {
        BattleSquare targetBattleSquare = target.GetComponent<BattleSquare>();

        return targetBattleSquare && !targetBattleSquare.IsCreatureOnSquare();
    }

    public override void PlayCard(GameObject target)
    {
        ResetCardValues();
        base.PlayCard(target);
    }
}
