﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMovement : HoverableObject
{
    private GameObject hand;
    private Transform UICanvas;
    private int placeInHand;
    private GameObject cardCopy;
    private GameObject cardPreviewArea;
    private Vector3 originalScale;

    private GameController gameController;
    
    void Start()
    {
        hand = GameObject.FindGameObjectWithTag(Constants.HAND_AREA_TAG);
        UICanvas = GameObject.FindGameObjectWithTag("UIPanel").transform;
        placeInHand = this.transform.GetSiblingIndex();
        cardPreviewArea = GameObject.FindGameObjectWithTag(Constants.CARD_PREVIEW_TAG);
        originalScale = this.transform.localScale;
        gameController = GameObject.FindGameObjectWithTag(Constants.GAME_CONTROLLER_TAG).GetComponent<GameController>();
    }

    void Update()
    {
        if(mouseOnObject && this.GetComponent<CardInfo>().card.HasEnoughMana() && Input.GetMouseButtonDown(0) && gameController.currentPlayerTurn == Constants.PLAYER1)
        {
            gameController.HoldObject(this.gameObject);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        this.gameObject.GetComponent<Outline>().enabled = true;
        
        if(!gameController.activeObject)
            ShowCardPreviewFromHand();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        this.gameObject.GetComponent<Outline>().enabled = false;
        
        // Only Destroy card if it isnt the active card being played
        if (!GameObject.ReferenceEquals( gameController.activeObject, this.gameObject)){
            Destroy(cardCopy);
        }
    }

    public void ReturnCardToHand(){
        this.transform.SetParent(hand.transform);
        this.transform.SetSiblingIndex(placeInHand);
        this.transform.localScale = originalScale;
    }

    public void ShowCardPreviewFromHand(){
        cardCopy = Instantiate(this.gameObject);
        cardCopy.transform.SetParent(UICanvas);
        cardCopy.transform.position = cardPreviewArea.transform.position;
        cardCopy.transform.localScale = cardPreviewArea.transform.localScale;
        cardCopy.GetComponent<CardInfo>().card = this.GetComponent<CardInfo>().card;
        cardCopy.gameObject.GetComponent<CardMovement>().enabled = false;
    }

    public void DestroyCardPreview(){
        if(cardCopy != null)
            Destroy(cardCopy);
    }

    public void PutCardInGraveyard(){
        Destroy(cardCopy);
        Destroy(this.gameObject);
    }
}
