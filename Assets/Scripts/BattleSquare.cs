﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class BattleSquare : HoverableObject
{
    private GameController gameController;
    public int orderInColumn;

    public int row;
    public int col;

    public GameObject cardPrefab;
    private GameObject battleSquareAttackGraphic;
    private GameObject battleSquareDefenseGraphic;

    private GameObject battlePlaySquare;

    public bool objectPlayed = false;

    public bool squareOccupied;

    public List<int> availablePlacesToMove = new List<int>();

    public List<Card> cardsPlayedOnObject = new List<Card>();

    void Awake()
    {
        battlePlaySquare = this.gameObject.transform.parent.gameObject;

        
        battleSquareAttackGraphic = Utils.FindChildWithTag(this.gameObject, Constants.BATTLE_SQUARE_ATTACK_GRAPHIC_TAG);
        battleSquareDefenseGraphic = Utils.FindChildWithTag(this.gameObject, Constants.BATTLE_SQUARE_DEFENSE_GRAPHIC_TAG);

        battleSquareAttackGraphic.SetActive(false);
        battleSquareDefenseGraphic.SetActive(false);

        row = this.gameObject.transform.GetSiblingIndex() / Constants.BOARD_WIDTH;
        col = this.gameObject.transform.GetSiblingIndex() % Constants.BOARD_WIDTH;

        // To get child sibling index
        // i = row * Constants.BOARD_WIDTH + col

    }

    void Start(){
        gameController = GameObject.FindGameObjectWithTag(Constants.GAME_CONTROLLER_TAG).GetComponent<GameController>();
        orderInColumn = this.transform.GetSiblingIndex();
    }

    void Update(){
        
        if(mouseOnObject && Input.GetMouseButtonDown(0)){
            if (gameController.activeObject)
            {
                // If the activeObject is a Card
                if (gameController.activeObject.CompareTag(Constants.CARD_TAG) && gameController.activeObject.GetComponent<CardInfo>().card.CanPlayCardOnTarget(this.gameObject))
                {
                    Card card = gameController.activeObject.GetComponent<CardInfo>().card;
                    card.PlayCard(this.gameObject);

                    gameController.ResetSelf();
                }
                else if (gameController.activeObject.CompareTag(Constants.BATTLE_SQUARE_ID) && gameController.activeObject.GetComponent<BattleSquare>().availablePlacesToMove.Contains(orderInColumn))
                {
                    foreach (Card card in gameController.activeObject.GetComponent<BattleSquare>().GetMovableCardsPlayedOnSquare())
                    {
                        card.PlayCard(this.gameObject);
                    }
                    
                    // clean the square that the cards came from
                    gameController.activeObject.GetComponent<BattleSquare>().ResetBattleSquareToDefaultState(false);
                    gameController.ResetSelf();
                    battlePlaySquare.GetComponent<BattleBoard>().ResetSelf();
                }

                if (objectPlayed)
                {
                    UpdateAttackAndDefenseGraphics();
                    objectPlayed = false;
                }                
            }
            else
            {
                CreatureCard.MoveDirections[] moves = GetFilteredCardsPlayedOnSquare(Card.CardTypes.MONSTER).SelectMany(x => ((CreatureCard)x).moveDirections).ToArray().Distinct().Cast<CreatureCard.MoveDirections>().ToArray();
                LightUpMoveSquares(moves);
                PickUpCardsOnSquare();
            }
        }
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
    
    private void PickUpCardsOnSquare()
    {
        gameController.activeObject = this.gameObject;
        gameController.userGraphicsUp = true;
    }

    private int CalculateSiblingIndex(int x, int y)
    {
        return x * Constants.BOARD_WIDTH + y;
    }

    private void LightUpMoveSquares(CreatureCard.MoveDirections[] possibleMoveDirections)
    {
        foreach(CreatureCard.MoveDirections move in possibleMoveDirections)
        {
            switch (move)
            {
                case CreatureCard.MoveDirections.UP:
                    if(IsValidBoardSquare(row - 1, col))
                    {
                        ColorSquare(CalculateSiblingIndex(row - 1, col), Color.green);
                    }
                    break;
                case CreatureCard.MoveDirections.DOWN:
                    if (IsValidBoardSquare(row + 1, col))
                    {
                        ColorSquare(CalculateSiblingIndex(row + 1, col), Color.green);
                    }
                    break;
                case CreatureCard.MoveDirections.LEFT:
                    if (IsValidBoardSquare(row, col - 1))
                    {
                        ColorSquare(CalculateSiblingIndex(row, col - 1), Color.green);
                    }
                    break;
                case CreatureCard.MoveDirections.RIGHT:
                    if (IsValidBoardSquare(row, col + 1))
                    {
                        ColorSquare(CalculateSiblingIndex(row, col + 1), Color.green);
                    }
                    break;
                case CreatureCard.MoveDirections.TOP_LEFT:
                    if (IsValidBoardSquare(row - 1, col - 1))
                    {
                        ColorSquare(CalculateSiblingIndex(row - 1, col - 1), Color.green);
                    }
                    break;
                case CreatureCard.MoveDirections.TOP_RIGHT:
                    if (IsValidBoardSquare(row - 1, col + 1))
                    {
                        ColorSquare(CalculateSiblingIndex(row - 1, col + 1), Color.green);
                    }
                    break;
                case CreatureCard.MoveDirections.BOTTOM_LEFT:
                    if (IsValidBoardSquare(row + 1, col - 1))
                    {
                        ColorSquare(CalculateSiblingIndex(row + 1, col - 1), Color.green);
                    }
                    break;
                case CreatureCard.MoveDirections.BOTTOM_RIGHT:
                    if (IsValidBoardSquare(row + 1, col + 1))
                    {
                        ColorSquare(CalculateSiblingIndex(row + 1, col + 1), Color.green);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void AddToLitSquaresIndex(int siblingIndex)
    {
        if (!battlePlaySquare.transform.GetChild(siblingIndex).GetComponent<BattleSquare>().squareOccupied)
        {
            battlePlaySquare.GetComponent<BattleBoard>().battleSquareIndiciesLit.Add(siblingIndex);
            availablePlacesToMove.Add(siblingIndex);
        }
    }

    private void ColorSquare(int siblingIndex, Color color)
    {
        Transform battleSquare = battlePlaySquare.transform.GetChild(siblingIndex);

        if (battleSquare && !battleSquare.GetComponent<BattleSquare>().squareOccupied)
        {
            battleSquare.GetComponent<Image>().color = color;
            AddToLitSquaresIndex(siblingIndex);
            battlePlaySquare.GetComponent<BattleBoard>().userGraphicsUp = true;
        }
    }

    private bool IsValidBoardSquare(int row, int col)
    {
        if(row >= 0 && col >= 0 && row < Constants.BOARD_WIDTH && col < Constants.BOARD_WIDTH)
        {
            return true;
        }
        return false;
    }



    public List<Card> GetCreatureCardsPlayedOnSquare()
    {
        return cardsPlayedOnObject.Where(card => card.CardType == Card.CardTypes.MONSTER).ToList();
    }

    public List<Card> GetMovableCardsPlayedOnSquare()
    {
        return cardsPlayedOnObject.Where(card => card.CardType != Card.CardTypes.SQUARE_MODIFIER).ToList();
    }

    public List<Card> GetFilteredCardsPlayedOnSquare(Card.CardTypes cardType)
    {
        return cardsPlayedOnObject.Where(card => card.CardType == cardType).ToList();
    }

    public int CalculateSquarePowerTotals()
    {
        int total = 0;
        List<Card> creatureList = GetFilteredCardsPlayedOnSquare(Card.CardTypes.MONSTER);
        foreach(CreatureCard creatureCard in creatureList)
        {
            total += creatureCard.GetTotalPowerTotal();
        }
        return total;
    }

    public bool IsCreatureOnSquare(){
        return GetCreatureCardsPlayedOnSquare().Count > 0;
    }

    private bool AnyCreatureModifiedOnSquare()
    {
        return GetFilteredCardsPlayedOnSquare(Card.CardTypes.MONSTER).Where(card => ((CreatureCard)card).cardModified == true).ToArray().Length > 0;
    }

    public bool AnySquareModifiers()
    {
        return GetFilteredCardsPlayedOnSquare(Card.CardTypes.SQUARE_MODIFIER).Count > 0;
    }

    private void UpdateAttackAndDefenseGraphics()
    {
        battleSquareAttackGraphic.SetActive(true);
        battleSquareDefenseGraphic.SetActive(true);

        battleSquareAttackGraphic.GetComponentInChildren<Text>().text = CalculateSquarePowerTotals().ToString();

        this.GetComponent<Image>().color = Color.red;

        if (AnyCreatureModifiedOnSquare())
        {
            battleSquareAttackGraphic.GetComponentInChildren<Text>().color = Color.green;
        }

        // TODO: Calculate correct defense total
        battleSquareDefenseGraphic.GetComponentInChildren<Text>().text = 0.ToString();
    }

    public void ResetBattleSquareToDefaultState(bool fullReset)
    {
        battleSquareAttackGraphic.GetComponentInChildren<Text>().text = 0.ToString();
        battleSquareDefenseGraphic.GetComponentInChildren<Text>().text = 0.ToString();

        this.GetComponent<Image>().color = AnySquareModifiers() ? Color.yellow : Color.white;

        if (fullReset)
        {
            cardsPlayedOnObject.Clear();
        }
        else
        {
            cardsPlayedOnObject = GetFilteredCardsPlayedOnSquare(Card.CardTypes.SQUARE_MODIFIER);
        }

        foreach(Card card in GetFilteredCardsPlayedOnSquare(Card.CardTypes.SQUARE_MODIFIER))
        {
            ((SpellCard)card).cardApplied = false;
        }

        squareOccupied = false;
        availablePlacesToMove.Clear();

        battleSquareAttackGraphic.SetActive(false);
        battleSquareDefenseGraphic.SetActive(false);
    }
}
