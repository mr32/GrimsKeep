using System.Collections;
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

    public int row;
    public int col;

    private CardInfo currentCard;
    public BattlePaneStats battlePaneStats;

    public GameObject cardPrefab;
    private GameObject battleSquarePreviewPanel;
    private GameObject battleSquarePreviewContentPane;
    private GameObject battleSquareAttackGraphic;
    private GameObject battleSquareDefenseGraphic;

    private GameObject battlePlaySquare;
    private Color originalColor;
    private bool battleSquareClicked;
    private List<int> moveSquareIndiciesColored = new List<int>();

    void Awake()
    {
        battleSquarePreviewPanel = GameObject.FindGameObjectWithTag(Constants.BATTLE_SQUARE_PREVIEW_PANE_TAG);
        battleSquarePreviewContentPane = GameObject.FindGameObjectWithTag(Constants.BATTLE_SQUARE_PREVIEW_CONTENT_PANE_TAG);
        originalColor = this.GetComponent<Image>().color;

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
            if (gameController.objectBeingPlayed)
            {
                if (gameController.activeObject.CompareTag(Constants.BATTLE_SQUARE_ID))
                {
                    // 1. Check if its within the bounds of the movement

                    // 2. Copy all CardInfo over
                    foreach(CardInfo card in GetCardsPlayedOnSquare())
                    {
                        card.cardCopied = true;
                        UnityEditorInternal.ComponentUtility.CopyComponent(card);
                        UnityEditorInternal.ComponentUtility.PasteComponentAsNew(gameController.battleSquareToPlayOn);

                        card.enabled = false;
                    }

                    // 3. Clean current square
                    this.gameObject.GetComponent<Image>().color = Color.white;
                    UpdateAttackAndDefenseGraphics();
                    gameController.activeObject.GetComponent<BattleSquare>().UpdateAttackAndDefenseGraphics();
                    gameController.CleanController();
                    currentCard = null;

                }
                else
                {
                    currentCard = gameController.activeObject.GetComponent<CardInfo>();

                    if (currentCard.CanPlayCardOnObject(this.gameObject))
                    {
                        this.gameObject.GetComponent<Image>().color = Color.red;
                        PlayCardOnSquare();
                        UpdateAttackAndDefenseGraphics();
                    }
                }
                
            }
            else if (!battlePlaySquare.GetComponent<BattleBoard>().userGraphicsUp)
            {   
                HashSet<CreatureCard.MoveDirections> moveDirections = new HashSet<CreatureCard.MoveDirections>();
                foreach (CreatureCard creatureCard in GetCreatureCardsPlayedOnSquare())
                {
                    foreach (var val in creatureCard.moveDirections)
                    {
                        moveDirections.Add(val);
                    }
                }
                LightUpMoveSquares(moveDirections.ToArray());
                gameController.activeObject = this.gameObject;
                gameController.objectBeingPlayed = true;
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

    public void PlayCardOnSquare(){
        currentCard = gameController.activeObject.GetComponent<CardInfo>();
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
                        // Should probably add this logic to the overall board (parent of this)
                        moveSquareIndiciesColored.Add(CalculateSiblingIndex(row - 1, col));
                    }
                    break;
                case CreatureCard.MoveDirections.DOWN:
                    if (IsValidBoardSquare(row + 1, col))
                    {
                        ColorSquare(CalculateSiblingIndex(row + 1, col), Color.green);
                        moveSquareIndiciesColored.Add(CalculateSiblingIndex(row + 1, col));
                    }
                    break;
                case CreatureCard.MoveDirections.LEFT:
                    if (IsValidBoardSquare(row, col - 1))
                    {
                        ColorSquare(CalculateSiblingIndex(row, col - 1), Color.green);
                        moveSquareIndiciesColored.Add(CalculateSiblingIndex(row, col - 1));
                    }
                    break;
                case CreatureCard.MoveDirections.RIGHT:
                    if (IsValidBoardSquare(row, col + 1))
                    {
                        ColorSquare(CalculateSiblingIndex(row, col + 1), Color.green);
                        moveSquareIndiciesColored.Add(CalculateSiblingIndex(row, col + 1));
                    }
                    break;
                case CreatureCard.MoveDirections.TOP_LEFT:
                    if (IsValidBoardSquare(row - 1, col - 1))
                    {
                        ColorSquare(CalculateSiblingIndex(row - 1, col - 1), Color.green);
                        moveSquareIndiciesColored.Add(CalculateSiblingIndex(row - 1, col - 1));
                    }
                    break;
                case CreatureCard.MoveDirections.TOP_RIGHT:
                    if (IsValidBoardSquare(row - 1, col + 1))
                    {
                        ColorSquare(CalculateSiblingIndex(row - 1, col + 1), Color.green);
                        moveSquareIndiciesColored.Add(CalculateSiblingIndex(row - 1, col + 1));
                    }
                    break;
                case CreatureCard.MoveDirections.BOTTOM_LEFT:
                    if (IsValidBoardSquare(row + 1, col - 1))
                    {
                        ColorSquare(CalculateSiblingIndex(row + 1, col - 1), Color.green);
                        moveSquareIndiciesColored.Add(CalculateSiblingIndex(row + 1, col - 1));
                    }
                    break;
                case CreatureCard.MoveDirections.BOTTOM_RIGHT:
                    if (IsValidBoardSquare(row + 1, col + 1))
                    {
                        ColorSquare(CalculateSiblingIndex(row + 1, col + 1), Color.green);
                        moveSquareIndiciesColored.Add(CalculateSiblingIndex(row + 1, col + 1));
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void ColorSquare(int siblingIndex, Color color)
    {
        Transform battleSquare = battlePlaySquare.transform.GetChild(siblingIndex);

        if (battleSquare)
        {
            battleSquare.GetComponent<Image>().color = color;
            battlePlaySquare.GetComponent<BattleBoard>().battleSquareIndiciesLit.Add(siblingIndex);
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
        return GetCreatureCardsPlayedOnSquare().Where(cardType => cardType.cardModified == true).ToArray().Length > 0;
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
