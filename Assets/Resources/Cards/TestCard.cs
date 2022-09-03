using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCard : CreatureCard
{
    private uint modifyAmount = 3;
    public CardGraphics cardGraphics;
    void Awake(){
        cardName = "Test Card A";
        cardDescription = $"If the card is in the front\nit will get a power boost of {modifyAmount}";
        baseCreaturePower = 5;
        cardCost = 3;

        allowedToBePlayedOn.Add(
            Constants.BATTLE_SQUARE_ID
        );
    }

    void Start(){
        if(!cardCopied)
            this.gameObject.SendMessage(Constants.SET_CARD_GRAPHICS_FUNCTION_NAME, this);
    }

    public override void CardRules(GameObject gameObjectPlayedOn)
    {
        BattleSquare battleSquare = gameObjectPlayedOn.GetComponent<BattleSquare>();

        if(battleSquare != null && battleSquare.col == 0)
        {
            powerModifier = modifyAmount;
            cardModified = true;
        }
    }

    public override bool CanPlayCardOnObject(GameObject gameObjectPlayedOn)
    {
        if(gameObjectPlayedOn.GetComponent<BattleSquare>().IsCreatureOnSquare()){
            return false;
        }
        return HasEnoughMana();
    }
}
