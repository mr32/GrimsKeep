using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : UserGraphicController
{
    public GameObject activeObject;
    public GameObject mousePointer;

    private GameObject battleBoard;

    public List<string> playersTurn;
    public string currentPlayerTurn;
    public bool cardPlayed;


    public GameObject battleSquareToPlayOn;
    void Awake(){
        mousePointer = GameObject.FindGameObjectWithTag(Constants.MOUSE_POINTER_TAG);
        mousePointer.SetActive(false);

        playersTurn = new List<string>() { Constants.PLAYER1, Constants.ENEMY1 };

        activeObject = null;
        currentPlayerTurn = Constants.PLAYER1;

        battleBoard = GameObject.FindGameObjectWithTag(Constants.BATTLE_BOARD_TAG);

    }

    public override void Update(){
        if(activeObject){
            if(!mousePointer.activeSelf){
                mousePointer.SetActive(true);
                mousePointer.transform.SetAsLastSibling();
            }
            
            mousePointer.transform.position = Input.mousePosition;
        }

        if (cardPlayed)
        {
            TriggerAllOnBoardConditions();
            cardPlayed = false;
        }

        base.Update();
    }

    public void CleanController(){
        if (activeObject && activeObject.GetComponent<CardMovement>())
            activeObject.GetComponent<CardMovement>().DestroyCardPreview();

        activeObject = null;
        mousePointer.SetActive(false);

        
    }

    public void HoldObject(GameObject target)
    {
        if(activeObject == null)
        {
            activeObject = target;
            userGraphicsUp = true;
        }
        
    }
    public override bool ResetCondition()
    {
        return activeObject != null;
    }

    public override void ResetSelf()
    {
        if (activeObject && activeObject.GetComponent<CardMovement>())
            activeObject.GetComponent<CardMovement>().DestroyCardPreview();

        activeObject = null;
        mousePointer.SetActive(false);

        base.ResetSelf();
    }

    private void TriggerAllOnBoardConditions()
    {
        foreach (BattleSquare battleSquare in battleBoard.GetComponentsInChildren<BattleSquare>())
        {
            if (battleSquare.cardsPlayedOnObject.Count > 0)
            {
                foreach (Card c in battleSquare.cardsPlayedOnObject)
                {
                    c.OnBoardConditions();
                }
            }

        }
    }
}
