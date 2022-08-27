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
        Object[] cardsForTesting = Resources.LoadAll("Cards");
        for(int i = 0; i < 3; i++){
            foreach(var c in cardsForTesting){
                deck.Add(c.name);
            }
        }
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

        foreach(string cardName in deck){
            GameObject card = Instantiate(cardPrefab);
            card.transform.SetParent(playerHand);
            card.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            card.gameObject.AddComponent(System.Type.GetType(cardName));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
