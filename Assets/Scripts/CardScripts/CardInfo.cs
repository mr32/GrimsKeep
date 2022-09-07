using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class CardInfo : CardAction
{
    public abstract string CardName { get; }
    public abstract string CardDescription { get; }
    public abstract uint CardCost { get; set; }

    public abstract string CardFlair { get; }

    public bool cardCopied = false;
    public bool cardMoved = false;
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

    public virtual bool MoveCard(string playedOn, GameObject gameObjectMovedTo, GameObject gameObjectMovedFrom)
    {
        if (!CanPlayCardOnObject(gameObjectMovedTo))
        {
            return false;
        }
        CardRules(gameObjectMovedTo);
        CardInfo cardInfo = gameObjectMovedTo.AddComponent(this.GetType()) as CardInfo;
        CopyClassValues(this, cardInfo);
        return true;
    }

    private void CopyClassValues(CardInfo sourceComp, CardInfo targetComp)
    {
        FieldInfo[] sourceFields = sourceComp.GetType().GetFields(BindingFlags.Public |
                                                    BindingFlags.NonPublic |
                                                    BindingFlags.Instance);
        int i = 0;
        for (i = 0; i < sourceFields.Length; i++)
        {
            var value = sourceFields[i].GetValue(sourceComp);
            sourceFields[i].SetValue(targetComp, value);
        }
    }
}
