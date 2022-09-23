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

    public GameObject cardPrefab;
    private GameObject battleSquareAttackGraphic;
    private GameObject battleSquareDefenseGraphic;
    private GameObject battleSquareHealthGraphic;

    private GameManager gameManager;

    private GameObject battlePlaySquare;

    public bool objectPlayed = false;

    public bool squareOccupied;

    public List<int> availablePlacesToMove = new List<int>();

    public List<Card> cardsPlayedOnObject = new List<Card>();

    public Color currentColor;

    void Awake()
    {
        battlePlaySquare = this.gameObject.transform.parent.gameObject;

        
        battleSquareAttackGraphic = Utils.FindChildWithTag(this.gameObject, Constants.BATTLE_SQUARE_ATTACK_GRAPHIC_TAG);
        battleSquareDefenseGraphic = Utils.FindChildWithTag(this.gameObject, Constants.BATTLE_SQUARE_DEFENSE_GRAPHIC_TAG);
        battleSquareHealthGraphic = Utils.FindChildWithTag(this.gameObject, Constants.BATTLE_SQUARE_HEALTH_GRAPHIC_TAG);

        battleSquareAttackGraphic.SetActive(false);
        battleSquareDefenseGraphic.SetActive(false);
        battleSquareHealthGraphic.SetActive(false);

        gameController = GameObject.FindGameObjectWithTag(Constants.GAME_CONTROLLER_TAG).GetComponent<GameController>();
        gameManager = GameObject.FindGameObjectWithTag(Constants.GAME_MANAGER_TAG).GetComponent<GameManager>();
        orderInColumn = this.transform.GetSiblingIndex();

        (row, col) = Utils.GetRowAndColIndex(orderInColumn);
    }

    void Update(){
        
        if(mouseOnObject && Input.GetMouseButtonDown(0)){
            if (gameController.activeObject)
            {
                // If activeObject is a Card
                if (gameController.activeObject.CompareTag(Constants.CARD_TAG) && gameController.activeObject.GetComponent<CardInfo>().card.CanPlayCardOnTarget(this.gameObject))
                {
                    Card card = gameController.activeObject.GetComponent<CardInfo>().card;
                    card.PlayCard(this.gameObject);

                    gameController.ResetSelf();
                }
                // If activeObject is a BattleSquare
                else if (gameController.activeObject.CompareTag(Constants.BATTLE_SQUARE_ID) && gameController.activeObject.GetComponent<BattleSquare>().availablePlacesToMove.Contains(orderInColumn))
                {
                    // If this BattleSquare is Vacant
                    if(!IsCreatureOnSquare() && !IsObstacleOnSquare())
                    {
                        // Play Card
                        foreach (Card card in gameController.activeObject.GetComponent<BattleSquare>().GetMovableCardsPlayedOnSquare())
                        {
                            card.PlayCard(this.gameObject);
                        }

                        // Clean square card came from
                        gameController.activeObject.GetComponent<BattleSquare>().ResetBattleSquareToDefaultState(false);
                    }
                    else if(HasAnyAttackableCards())
                    {
                        // Attack the creatures on this square
                        foreach(CreatureCard card in gameController.activeObject.GetComponent<BattleSquare>().GetCreatureCardsPlayedOnSquare())
                        {
                            List<Card> allBoardTargets = GetCreatureCardsPlayedOnSquare().Concat(GetObstacleCardsPlayedOnSquare()).ToList();
                            foreach (BoardTarget squareBoardTarget in allBoardTargets)
                            {
                                card.AttackBoardTarget(squareBoardTarget);
                            }
                        }
                    }
                    gameController.ResetSelf();
                    battlePlaySquare.GetComponent<BattleBoard>().ResetSelf();
                }   

                if (objectPlayed)
                {
                    UpdateAttackAndDefenseGraphics();
                    objectPlayed = false;
                    cardsPlayedOnObject = cardsPlayedOnObject.OrderBy(card => (int)(card.CardType)).ToList();
                    gameManager.cardPlayed = true;
                }                
            }
            else
            {
                CreatureCard.MoveDirections[] moves = GetCreatureCardsPlayedOnSquare().SelectMany(x => ((CreatureCard)x).moveDirections).ToArray().Distinct().Cast<CreatureCard.MoveDirections>().ToArray();
                LightUpMoveSquares(moves);
                if(GetSquareOwner() == Card.PlayTypes.SELF)
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

    
    // Square Calculations
    public int CalculateSquarePowerTotals()
    {
        int total = 0;
        List<Card> creatureList = GetAllBoardTargetsOnSquare();
        foreach(BoardTarget creatureCard in creatureList)
        {
            total += creatureCard.GetTotalPowerTotal();
        }
        return total;
    }

    public int CalculateSquareHealthTotals()
    {
        int total = 0;
        foreach(BoardTarget creatureCard in GetAllBoardTargetsOnSquare())
        {
            total += creatureCard.GetTotalCurrentCreatureHP();
        }
        return total;
    }

    public int CalculateSquareDefenseTotals()
    {
        int total = 0;
        foreach(BoardTarget creatureCard in GetAllBoardTargetsOnSquare())
        {
            total += creatureCard.GetTotalCurrentDefenseCreatureHP();
        }
        return total;
    }
    // End Square Calculations
    

    

    // Board Graphics
    public void ResetBattleSquareToDefaultState(bool fullReset)
    {
        battleSquareAttackGraphic.GetComponentInChildren<Text>().text = 0.ToString();
        battleSquareDefenseGraphic.GetComponentInChildren<Text>().text = 0.ToString();
        battleSquareHealthGraphic.GetComponentInChildren<Text>().text = 0.ToString();
        battleSquareAttackGraphic.GetComponentInChildren<Text>().color = Color.white;

        if (fullReset)
        {
            cardsPlayedOnObject.Clear();
        }
        else
        {
            cardsPlayedOnObject = GetCardsPlayedByType(Card.CardTypes.SQUARE_MODIFIER);
        }

        this.GetComponent<Image>().color = AnySquareModifiers() ? Color.yellow : Color.white;


        squareOccupied = false;
        availablePlacesToMove.Clear();

        EnableOrDisableSquareGraphics(false);
    }

    public void UpdateAttackAndDefenseGraphics()
    {
        EnableOrDisableSquareGraphics(true);

        battleSquareAttackGraphic.GetComponentInChildren<Text>().text = CalculateSquarePowerTotals().ToString();

        this.GetComponent<Image>().color = HasAnyAttackableCards() ? Color.red : Color.black;

        if (IsCommanderOnSquare())
        {
            this.GetComponent<Image>().color = Color.magenta;
        }

        if (AnyCreatureModifiedOnSquare())
        {
            battleSquareAttackGraphic.GetComponentInChildren<Text>().color = Color.green;
        }

        int defenseTotal = CalculateSquareDefenseTotals();
        if (defenseTotal == 0)
        {
            battleSquareDefenseGraphic.SetActive(false);
        }
        battleSquareDefenseGraphic.GetComponentInChildren<Text>().text = CalculateSquareDefenseTotals().ToString();
        battleSquareHealthGraphic.GetComponentInChildren<Text>().text = CalculateSquareHealthTotals().ToString();
        currentColor = this.GetComponent<Image>().color;
    }

    private void EnableOrDisableSquareGraphics(bool show)
    {
        battleSquareAttackGraphic.SetActive(show);
        battleSquareDefenseGraphic.SetActive(show);
        battleSquareHealthGraphic.SetActive(show);
    }

    private void LightUpMoveSquares(CreatureCard.MoveDirections[] possibleMoveDirections)
    {
        foreach (CreatureCard.MoveDirections move in possibleMoveDirections)
        {
            switch (move)
            {
                case CreatureCard.MoveDirections.UP:
                    if (IsValidBoardSquare(row - 1, col))
                    {
                        ColorSquare(Utils.CalculateSiblingIndex(row - 1, col), Color.green);
                    }
                    break;
                case CreatureCard.MoveDirections.DOWN:
                    if (IsValidBoardSquare(row + 1, col))
                    {
                        ColorSquare(Utils.CalculateSiblingIndex(row + 1, col), Color.green);
                    }
                    break;
                case CreatureCard.MoveDirections.LEFT:
                    if (IsValidBoardSquare(row, col - 1))
                    {
                        ColorSquare(Utils.CalculateSiblingIndex(row, col - 1), Color.green);
                    }
                    break;
                case CreatureCard.MoveDirections.RIGHT:
                    if (IsValidBoardSquare(row, col + 1))
                    {
                        ColorSquare(Utils.CalculateSiblingIndex(row, col + 1), Color.green);
                    }
                    break;
                case CreatureCard.MoveDirections.TOP_LEFT:
                    if (IsValidBoardSquare(row - 1, col - 1))
                    {
                        ColorSquare(Utils.CalculateSiblingIndex(row - 1, col - 1), Color.green);
                    }
                    break;
                case CreatureCard.MoveDirections.TOP_RIGHT:
                    if (IsValidBoardSquare(row - 1, col + 1))
                    {
                        ColorSquare(Utils.CalculateSiblingIndex(row - 1, col + 1), Color.green);
                    }
                    break;
                case CreatureCard.MoveDirections.BOTTOM_LEFT:
                    if (IsValidBoardSquare(row + 1, col - 1))
                    {
                        ColorSquare(Utils.CalculateSiblingIndex(row + 1, col - 1), Color.green);
                    }
                    break;
                case CreatureCard.MoveDirections.BOTTOM_RIGHT:
                    if (IsValidBoardSquare(row + 1, col + 1))
                    {
                        ColorSquare(Utils.CalculateSiblingIndex(row + 1, col + 1), Color.green);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void AddToLitSquaresIndex(int siblingIndex)
    {
        battlePlaySquare.GetComponent<BattleBoard>().battleSquareIndiciesLit.Add(siblingIndex);
        availablePlacesToMove.Add(siblingIndex);
    }

    private void ColorSquare(int siblingIndex, Color color)
    {
        Transform battleSquare = battlePlaySquare.transform.GetChild(siblingIndex);

        if (battleSquare && (!battleSquare.GetComponent<BattleSquare>().AnythingOnSquare() || (battleSquare.GetComponent<BattleSquare>().GetSquareOwner() == Card.PlayTypes.ENEMY || battleSquare.GetComponent<BattleSquare>().GetSquareOwner() == Card.PlayTypes.NEUTRAL)))
        {
            battleSquare.GetComponent<Image>().color = color;
            AddToLitSquaresIndex(siblingIndex);
            battlePlaySquare.GetComponent<BattleBoard>().userGraphicsUp = true;
        }
    }
    // End Board Graphics

    // Card Filtering
    private Card.PlayTypes GetSquareOwner()
    {
        CreatureCard c = cardsPlayedOnObject.SingleOrDefault(card => card is CreatureCard) as CreatureCard;

        if (c == null)
        {
            return Card.PlayTypes.NEUTRAL;
        }

        return c.cardOwner;
    }

    public List<Card> GetCreatureCardsPlayedOnSquare()
    {
        return cardsPlayedOnObject.Where(card => card is CreatureCard).ToList();
    }

    public List<Card> GetObstacleCardsPlayedOnSquare()
    {
        return cardsPlayedOnObject.Where(card => card is ObstacleCard).ToList();
    }

    public List<Card> GetAllBoardTargetsOnSquare()
    {
        return cardsPlayedOnObject.Where(card => card is BoardTarget).ToList();
    }

    public List<Card> GetMovableCardsPlayedOnSquare()
    {
        return cardsPlayedOnObject.Where(card => card.CardType != Card.CardTypes.SQUARE_MODIFIER).ToList();
    }

    public List<Card> GetCardsPlayedByType(Card.CardTypes cardType)
    {
        return cardsPlayedOnObject.Where(card => card.CardType == cardType).ToList();
    }
    public CommanderCard GetCommanderCard()
    {
        return cardsPlayedOnObject.Single(card => card is CommanderCard) as CommanderCard;
    }
    // End Card Filtering


    // Board Checks
    public bool IsCommanderOnSquare()
    {
        return cardsPlayedOnObject.Where(card => card is CommanderCard).ToList().Count > 0;
    }
    public bool IsCreatureOnSquare()
    {
        return GetCreatureCardsPlayedOnSquare().Count > 0;
    }
    public bool IsObstacleOnSquare()
    {
        return cardsPlayedOnObject.Where(card => card is ObstacleCard).ToList().Count > 0;
    }
    public bool AnythingOnSquare()
    {
        return GetAllBoardTargetsOnSquare().Count > 0;
    }

    private bool AnyCreatureModifiedOnSquare()
    {
        return GetAllBoardTargetsOnSquare().Where(card => card is CreatureCard && ((CreatureCard)card).cardModified == true).ToList().Count > 0;
    }

    //private bool AnyObstacleModifiedOnSquare()
    //{
    //    return GetAllBoardTargetsOnSquare().Where(card => card is ObstacleCard && ((ObstacleCard)card).cardModified == true).ToList().Count > 0;
    //}

    public bool AnySquareModifiers()
    {
        return GetCardsPlayedByType(Card.CardTypes.SQUARE_MODIFIER).Count > 0;
    }

    public bool HasAnyAttackableCards()
    {
        return GetAllBoardTargetsOnSquare().Where(card => card.cardOwner == Card.PlayTypes.ENEMY || card.cardOwner == Card.PlayTypes.NEUTRAL).ToList().Count > 0;
    }

    public bool IsSquareOccupied()
    {
        return GetAllBoardTargetsOnSquare().Count() > 0;
    }

    private bool IsValidBoardSquare(int row, int col)
    {
        if (row >= 0 && col >= 0 && row < Constants.BOARD_WIDTH && col < Constants.BOARD_WIDTH)
        {
            return true;
        }
        return false;
    }
    // End Board Checks
}
