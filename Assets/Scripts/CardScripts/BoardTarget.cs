using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardTarget : Card
{
    protected abstract int BasePower { get; }
    protected abstract int BaseDefense { get; }
    protected abstract int BaseHealth { get; }

    protected int currentHP;
    public bool cardModified;

    public BoardTarget()
    {
        currentHP = BaseHealth;
    }

    public void SetCreatureHP(int value)
    {
        currentHP = value;
    }

    public int GetTotalCurrentCreatureHP()
    {
        return currentHP;
    }

    public int GetTotalCurrentDefenseCreatureHP()
    {
        return BaseDefense;
    }

    public int GetTotalPowerTotal()
    {
        int total = BasePower;

        foreach (var item in cardModifiers)
        {
            foreach (var inner_item in cardModifiers[item.Key])
            {
                total += inner_item.Value;
            }
        }

        return total;
    }
}
