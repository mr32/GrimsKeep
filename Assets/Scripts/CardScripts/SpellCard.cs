using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCard : CardInfo
{
    public uint creatureModifierAmount = 0;
    public SpellCard(){
        cardType = CardType.SPELL;
    }
}
