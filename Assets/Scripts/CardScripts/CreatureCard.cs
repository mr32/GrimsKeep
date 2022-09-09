using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CreatureCard : Card
{
    public abstract int BaseCreaturePower { get; }
    public abstract int BaseCreatureDefense { get; }

    public abstract int BaseCreatureHealth { get; }
    public override CardTypes CardType => CardTypes.MONSTER;

    public int powerModifier = 0;
    public int additionalPowerModifier = 0;
    public bool cardModified;

    public enum MoveDirections
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        TOP_LEFT,
        TOP_RIGHT,
        BOTTOM_RIGHT,
        BOTTOM_LEFT
    }

    public abstract MoveDirections[] moveDirections { get; } 

    public int GetTotalPowerTotal(){
        return BaseCreaturePower + powerModifier + additionalPowerModifier;
    }

    public override void ResetCardValues()
    {
        powerModifier = 0;
        additionalPowerModifier = 0;
        cardModified = false;
    }

    public override string ToString()
    {
        return string.Format("cName: {0} -- cCost: {1} -- cPower: {2} -- cTotalPower: {3}", CardName, CardCost, BaseCreaturePower, GetTotalPowerTotal());
    }

    public override void PlayCard(GameObject target)
    {
        base.PlayCard(target);

        // Check if there are any existing spell modifiers
        BattleSquare battleSquare = target.GetComponent<BattleSquare>();

        if (battleSquare)
        {
            foreach(SpellCard spellCard in battleSquare.GetFilteredCardsPlayedOnSquare(CardTypes.SQUARE_MODIFIER))
            {
                spellCard.ApplyToCreature(this);
            }
        }
    }

}
