using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGraphics : MonoBehaviour
{
    public GameObject cardNameGraphic;
    public GameObject cardDescGraphic;
    public GameObject cardFlairGraphic;
    public GameObject cardPowerGraphic;

    void Awake(){
        cardNameGraphic = Utils.GetChildWithName(this.gameObject, "Name");
        cardDescGraphic = Utils.GetChildWithName(this.gameObject, "Description");
        cardFlairGraphic = Utils.GetChildWithName(this.gameObject, "Flair Text");
        cardPowerGraphic = Utils.GetChildWithName(this.gameObject, "CardPower");
    }

    public void SetCardGraphics(CardInfo cardInfo){
        cardNameGraphic.GetComponentInChildren<Text>().text = cardInfo.cardName;
        cardDescGraphic.GetComponentInChildren<Text>().text = cardInfo.cardDescription;
        
        if(cardInfo.cardType == CardInfo.CardType.MONSTER){
            CreatureCard creatureCard = (CreatureCard) cardInfo;
            cardPowerGraphic.GetComponentInChildren<Text>().text = creatureCard.baseCreaturePower.ToString();
        }

        if(cardInfo.cardType == CardInfo.CardType.SPELL || cardInfo.cardType == CardInfo.CardType.CREATURE_MODIFIER){
            cardPowerGraphic.gameObject.SetActive(false);
        }
        
    }

   
    
}
