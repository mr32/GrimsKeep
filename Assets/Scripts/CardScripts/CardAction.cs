using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class CardAction : MonoBehaviour
{
    public virtual bool PlayCard(string playedOn, GameObject gameObjectPlayedOn, CardInfo cardPlayed){
        if(!CanPlayCardOnObject(gameObjectPlayedOn)){
            return false;
        }
        CardRules(gameObjectPlayedOn);
        CopyCardToGameObject(gameObjectPlayedOn);
        GameObject.FindGameObjectWithTag(Constants.PLAYER_STAT_GAMEOBJECT_TAG).GetComponent<PlayerStats>().playerMana -= (int) cardPlayed.CardCost;
        GameObject.FindGameObjectWithTag(Constants.PLAYER_STAT_GAMEOBJECT_TAG).GetComponent<PlayerStats>().UpdateCardGraphics();
        if(this.gameObject.GetComponent<CardMovement>())
            this.gameObject.SendMessage(Constants.CARD_GRAVEYARD_FUNCTION_NAME);
        return true;
    }

    private void CopyCardToGameObject(GameObject gameObjectPlayedOn)
    {
        CardInfo cardInfo = gameObjectPlayedOn.AddComponent(this.GetType()) as CardInfo;
        ((CardInfo)this).cardCopied = true;
        CopyClassValues((CardInfo)this, cardInfo);
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

    public abstract bool CanPlayCardOnObject(GameObject gameObjectPlayedOn);
    public abstract void CardRules(GameObject gameObjectPlayedOn);
}
