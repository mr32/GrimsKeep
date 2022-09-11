using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpell : SpellCard
{
    public override string CardName => "Test Spell";
    public override string CardDescription => $"Add {creatureModifierAmount} to target Creature";
    public override uint CardCost { get => 1; set => cardCost = value; }
    public override string CardFlair => "Cool Test Spell!";
    public int creatureModifierAmount = 5;
    public override CardTypes CardType => CardTypes.SQUARE_MODIFIER;

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
        return targetBattleSquare && targetBattleSquare.IsCreatureOnSquare();
    }

    public override void ApplyToTarget(Card targetedCard)
    {
        if(targetedCard is CreatureCard c)
        {
            Utils.AddToCardModifiers(
                cardModifierDictionary: c.cardModifiers,
                cardToAdd: this,
                valueToAdd: creatureModifierAmount
            );
            //if (!c.cardModifiers.ContainsKey(CardType))
            //{
            //    c.cardModifiers.Add(CardType, 0);
            //}

            //c.cardModifiers[CardType] += creatureModifierAmount;
            c.cardModified = true;
        }
    }
}
