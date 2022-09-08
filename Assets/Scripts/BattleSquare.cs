using System.Collections.Generic;
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

    public BattlePaneStats battlePaneStats;

    public GameObject cardPrefab;
    private GameObject battleSquarePreviewPanel;
    private GameObject battleSquarePreviewContentPane;
    private GameObject battleSquareAttackGraphic;
    private GameObject battleSquareDefenseGraphic;

    private GameObject battlePlaySquare;

    public bool squareOccupied;

    public List<int> availablePlacesToMove = new List<int>();

    void Awake()
    {
        battleSquarePreviewPanel = GameObject.FindGameObjectWithTag(Constants.BATTLE_SQUARE_PREVIEW_PANE_TAG);
        battleSquarePreviewContentPane = GameObject.FindGameObjectWithTag(Constants.BATTLE_SQUARE_PREVIEW_CONTENT_PANE_TAG);

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

        battleSquarePreviewPanel.SetActive(false);
    }

    void Update(){
        
        if(mouseOnObject && Input.GetMouseButtonDown(0)){
            if (gameController.activeObject)
            {
                // If the activeObject is a Card
                if (gameController.activeObject.CompareTag(Constants.CARD_TAG))
                {
                    Card card = gameController.activeObject.GetComponent<Card>();
                    card.PlayCard(this.gameObject);

                    squareOccupied = true;
                    UpdateAttackAndDefenseGraphics();
                    gameController.ResetSelf();
                }
                else if (gameController.activeObject.CompareTag(Constants.BATTLE_SQUARE_ID) && gameController.activeObject.GetComponent<BattleSquare>().availablePlacesToMove.Contains(orderInColumn))
                {
                    foreach(Card card in gameController.activeObject.GetComponent<BattleSquare>().GetCardsPlayedOnSquare())
                    {
                        card.MoveCard(this.gameObject);
                    }
                    squareOccupied = true;
                    UpdateAttackAndDefenseGraphics();
                    
                    // clean the square that the cards came from
                    gameController.activeObject.GetComponent<BattleSquare>().ResetBattleSquareToDefaultState();
                    gameController.ResetSelf();
                    battlePlaySquare.GetComponent<BattleBoard>().ResetSelf();
                }
                
            }
            else
            {
                CreatureCard.MoveDirections[] moves = GetCreatureCardsPlayedOnSquare().SelectMany(x => ((CreatureCard)x).moveDirections).ToArray().Distinct().Cast<CreatureCard.MoveDirections>().ToArray();
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

    public Card[] GetCardsPlayedOnSquare(){
        Card[] cardsPlayedOnSquare = this.gameObject.GetComponents<Card>();
        return cardsPlayedOnSquare;
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

    public Card[] GetCreatureCardsPlayedOnSquare()
    {
        return GetCardsPlayedOnSquare().Where(card => card.CardType == Card.CardTypes.MONSTER).ToArray();
    }

    public int CalculateSquarePowerTotals()
    {
        int total = 0;
        Card[] creatureList = GetCreatureCardsPlayedOnSquare();
        foreach(CreatureCard creatureCard in creatureList)
        {
            total += creatureCard.GetTotalPowerTotal();
        }
        return total;
    }

    public bool IsCreatureOnSquare(){
        return GetCardsPlayedOnSquare().Where(card => card.CardType == Card.CardTypes.MONSTER).ToArray().Length > 0;
    }

    private bool AnyCreatureModifiedOnSquare()
    {
        return GetCreatureCardsPlayedOnSquare().Where(card => ((CreatureCard)card).cardModified == true).ToArray().Length > 0;
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

    public void ResetBattleSquareToDefaultState()
    {
        battleSquareAttackGraphic.GetComponentInChildren<Text>().text = 0.ToString();
        battleSquareDefenseGraphic.GetComponentInChildren<Text>().text = 0.ToString();

        this.GetComponent<Image>().color = Color.white;

        foreach(Card card in GetCardsPlayedOnSquare())
        {
            Destroy(card);
        }

        squareOccupied = false;
        availablePlacesToMove.Clear();

        battleSquareAttackGraphic.SetActive(false);
        battleSquareDefenseGraphic.SetActive(false);
    }
}
