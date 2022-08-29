using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAction : MonoBehaviour
{
    public virtual bool PlayCard(string playedOn, GameObject gameObjectPlayedOn, CardInfo cardPlayed){
        if(!CanPlayCard(gameObjectPlayedOn, cardPlayed)){
            return false;
        }
        CardRules(gameObjectPlayedOn);
        GameObject.FindGameObjectWithTag(Constants.PLAYER_STAT_GAMEOBJECT_TAG).GetComponent<PlayerStats>().playerMana -= (int) cardPlayed.cardCost;
        GameObject.FindGameObjectWithTag(Constants.PLAYER_STAT_GAMEOBJECT_TAG).GetComponent<PlayerStats>().UpdateCardGraphics();
        this.gameObject.SendMessage(Constants.CARD_GRAVEYARD_FUNCTION_NAME);
        return true;
    }

    // Will return true if the card cost is less than or equal to playerMana
    public virtual bool CanPlayCard(GameObject gameObjectPlayedOn, CardInfo cardPlayed){
        return (int)cardPlayed.cardCost <= GameObject.FindGameObjectWithTag(Constants.PLAYER_STAT_GAMEOBJECT_TAG).GetComponent<PlayerStats>().playerMana;
    }

    public virtual void CardRules(GameObject gameObjectPlayedOn){}
}
