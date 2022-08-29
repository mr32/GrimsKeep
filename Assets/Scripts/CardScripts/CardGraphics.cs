﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardGraphics : MonoBehaviour
{
    public GameObject cardNameGraphic;
    public GameObject cardDescGraphic;
    public GameObject cardFlairGraphic;
    public GameObject cardPowerGraphic;
    public GameObject cardDefenseGraphic;
    public GameObject cardCostGraphic;

    void Awake(){
        cardNameGraphic = Utils.FindChildWithTag(this.gameObject, CardConstants.CARD_NAME_TAG);
        cardDescGraphic = Utils.FindChildWithTag(this.gameObject, CardConstants.CARD_DESC_TAG);
        cardFlairGraphic = Utils.FindChildWithTag(this.gameObject, CardConstants.CARD_FLAIR_TEXT);
        cardPowerGraphic = Utils.FindChildWithTag(this.gameObject, CardConstants.CARD_POWER_TAG);
        cardDefenseGraphic = Utils.FindChildWithTag(this.gameObject, CardConstants.CARD_DEFENSE_TAG);
        cardCostGraphic = Utils.FindChildWithTag(this.gameObject, CardConstants.CARD_COST_TAG);
    }

    public void SetCardGraphics(CardInfo cardInfo){
        cardNameGraphic.GetComponentInChildren<Text>().text = cardInfo.cardName;
        cardDescGraphic.GetComponentInChildren<Text>().text = cardInfo.cardDescription;
        cardCostGraphic.GetComponentInChildren<Text>().text = cardInfo.cardCost.ToString();
        
        if(cardInfo.cardType == CardInfo.CardType.MONSTER){
            CreatureCard creatureCard = (CreatureCard) cardInfo;
            cardPowerGraphic.GetComponentInChildren<Text>().text = creatureCard.baseCreaturePower.ToString();
        }

        if(cardInfo.cardType == CardInfo.CardType.SPELL || cardInfo.cardType == CardInfo.CardType.CREATURE_MODIFIER){
            cardPowerGraphic.SetActive(false);
            cardDefenseGraphic.SetActive(false);
        }
        
    }

   
    
}
