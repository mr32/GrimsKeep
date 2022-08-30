using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreatureCard : CardInfo
{
    public uint baseCreaturePower;
    public uint baseCreatureDefense;
    public uint powerModifier = 0;
    public uint additionalPowerModifier = 0;

    public CreatureCard(){
        cardType = CardType.MONSTER;
    }

    public uint GetTotalPowerTotal(){
        return baseCreaturePower + powerModifier + additionalPowerModifier;
    }
}
