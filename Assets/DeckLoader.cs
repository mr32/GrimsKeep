using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckLoader : MonoBehaviour
{
    public GameObject cardPrefab;
    private Transform playerHand;
    // Start is called before the first frame update
    public List<string> deck = new List<string>();

    void Awake(){
        //UnityEngine.Object[] cardsForTesting = Resources.LoadAll("Cards");
        //for(int i = 0; i < 3; i++){
        //    foreach(var c in cardsForTesting){
        //        deck.Add(c.name);
        //    }
        //}

        deck.Add("TestCard");
        deck.Add("TestCard");
        deck.Add("TestCard");
        deck.Add("TestSpell");
        deck.Add("TestSpell");
        deck.Add("TestCommander");

        // deck = new List<string>(){
        //     CardNames.TEST_CARD,
        //     CardNames.TEST_CARD,
        //     CardNames.TEST_CARD,
        //     CardNames.TEST_CARD,
        // };
    }
    void Start()
    {
        playerHand = GameObject.FindGameObjectWithTag(Constants.HAND_AREA_TAG).transform;
        bool testBool = false;

        foreach(string cardName in deck){
            GameObject cardMade = Utils.CreateCardGameObject(
                cardPrefab: cardPrefab,
                parent: playerHand,
                card: (Card)Activator.CreateInstance(Type.GetType(cardName)),
                scale: new Vector3(0.5f, 0.5f, 0.5f),
                cardPlayedFrom: Card.CardPlayedFrom.HAND,
                cardOwner: Card.CardOwner.SELF
            );
            if (!testBool)
            {
                cardMade.GetComponent<CardInfo>().card.cardOwner = Card.CardOwner.ENEMY;
                testBool = true;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
