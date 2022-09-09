using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpell : SpellCard
{
    public override string CardName => "Test Spell";
    public override string CardDescription => $"Add {creatureModifierAmount} to target Creature";
    public override uint CardCost { get => 1; set => cardCost = value; }
    public override string CardFlair => "Cool Test Spell!";
    public override CardTypes CardType => CardTypes.SQUARE_MODIFIER;
    public int creatureModifierAmount = 5;

    public override void CardRules(GameObject gameObjectPlayedOn)
    {
        CreatureCard creatureCard = gameObjectPlayedOn.GetComponent<CreatureCard>();
        
        if(creatureCard)
            creatureCard.additionalPowerModifier += creatureModifierAmount;
    }

    public override bool CanPlayCardOnTarget(GameObject target)
    {
        BattleSquare targetBattleSquare = target.GetComponent<BattleSquare>();

        // If we played it on a BattleSquare and there is a creature on the square
        return targetBattleSquare && targetBattleSquare.IsCreatureOnSquare();
    }
}
