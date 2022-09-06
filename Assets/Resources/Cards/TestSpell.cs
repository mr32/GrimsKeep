using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpell : SpellCard
{
    public override string CardName => "Test Spell";
    public override string CardDescription => $"Add {creatureModifierAmount} to target Creature";
    public override uint CardCost { get => 1; set => cardCost = value; }
    public override string CardFlair => "Cool Test Spell!";
    void Awake() {
        creatureModifierAmount = 5;
        cardType = CardType.CREATURE_MODIFIER;
    }
    void Start(){
        if(!cardCopied)
            this.gameObject.SendMessage(Constants.SET_CARD_GRAPHICS_FUNCTION_NAME, this);
    }

    public override void CardRules(GameObject gameObjectPlayedOn)
    {
        CreatureCard creatureCard = gameObjectPlayedOn.GetComponent<CreatureCard>();

        creatureCard.additionalPowerModifier += creatureModifierAmount;
    }

    public override bool CanPlayCardOnObject(GameObject gameObjectPlayedOn)
    {
        return HasEnoughMana() && gameObjectPlayedOn.GetComponent<BattleSquare>().IsCreatureOnSquare();
    }
}
