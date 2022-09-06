using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreatureCard : CardInfo
{
    public abstract uint baseCreaturePower { get; }
    public abstract uint baseCreatureDefense { get; }
    public uint powerModifier = 0;
    public uint additionalPowerModifier = 0;

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
        cardType = CardType.MONSTER;
    }

    public uint GetTotalPowerTotal(){
        return baseCreaturePower + powerModifier + additionalPowerModifier;
    }

}
