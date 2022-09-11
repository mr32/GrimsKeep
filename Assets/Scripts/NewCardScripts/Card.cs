﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class Card
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
        SQUARE_MODIFIER,
        COMMANDER
    }

    public abstract CardTypes CardType { get; }

    public enum CardSource
    {
        HAND,
        BATTLE_SQUARE
    }

    public CardSource cardSource;

    public Dictionary<CardTypes, int> cardModifiers = new Dictionary<CardTypes, int>();
    public GameObject parentGameobject;

    public GameObject battleBoard;

    public virtual void PlayCard(GameObject target)
    {
        // Remove any square modifiers
        cardModifiers.Remove(CardTypes.SQUARE_MODIFIER);

        // Assumes CanPlayCardOnTarget and HasEnoughMana are called from an external source
        OnPlayConditions(target);

        switch (cardSource)
        {
            case CardSource.HAND:
                GameObject.FindGameObjectWithTag(Constants.PLAYER_STAT_GAMEOBJECT_TAG).GetComponent<PlayerStats>().playerMana -= (int)this.CardCost;
                GameObject.FindGameObjectWithTag(Constants.PLAYER_STAT_GAMEOBJECT_TAG).GetComponent<PlayerStats>().UpdateCardGraphics();
                break;
            default:
                break;
        }

        BattleSquare targetBattleSquare = target.GetComponent<BattleSquare>();

        OccupyBattleSquare(targetBattleSquare);

        OnBoardConditions();

        DiscardCard();
    }

    public override string ToString()
    {
        return string.Format("cName: {0} -- cCost: {1}", CardName, CardCost);
    }

    public virtual void MoveCard(GameObject target)
    {
        ResetCardValues();
        OnPlayConditions(target);
    }

    public virtual void ResetCardValues() { }

    public virtual void OnPlayConditions(GameObject target) { }
    public virtual void OnBoardConditions() { }
    public abstract bool CanPlayCardOnTarget(GameObject target);

    private void DiscardCard()
    {
        if(parentGameobject != null)
        {
            GameObject.Destroy(parentGameobject);
        }
    }

    private void OccupyBattleSquare(BattleSquare target)
    {
        if (target)
        {
            target.objectPlayed = true;
            target.squareOccupied = true;
            target.cardsPlayedOnObject.Add(this);
            cardSource = CardSource.BATTLE_SQUARE;
        }
    }

    public bool HasEnoughMana()
    {
        return (int)this.CardCost <= GameObject.FindGameObjectWithTag(Constants.PLAYER_STAT_GAMEOBJECT_TAG).GetComponent<PlayerStats>().playerMana;
    }

    public CommanderCard GetCommanderCard()
    {
        foreach(BattleSquare battleSquare in battleBoard.GetComponentsInChildren<BattleSquare>())
        {
            if (battleSquare.IsCommanderOnSquare())
            {
                return battleSquare.GetCommanderCard();
            }
        }
        return null;
    }
}
