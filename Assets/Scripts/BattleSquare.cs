﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System.Reflection;

public class BattleSquare : HoverableObject
{
    private GameController gameController;
    public int orderInColumn;
    private CardInfo currentCard;
    public BattlePaneStats battlePaneStats;

    public GameObject cardPrefab;
    private GameObject battleSquarePreviewPanel;
    private GameObject battleSquarePreviewContentPane;
    private GameObject battleSquareAttackGraphic;
    private GameObject battleSquareDefenseGraphic;
    private bool battleSquareClicked;

    void Awake()
    {
        battleSquarePreviewPanel = GameObject.FindGameObjectWithTag(Constants.BATTLE_SQUARE_PREVIEW_PANE_TAG);
        battleSquarePreviewContentPane = GameObject.FindGameObjectWithTag(Constants.BATTLE_SQUARE_PREVIEW_CONTENT_PANE_TAG);

        
        battleSquareAttackGraphic = Utils.FindChildWithTag(this.gameObject, Constants.BATTLE_SQUARE_ATTACK_GRAPHIC_TAG);
        battleSquareDefenseGraphic = Utils.FindChildWithTag(this.gameObject, Constants.BATTLE_SQUARE_DEFENSE_GRAPHIC_TAG);

        battleSquareAttackGraphic.SetActive(false);
        battleSquareDefenseGraphic.SetActive(false);

    }

    void Start(){
        gameController = GameObject.FindGameObjectWithTag(Constants.GAME_CONTROLLER_TAG).GetComponent<GameController>();
        orderInColumn = this.transform.GetSiblingIndex();

        battleSquarePreviewPanel.SetActive(false);
    }

    void Update(){
        if(mouseOnObject && Input.GetMouseButtonDown(0)){
            int col = this.gameObject.transform.GetSiblingIndex() % 5;
            int row = this.gameObject.transform.GetSiblingIndex() / 5;
            Debug.Log("Row: " + row.ToString() + "\nCol: " + col.ToString());
            if(!gameController.cardBeingPlayed){
                battleSquareClicked = !battleSquareClicked;
                if(battleSquarePreviewPanel.activeSelf){
                    DestroyCardPreview();
                }
                ShowOrHideCardPanel(battleSquareClicked);
            }else{
                currentCard = gameController.activeCard.GetComponent<CardInfo>();

                if(currentCard.CanPlayCardOnObject(this.gameObject)){
                    this.gameObject.GetComponent<Image>().color = Color.red;
                    PlayCardOnSquare();
                    UpdateAttackAndDefenseGraphics();
                }
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
        // battlePaneStats.UpdateGraphics(this.gameObject.transform.parent);

        if(battleSquareClicked){
            ShowCardsPlayedOnSquare();
        }

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

    public CardInfo[] GetCreatureCardsPlayedOnSquare()
    {
        return GetCardsPlayedOnSquare().Where(card => card.cardType == CardInfo.CardType.MONSTER).ToArray();
    }

    public uint CalculateSquarePowerTotals()
    {
        uint total = 0;
        CardInfo[] creatureList = GetCreatureCardsPlayedOnSquare();
        foreach(CreatureCard creatureCard in creatureList)
        {
            total += creatureCard.GetTotalPowerTotal();
        }
        return total;
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

    private void ShowOrHideCardPanel(bool show){
        if(show){
            ShowCardsPlayedOnSquare();
        }else{
            DestroyCardPreview();
        }
    }

    private void ShowCardsPlayedOnSquare(){
        CardInfo[] cardsOnSquare = GetCardsPlayedOnSquare();

        if(cardsOnSquare.Length > 0){
            battleSquarePreviewPanel.SetActive(true);
        }
        
        foreach(CardInfo cardInfo in cardsOnSquare){
            CardInfo cardInfoCopy = cardInfo;
            cardInfoCopy.cardCopied = false;
            GameObject card = Instantiate(cardPrefab);
            card.transform.SetParent(battleSquarePreviewContentPane.transform);
            card.transform.localScale = new Vector3(1f, 1f, 1f);
            card.GetComponent<CardMovement>().enabled = false;
            card.AddComponent(cardInfo.GetType());
            CopyClassValues(cardInfoCopy, card.GetComponent<CardInfo>());
        }
    }

    private void DestroyCardPreview(){
        foreach (Transform child in battleSquarePreviewContentPane.transform) {
            GameObject.Destroy(child.gameObject);
        }
        battleSquarePreviewPanel.SetActive(false);
        battleSquareClicked = false;
    }

    private void CopyClassValues(CardInfo sourceComp, CardInfo targetComp) {
        FieldInfo[] sourceFields = sourceComp.GetType().GetFields(BindingFlags.Public | 
                                                    BindingFlags.NonPublic | 
                                                    BindingFlags.Instance);
        int i = 0;
        for(i = 0; i < sourceFields.Length; i++) {
            var value = sourceFields[i].GetValue(sourceComp);
            sourceFields[i].SetValue(targetComp, value);
        }
    }

    private bool AnyCreatureModifiedOnSquare()
    {
        return GetCreatureCardsPlayedOnSquare().Where(cardType => cardType.cardModified).ToArray().Length > 0;
    }

    private void UpdateAttackAndDefenseGraphics()
    {
        battleSquareAttackGraphic.SetActive(true);
        battleSquareDefenseGraphic.SetActive(true);

        battleSquareAttackGraphic.GetComponentInChildren<Text>().text = CalculateSquarePowerTotals().ToString();

        if (AnyCreatureModifiedOnSquare())
        {
            battleSquareAttackGraphic.GetComponentInChildren<Text>().color = Color.green;
        }

        // TODO: Calculate correct defense total
        battleSquareDefenseGraphic.GetComponentInChildren<Text>().text = 0.ToString();
    }
}
