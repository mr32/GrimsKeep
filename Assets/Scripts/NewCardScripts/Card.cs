using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public abstract string CardName { get; }
    public abstract string CardDescription { get; }
    public abstract uint CardCost { get; set; }
    public abstract string CardFlair { get; }

    protected uint cardCost;
    public bool cardCopied = false;

    public enum CardTypes
    {
        MONSTER,
        SPELL,
        CREATURE_MODIFIER,
        SQUARE_MODIFIER
    }

    public void Start()
    {
        if(!cardCopied)
            this.gameObject.SendMessage(Constants.SET_CARD_GRAPHICS_FUNCTION_NAME, this);
    }

    public abstract CardTypes CardType { get; }

    public virtual void PlayCard(GameObject target)
    {
        // Assumes CanPlayCardOnTarget and HasEnoughMana are called from an external source
        CardRules(target);
        CopyCardToTarget(target);

        GameObject.FindGameObjectWithTag(Constants.PLAYER_STAT_GAMEOBJECT_TAG).GetComponent<PlayerStats>().playerMana -= (int)this.CardCost;
        GameObject.FindGameObjectWithTag(Constants.PLAYER_STAT_GAMEOBJECT_TAG).GetComponent<PlayerStats>().UpdateCardGraphics();

        if (!cardCopied || this.GetComponent<CardMovement>())
        {
            this.gameObject.SendMessage(Constants.CARD_GRAVEYARD_FUNCTION_NAME);
        }
    }

    public virtual void MoveCard(GameObject target)
    {
        ResetCardValues();
        CardRules(target);
        CopyCardToTarget(target);
    }

    public virtual void ResetCardValues() { }

    public virtual void CardRules(GameObject target) { }
    public abstract bool CanPlayCardOnTarget(GameObject target);

    public bool HasEnoughMana()
    {
        return (int)this.CardCost <= GameObject.FindGameObjectWithTag(Constants.PLAYER_STAT_GAMEOBJECT_TAG).GetComponent<PlayerStats>().playerMana;
    }

    public void CopyCardToTarget(GameObject target)
    {
        cardCopied = true;
        Card copiedCard = target.AddComponent(this.GetType()) as Card;
        CopyClassValues(this, copiedCard);
    }

    private void CopyClassValues(Card sourceComp, Card targetComp)
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
