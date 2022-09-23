using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestSpell : SpellCard
{
    public override string CardName => "Creature Square Modifier";
    public override string CardDescription => $"Add {creatureModifierAmount} to target Creature";
    public override uint CardCost { get => 1; set => cardCost = value; }
    public override string CardFlair => "Square Modifier";
    public int creatureModifierAmount = 5;

    public override CardTypes CardType => CardTypes.SQUARE_MODIFIER;
    public override PlayTypes IsAbleToBeUsedOn => PlayTypes.NEUTRAL;

    public override void OnPlayConditions(GameObject gameObjectPlayedOn)
    {
        BattleSquare battleSquare = gameObjectPlayedOn.GetComponent<BattleSquare>();

        foreach(CreatureCard creatureCard in battleSquare.GetCreatureCardsPlayedOnSquare())
        {
            ApplyToTarget(creatureCard);
        }
    }

    public override bool CanPlayCardOnTarget(GameObject target)
    {
        BattleSquare targetBattleSquare = target.GetComponent<BattleSquare>();

        // If we played it on a BattleSquare and there is a creature on the square
        return targetBattleSquare && targetBattleSquare.IsSquareOccupied();
    }

    public override void ApplyToTarget(Card targetedCard)
    {
        if(targetedCard is CreatureCard c && (IsAbleToBeUsedOn == c.cardOwner || IsAbleToBeUsedOn == PlayTypes.NEUTRAL))
        {
            Utils.AddToCardModifiers(
                cardModifierDictionary: c.cardModifiers,
                cardToAdd: this,
                valueToAdd: creatureModifierAmount
            );
            c.cardModified = true;
        }
    }
}
