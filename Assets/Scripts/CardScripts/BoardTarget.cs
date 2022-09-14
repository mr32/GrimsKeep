using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardTarget : Card
{
    protected abstract int BasePower { get; }
    protected abstract int BaseDefense { get; }
    protected abstract int BaseHealth { get; }

    protected int currentHP;

    public BoardTarget()
    {
        currentHP = BaseHealth;
    }
}
