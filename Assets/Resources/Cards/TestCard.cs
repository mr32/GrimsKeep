using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCard : CreatureCard
{
    private uint modifyAmount = 3;
    public CardGraphics cardGraphics;

    public override MoveDirections[] moveDirections { 
        get{ 
            return new MoveDirections[]{ 
                MoveDirections.UP, 
                //MoveDirections.DOWN, 
                //MoveDirections.TOP_LEFT, 
                //MoveDirections.LEFT, 
                //MoveDirections.RIGHT, 
                //MoveDirections.TOP_RIGHT, 
                //MoveDirections.BOTTOM_RIGHT,
                //MoveDirections.BOTTOM_LEFT
            }; 
        }
    }

    public override string CardName => "Test Card A";
    public override string CardDescription => $"If the card is in the front\nit will get a power boost of {modifyAmount}";
    public override uint baseCreaturePower => 5;
    public override uint baseCreatureDefense => 3;
    public override string CardFlair => "LARGE CREATURE";
    public override uint CardCost { get => 3; set => cardCost = value; }

    void Awake(){
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
        else
        {
            ResetCardValues();
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
