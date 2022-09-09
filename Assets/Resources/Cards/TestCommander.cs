using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCommander : CommanderCard
{
    public override int BaseCreaturePower => 5;

    public override int BaseCreatureDefense => 5;

    public override int BaseCreatureHealth => 10;

    public override string CardName => throw new System.NotImplementedException();

    public override string CardDescription => throw new System.NotImplementedException();

    public override uint CardCost { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override string CardFlair => throw new System.NotImplementedException();

    public override bool CanPlayCardOnTarget(GameObject target)
    {
        throw new System.NotImplementedException();
    }
}
