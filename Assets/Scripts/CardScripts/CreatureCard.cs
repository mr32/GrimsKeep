using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CreatureCard : Card
{
    public abstract int BaseCreaturePower { get; }
    public abstract int BaseCreatureDefense { get; }

    public abstract int BaseCreatureHealth { get; }
    public override CardTypes CardType => CardTypes.MONSTER;

    public int powerModifier = 0;
    public int additionalPowerModifier = 0;
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

    public int GetTotalPowerTotal(){
        return BaseCreaturePower + powerModifier + additionalPowerModifier;
    }

    public override void ResetCardValues()
    {
        powerModifier = 0;
        additionalPowerModifier = 0;
        cardModified = false;
    }
}
