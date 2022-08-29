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

    void Start(){
        gameController = GameObject.FindGameObjectWithTag(Constants.GAME_CONTROLLER_TAG).GetComponent<GameController>();
        orderInColumn = this.transform.GetSiblingIndex();
    }

    void Update(){
        if (mouseOnObject && gameController.cardBeingPlayed && Input.GetMouseButtonDown(0)){
            currentCard = gameController.activeCard.GetComponent<CardInfo>();

            if(currentCard.CanPlayCard(this.gameObject, currentCard)){
                this.gameObject.GetComponent<Image>().color = Color.red;
                PlayCardOnSquare();
            }
        }
    }

    public void CleanSquare(){
        currentCard = null;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        gameController.battleSquareToPlayOn = this.gameObject;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
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
}
