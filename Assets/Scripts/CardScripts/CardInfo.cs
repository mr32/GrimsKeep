using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardInfo : CardAction
{
    public string cardName;
    public string cardDescription;
    public uint cardCost;
    public bool cardCopied = false;
    public bool cardModified = false;

    public enum CardType {
        SPELL,
        MONSTER,
        CREATURE_MODIFIER
    }

    public CardType cardType;
    public List<string> allowedToBePlayedOn = new List<string>();

    public bool HasEnoughMana()
    {
        return (int)cardCost <= GameObject.FindGameObjectWithTag(Constants.PLAYER_STAT_GAMEOBJECT_TAG).GetComponent<PlayerStats>().playerMana;
    }
}
