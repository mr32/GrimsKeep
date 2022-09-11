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
        int total = BaseCreaturePower;

        foreach(var item in cardModifiers)
        {
            total += item.Value;
        }

        return total;
    }

    public override void ResetCardValues()
    {
        List<CardTypes> cardTypesToExcludeFromRemoval = new List<CardTypes> { CardTypes.COMMANDER };
        var keysToRemove = cardModifiers.Keys.Except(cardTypesToExcludeFromRemoval).ToList();
        foreach(var key in keysToRemove)
        {
            cardModifiers.Remove(key);
        }
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
            foreach(SpellCard spellCard in battleSquare.GetCardsPlayedByType(CardTypes.SQUARE_MODIFIER))
            {
                spellCard.ApplyToTarget(this);
            }
        }
    }

    public override void OnBoardConditions()
    {
        CommanderCard commanderCard = GetCommanderCard();

        if(commanderCard != null)
        {
            commanderCard.OnBoardConditions();
        }
    }

}
