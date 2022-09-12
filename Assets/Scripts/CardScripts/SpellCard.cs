using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellCard : Card
{

    public abstract PlayTypes IsAbleToBeUsedOn { get; }
    public override CardTypes CardType => CardTypes.SPELL;

    public abstract void ApplyToTarget(Card targetedCard);
}
