using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfo : CardAction
{
    public string cardName;
    public string cardDescription;
    public int cardCost;
    public bool cardCopied = false;

    public enum CardType {
        SPELL,
        MONSTER,
        CREATURE_MODIFIER
    }

    public CardType cardType;
    public List<string> allowedToBePlayedOn = new List<string>();
    
}
