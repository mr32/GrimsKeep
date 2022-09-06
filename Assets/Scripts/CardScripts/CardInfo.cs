using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardInfo : CardAction
{
    public abstract string CardName { get; }
    public abstract string CardDescription { get; }
    public abstract uint CardCost { get; set; }

    public abstract string CardFlair { get; }

    public bool cardCopied = false;
    public bool cardModified = false;
    protected uint cardCost;

    public enum CardType {
        SPELL,
        MONSTER,
        CREATURE_MODIFIER
    }

    public CardType cardType;
    public List<string> allowedToBePlayedOn = new List<string>();

    public bool HasEnoughMana()
    {
        return (int)CardCost <= GameObject.FindGameObjectWithTag(Constants.PLAYER_STAT_GAMEOBJECT_TAG).GetComponent<PlayerStats>().playerMana;
    }
}
