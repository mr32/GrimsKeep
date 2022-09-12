using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Utils
{
    public static GameObject GetChildWithName(this GameObject obj, string name) => obj.transform.Find(name)?.gameObject;

    public static GameObject FindChildWithTag(GameObject parent, string tag)
    {
        GameObject child = null;

        foreach (Transform transform in parent.transform)
        {
            if (transform.CompareTag(tag))
            {
                child = transform.gameObject;
                break;
            }
        }

        return child;
    }

    public static int CalculateSiblingIndex(int x, int y)
    {
        return x * Constants.BOARD_WIDTH + y;
    }

    public static (int row, int col) GetRowAndColIndex(int siblingIndex)
    {
        return (siblingIndex / Constants.BOARD_WIDTH, siblingIndex % Constants.BOARD_WIDTH);
    }

    public static GameObject CreateCardGameObject(GameObject cardPrefab, Transform parent, Card card, Vector3 scale, Card.CardPlayedFrom cardPlayedFrom, Card.PlayTypes cardOwner)
    {
        GameObject cardObject = UnityEngine.Object.Instantiate(cardPrefab);
        cardObject.transform.SetParent(parent);

        cardObject.transform.localScale = scale;

        CardInfo cardInfo = cardObject.GetComponent<CardInfo>();

        cardInfo.card = card;
        cardInfo.card.cardPlayedFrom = cardPlayedFrom;
       
        cardInfo.card.parentGameobject = cardObject;
        cardInfo.card.cardOwner = cardOwner;
        cardInfo.card.battleBoard = GameObject.FindGameObjectWithTag(Constants.BATTLE_BOARD_TAG);

        cardObject.GetComponent<CardGraphics>().SetCardGraphics();

        return cardObject;
    }

    public static void AddToCardModifiers(Dictionary<Card.CardTypes, Dictionary<string, int>> cardModifierDictionary, Card cardToAdd, int valueToAdd)
    {
        // Only have 1 commander modifier per card
        if(cardToAdd.CardType == Card.CardTypes.COMMANDER && cardModifierDictionary.ContainsKey(Card.CardTypes.COMMANDER)) { return; }
        // If the dictionary doesnt have the General Card Type, then it doesnt have the cardToAdd
        if (!cardModifierDictionary.ContainsKey(cardToAdd.CardType))
        {
            cardModifierDictionary.Add(cardToAdd.CardType, new Dictionary<string, int>() { { cardToAdd.GetType().Name, valueToAdd } });
            return;
        }

        // If the dictionary doesnt have the Specific Card in the dictionary
        if (!cardModifierDictionary[cardToAdd.CardType].ContainsKey(cardToAdd.GetType().Name))
        {
            cardModifierDictionary[cardToAdd.CardType].Add(cardToAdd.GetType().Name, valueToAdd);
            return;
        }

        cardModifierDictionary[cardToAdd.CardType][cardToAdd.GetType().Name] += valueToAdd;
    }
}
