using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleColumn : MonoBehaviour
{
    // public int powerTotal = 0;
    // private int defenseTotal = 0;
    public uint CalculateColumnStats(){
        uint power = 0;
        foreach (BattleSquare battleSquare in this.gameObject.GetComponentsInChildren<BattleSquare>()){
            CardInfo[] creatureList = battleSquare.GetCardsPlayedOnSquare().Where(card => card.cardType == CardInfo.CardType.MONSTER).ToArray();
            foreach (CreatureCard cardInfo in creatureList){
                power += cardInfo.GetTotalPowerTotal();
            }
        }
        return power;
    }
}
