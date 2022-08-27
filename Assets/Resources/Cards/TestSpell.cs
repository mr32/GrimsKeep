using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpell : SpellCard
{
    void Awake() {
        cardName = "Test Spell";
        creatureModifierAmount = 5;
        cardDescription = $"Add {creatureModifierAmount} to target Creature";
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

    public override bool CanPlayCard(GameObject gameObjectPlayedOn)
    {
        return gameObjectPlayedOn.GetComponent<BattleSquare>().IsCreatureOnSquare();
    }
}
