using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardAction : MonoBehaviour
{
    public virtual bool PlayCard(string playedOn, GameObject gameObjectPlayedOn, CardInfo cardPlayed){
        if(!CanPlayCardOnObject(gameObjectPlayedOn)){
            return false;
        }
        CardRules(gameObjectPlayedOn);
        GameObject.FindGameObjectWithTag(Constants.PLAYER_STAT_GAMEOBJECT_TAG).GetComponent<PlayerStats>().playerMana -= (int) cardPlayed.CardCost;
        GameObject.FindGameObjectWithTag(Constants.PLAYER_STAT_GAMEOBJECT_TAG).GetComponent<PlayerStats>().UpdateCardGraphics();
        this.gameObject.SendMessage(Constants.CARD_GRAVEYARD_FUNCTION_NAME);
        return true;
    }

    public abstract bool CanPlayCardOnObject(GameObject gameObjectPlayedOn);
    public abstract void CardRules(GameObject gameObjectPlayedOn);
}
