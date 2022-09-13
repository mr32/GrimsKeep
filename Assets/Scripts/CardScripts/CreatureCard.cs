﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CreatureCard : Card
{
    protected abstract int BaseCreaturePower { get; }
    protected abstract int BaseCreatureDefense { get; }
    protected abstract int BaseCreatureHealth { get; }
    public override CardTypes CardType => CardTypes.MONSTER;
    private int currentCreatureHP;
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

    public CreatureCard(){
        currentCreatureHP = BaseCreatureHealth;
    }
    public int GetTotalPowerTotal(){
        int total = BaseCreaturePower;

        foreach(var item in cardModifiers)
        {
            foreach(var inner_item in cardModifiers[item.Key])
            {
                total += inner_item.Value;
            }
        }

        return total;
    }

    public void SetCreatureHP(int value){
        currentCreatureHP = value;
    }

    public int GetTotalCurrentCreatureHP(){
        return currentCreatureHP;
    }

    public int GetTotalCurrentDefenseCreatureHP()
    {
        return BaseCreatureDefense;
    }

    public override void SoftResetCardValues()
    {
        List<CardTypes> cardTypesToExcludeFromRemoval = new List<CardTypes> { CardTypes.COMMANDER };
        var keysToRemove = cardModifiers.Keys.Except(cardTypesToExcludeFromRemoval).ToList();
        foreach(var key in keysToRemove)
        {
            cardModifiers.Remove(key);
        }
        cardModified = cardModifiers.Count > 0;
    }

    public override void HardResetCardValues()
    {
        base.HardResetCardValues();
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

    public virtual void AttackCreature(CreatureCard target)
    {
        target.SetCreatureHP(target.GetTotalCurrentCreatureHP() - GetTotalPowerTotal());
        if(target.GetTotalCurrentCreatureHP() <= 0){
            // Remove that card from the target BattleSquare
            target.battleSquare.cardsPlayedOnObject.Remove(target);
            
            // If there are no more enemy cards on the square
            if(!target.battleSquare.AnyEnemyCardsOnSquare()){
                BattleSquare previousBattleSquare = battleSquare;
                PlayCard(target.battleSquare.gameObject);

                previousBattleSquare.cardsPlayedOnObject.Remove(this);
                previousBattleSquare.ResetBattleSquareToDefaultState(false);
            }
        }
        else
        {
            // Nothing gets played, just update graphics
            target.battleSquare.UpdateAttackAndDefenseGraphics();
        }
    }
}
