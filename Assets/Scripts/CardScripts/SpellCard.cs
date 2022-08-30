using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellCard : CardInfo
{
    public uint creatureModifierAmount = 0;
    public SpellCard(){
        cardType = CardType.SPELL;
    }
}
