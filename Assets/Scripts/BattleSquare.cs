using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class BattleSquare : HoverableObject
{
    private GameController gameController;
    public int orderInColumn;
    private CardInfo currentCard;
    public BattlePaneStats battlePaneStats;

    public GameObject cardPrefab;
    private List<GameObject> cardsPreviewedOnHover = new List<GameObject>();
    public GameObject battleSquarePreviewPanel;
    public GameObject battleSquarePreviewContentPane;
    public bool battleSquareClicked;

    void Start(){
        gameController = GameObject.FindGameObjectWithTag(Constants.GAME_CONTROLLER_TAG).GetComponent<GameController>();
        orderInColumn = this.transform.GetSiblingIndex();
        battleSquarePreviewPanel = GameObject.FindGameObjectWithTag(Constants.BATTLE_SQUARE_PREVIEW_PANE_TAG);
        battleSquarePreviewContentPane = GameObject.FindGameObjectWithTag(Constants.BATTLE_SQUARE_PREVIEW_CONTENT_PANE_TAG);
        
        // if(battleSquarePreviewPanel.activeSelf){
        //     battleSquarePreviewPanel.SetActive(false);
        // }
        
    }

    void Update(){

        if(mouseOnObject && Input.GetMouseButtonDown(0)){
            if(!gameController.cardBeingPlayed){
                battleSquareClicked = !battleSquareClicked;
            }else{
                currentCard = gameController.activeCard.GetComponent<CardInfo>();

                if(currentCard.CanPlayCardOnObject(this.gameObject)){
                    this.gameObject.GetComponent<Image>().color = Color.red;
                    PlayCardOnSquare();
                }
            }
        }

        if(battleSquareClicked){
            ShowCardsPlayedOnSquare();
        }
        else
        {
            DestroyCardPreview();
        }
    }

    public void CleanSquare(){
        currentCard = null;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        ShowCardsPlayedOnSquare();
        gameController.battleSquareToPlayOn = this.gameObject;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        DestroyCardPreview();
        gameController.battleSquareToPlayOn = null;
    }

    public void PlayCardOnSquare(){
        currentCard = gameController.activeCard.GetComponent<CardInfo>();
        currentCard.PlayCard(Constants.BATTLE_SQUARE_ID, this.gameObject, currentCard);
        CopyCardInfoToSquare();
        
        
        // update battle square graphics here
    
        // send request to update battlepanestats
        battlePaneStats.UpdateGraphics(this.gameObject.transform.parent);

        gameController.CleanController();
        currentCard = null;
            
    }

    public CardInfo[] GetCardsPlayedOnSquare(){
        CardInfo[] cardsPlayedOnSquare = this.gameObject.GetComponents<CardInfo>();
        foreach(CardInfo cardInfo in cardsPlayedOnSquare){
            cardInfo.enabled = true;
        }
        return cardsPlayedOnSquare;
    }

    public bool IsCreatureOnSquare(){
        return GetCardsPlayedOnSquare().Where(card => card.cardType == CardInfo.CardType.MONSTER).ToArray().Length > 0;
    }

    private void CopyCardInfoToSquare(){
        if (currentCard == null){
            return;
        }

        currentCard.cardCopied = true;
        UnityEditorInternal.ComponentUtility.CopyComponent(currentCard);
        UnityEditorInternal.ComponentUtility.PasteComponentAsNew(this.gameObject);
    }

    private void ShowCardsPlayedOnSquare(){
        CardInfo[] cardsOnSquare = GetCardsPlayedOnSquare();

        if(cardsOnSquare.Length > 0){
            battleSquarePreviewPanel.SetActive(true);
        }
        
        foreach(CardInfo cardInfo in cardsOnSquare){
            GameObject card = Instantiate(cardPrefab);
            card.transform.SetParent(battleSquarePreviewContentPane.transform);
            card.transform.localScale = new Vector3(1f, 1f, 1f);
            card.GetComponent<CardMovement>().enabled = false;
            card.AddComponent(cardInfo.GetType());
            cardsPreviewedOnHover.Add(card);
        }
    }

    private void DestroyCardPreview(){
        if(cardsPreviewedOnHover.Count > 0){
            foreach(GameObject card in cardsPreviewedOnHover){
                Destroy(card);
            }
            battleSquarePreviewPanel.SetActive(false);
        }
    }
}
