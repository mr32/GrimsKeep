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

        if (!gameController.activeObject && gameController.battleSquareToPlayOn != null && GameObject.Equals(gameController.battleSquareToPlayOn, this.gameObject) && Input.GetMouseButtonDown(1))
        {
            BattleSquare battleSquare = gameController.battleSquareToPlayOn.GetComponent<BattleSquare>();
            battleSquarePreviewPane.SetActive(true);

            battleSquarePreviewPane.transform.SetAsLastSibling();
            battleSquarePreviewPane.transform.position = new Vector3(
                battleSquare.gameObject.transform.position.x,
                battleSquare.gameObject.transform.position.y + 100,
                battleSquare.gameObject.transform.position.z
            );


            foreach (Card card in battleSquare.GetCardsPlayedOnSquare())
            {
                GameObject cardPrefab = Instantiate(battleSquare.cardPrefab);

                cardPrefab.transform.SetParent(battleSquareContentPane.transform);
                cardPrefab.transform.localScale = new Vector3(1f, 1f, 1f);
                cardPrefab.GetComponent<CardMovement>().enabled = false;

                cardPrefab.AddComponent(card.GetType());
            }
            userGraphicsUp = true;
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
