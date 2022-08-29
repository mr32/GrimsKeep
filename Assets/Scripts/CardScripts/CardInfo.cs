using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfo : CardAction
{
    public string cardName;
    public string cardDescription;
    public uint cardCost;
    public bool cardCopied = false;

    public enum CardType {
        SPELL,
        MONSTER,
        CREATURE_MODIFIER
    }

    public CardType cardType;
    public List<string> allowedToBePlayedOn = new List<string>();

    public override bool CanPlayCard(GameObject gameObjectPlayedOn)
    {
        return (int)cardCost <= GameObject.FindGameObjectWithTag(Constants.PLAYER_STAT_GAMEOBJECT_TAG).GetComponent<PlayerStats>().playerMana;
    }

    public override bool CanPlayCard()
    {
        return (int)cardCost <= GameObject.FindGameObjectWithTag(Constants.PLAYER_STAT_GAMEOBJECT_TAG).GetComponent<PlayerStats>().playerMana;
    }
}
