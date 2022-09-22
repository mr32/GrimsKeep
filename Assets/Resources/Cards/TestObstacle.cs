using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObstacle : ObstacleCard
{
    public override string CardName => "Obstacle Card";

    public override string CardDescription => "This card is an obstacle";

    public override uint CardCost { get => 3; set => cardCost = value; }

    public override string CardFlair => "Blocking";

    protected override int BasePower => 0;

    protected override int BaseDefense => 10;

    protected override int BaseHealth => 100;

    public override bool CanPlayCardOnTarget(GameObject target)
    {
        BattleSquare targetBattleSquare = target.GetComponent<BattleSquare>();

        // If we played it on a BattleSquare and there is no creature on the square
        return targetBattleSquare && !targetBattleSquare.AnythingOnSquare();
    }
}
