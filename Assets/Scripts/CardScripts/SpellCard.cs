using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellCard : Card
{
    public bool cardApplied = false;
    public override CardTypes CardType => CardTypes.SPELL;
}
