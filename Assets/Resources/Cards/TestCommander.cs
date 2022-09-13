using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCommander : CommanderCard
{
    protected override int BaseCreaturePower => 1;

    protected override int BaseCreatureDefense => 5;

    public override string CardName => "Test Commander";

    public override string CardDescription => "Commander of the deck";

    public override uint CardCost { get => 6; set => cardCost = value; }

    public override string CardFlair => "My Life";

    protected override int BaseCreatureHealth => 10;

    public override bool CanPlayCardOnTarget(GameObject target)
    {
        BattleSquare targetBattleSquare = target.GetComponent<BattleSquare>();

        return targetBattleSquare && !targetBattleSquare.IsCreatureOnSquare();
    }

    public override void PlayCard(GameObject target)
    {
        SoftResetCardValues();
        base.PlayCard(target);
    }

    public override void OnBoardConditions()
    {
        foreach(BattleSquare battleSquare in battleBoard.GetComponentsInChildren<BattleSquare>())
        {
            foreach(CreatureCard creature in battleSquare.GetCreatureCardsPlayedOnSquare())
            {
                if(creature != this && creature.cardOwner == cardOwner)
                {
                    Utils.AddToCardModifiers(
                        cardModifierDictionary: creature.cardModifiers,
                        cardToAdd: this,
                        valueToAdd: 10
                    );
                    creature.cardModified = true;
                    battleSquare.UpdateAttackAndDefenseGraphics();
                }
            }
        }
    }
}
