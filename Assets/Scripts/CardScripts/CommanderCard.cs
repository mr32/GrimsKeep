using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommanderCard : CreatureCard
{
    public override MoveDirections[] moveDirections => new MoveDirections[]
    {
        MoveDirections.UP,
        MoveDirections.DOWN,
        MoveDirections.LEFT,
        MoveDirections.RIGHT,
        MoveDirections.BOTTOM_RIGHT,
        MoveDirections.BOTTOM_LEFT,
        MoveDirections.TOP_LEFT,
        MoveDirections.TOP_RIGHT
    };
}
