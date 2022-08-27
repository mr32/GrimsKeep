using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAction : MonoBehaviour
{
    public virtual bool PlayCard(string playedOn, GameObject gameObjectPlayedOn){
        if(!CanPlayCard(gameObjectPlayedOn)){
            return false;
        }
        CardRules(gameObjectPlayedOn);
        this.gameObject.SendMessage(Constants.CARD_GRAVEYARD_FUNCTION_NAME);
        return true;
    }

    public virtual bool CanPlayCard(GameObject gameObjectPlayedOn){
        return true;
    }

    public virtual void CardRules(GameObject gameObjectPlayedOn){}
}
