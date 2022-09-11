using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSquarePreview : UserGraphicController
{
    public GameObject battleSquarePreviewPane;
    public GameObject battleSquareContentPane;
    private GameController gameController;
    private void Awake()
    {
        battleSquarePreviewPane = GameObject.FindGameObjectWithTag(Constants.BATTLE_SQUARE_PREVIEW_PANE_TAG);
        battleSquareContentPane = GameObject.FindGameObjectWithTag(Constants.BATTLE_SQUARE_PREVIEW_CONTENT_PANE_TAG);
        gameController = GameObject.FindGameObjectWithTag(Constants.GAME_CONTROLLER_TAG).GetComponent<GameController>();
    }

    private void Start()
    {
        if (battleSquarePreviewPane.activeSelf)
        {
            battleSquarePreviewPane.SetActive(false);
        }
    }

    public override void Update()
    {
        base.Update();

        if(battleSquarePreviewPane.activeSelf && battleSquareContentPane.transform.childCount == 0)
        {
            battleSquarePreviewPane.SetActive(false);
        }

        if (!gameController.activeObject && gameController.battleSquareToPlayOn != null && GameObject.Equals(gameController.battleSquareToPlayOn, this.gameObject) && Input.GetMouseButtonDown(1))
        {
            BattleSquare battleSquare = gameController.battleSquareToPlayOn.GetComponent<BattleSquare>();
            
            if(battleSquare.cardsPlayedOnObject.Count > 0)
            {
                battleSquarePreviewPane.SetActive(true);

                battleSquarePreviewPane.transform.SetAsLastSibling();
                battleSquarePreviewPane.transform.position = new Vector3(
                    battleSquare.gameObject.transform.position.x,
                    battleSquare.gameObject.transform.position.y + 100,
                    battleSquare.gameObject.transform.position.z
                );


                foreach (Card card in battleSquare.cardsPlayedOnObject)
                {
                    GameObject cardMade = Utils.CreateCardGameObject(
                        cardPrefab: battleSquare.cardPrefab,
                        parent: battleSquareContentPane.transform,
                        card: card,
                        scale: new Vector3(1f, 1f, 1f),
                        cardPlayedFrom: card.cardPlayedFrom,
                        cardOwner: card.cardOwner
                    );
                    cardMade.GetComponent<CardMovement>().enabled = false;
                    cardMade.GetComponent<CardGraphics>().SetCardGraphics();
                }
                userGraphicsUp = true;
            }
           
        }
    }
    public override bool ResetCondition()
    {
        return battleSquarePreviewPane.activeSelf;
    }

    public override void ResetSelf()
    {
        foreach (Transform child in battleSquareContentPane.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        battleSquarePreviewPane.SetActive(false);
        base.ResetSelf();
    }
}
