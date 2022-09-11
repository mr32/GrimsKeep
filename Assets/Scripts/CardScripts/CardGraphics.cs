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

    public void SetCardGraphics(){
        Card cardToShow = this.gameObject.GetComponent<CardInfo>().card;

        if(cardToShow == null)
        {
            throw new System.Exception("Card is not set");
        }

        cardNameGraphic.GetComponentInChildren<Text>().text = cardToShow.CardName;
        cardDescGraphic.GetComponentInChildren<Text>().text = cardToShow.CardDescription;
        cardCostGraphic.GetComponentInChildren<Text>().text = cardToShow.CardCost.ToString();
        cardFlairGraphic.GetComponentInChildren<Text>().text = cardToShow.CardFlair;

        if (cardToShow is CreatureCard c)
        {
            cardPowerGraphic.GetComponentInChildren<Text>().text = c.BaseCreaturePower.ToString();
            if (c.cardModified)
            {
                cardPowerGraphic.GetComponentInChildren<Text>().text = c.GetTotalPowerTotal().ToString();
                cardPowerGraphic.GetComponentInChildren<Text>().color = Color.green;
            }
        }

        if (cardToShow is SpellCard)
        {
            cardPowerGraphic.SetActive(false);
            cardDefenseGraphic.SetActive(false);
        }

    }

   
    
}
