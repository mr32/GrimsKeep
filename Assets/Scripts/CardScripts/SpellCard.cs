using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellCard : Card
{
    public bool cardApplied = false;
    public override CardTypes CardType => CardTypes.SPELL;

    public override void PlayCard(GameObject target)
    {
        cardApplied = true;
        base.PlayCard(target);
    }

    public override void MoveCard(GameObject target)
    {
        cardApplied = true;
        base.MoveCard(target);
    }
}
