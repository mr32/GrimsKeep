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

    void Awake() {
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

        playerHand = GameObject.FindGameObjectWithTag(Constants.HAND_AREA_TAG).transform;

    }
    void Start()
    {
        LoadDeck();
    }

    public void LoadDeck()
    {
        foreach (string cardName in deck)
        {
            Utils.CreateCardGameObject(
                cardPrefab: cardPrefab,
                parent: playerHand,
                card: CreateCardInstanceByName(cardName),
                scale: new Vector3(0.5f, 0.5f, 0.5f),
                cardPlayedFrom: Card.CardPlayedFrom.HAND,
                cardOwner: Card.PlayTypes.SELF
            );
        }
    }

    public Card CreateCardInstanceByName(string s)
    {
        return (Card)Activator.CreateInstance(Type.GetType(s));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
