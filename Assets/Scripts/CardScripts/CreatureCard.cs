using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CreatureCard : BoardTarget
{
    public override CardTypes CardType => CardTypes.MONSTER;
    
    public int killCount = 0;

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
        return string.Format("cName: {0} -- cCost: {1} -- cPower: {2} -- cTotalPower: {3}", CardName, CardCost, BasePower, GetTotalPowerTotal());
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

    public virtual void AttackBoardTarget(BoardTarget target)
    {
        target.SetCreatureHP(target.GetTotalCurrentCreatureHP() - GetTotalPowerTotal());
        if(target.GetTotalCurrentCreatureHP() <= 0){
            killCount += 1;
            target.OnDeath();
            // Remove that card from the target BattleSquare
            target.battleSquare.cardsPlayedOnObject.Remove(target);
            
            // If there are no more enemy cards on the square
            if(!target.battleSquare.HasAnyAttackableCards()){
                BattleSquare previousBattleSquare = battleSquare;
                foreach(Card c in previousBattleSquare.GetMovableCardsPlayedOnSquare())
                {
                    c.PlayCard(target.battleSquare.gameObject);
                }

                previousBattleSquare.ResetBattleSquareToDefaultState(false);
            }

            //Debug.Log(string.Format("{0} has {1} kills!", this.GetType().Name, killCount));
        }
        else
        {
            // Nothing gets played, just update graphics
            target.battleSquare.UpdateAttackAndDefenseGraphics();
        }
    }
}
