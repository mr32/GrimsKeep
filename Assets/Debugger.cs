using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debugger : MonoBehaviour
{
    public BattleBoard battleBoard;
    public Transform playerHand;
    public DeckLoader deckLoader;
    public PlayerStats playerStats;
    public Button buttonToClick;
    public GameManager gameManager;

    public void Awake()
    {
        battleBoard = GameObject.FindGameObjectWithTag(Constants.BATTLE_BOARD_TAG).GetComponent<BattleBoard>();
        deckLoader = GameObject.FindGameObjectWithTag(Constants.DECK_LOADER_TAG).GetComponent<DeckLoader>();
        playerHand = GameObject.FindGameObjectWithTag(Constants.HAND_AREA_TAG).transform;
        playerStats = GameObject.FindGameObjectWithTag(Constants.PLAYER_STAT_GAMEOBJECT_TAG).GetComponent<PlayerStats>();
        gameManager = GameObject.FindGameObjectWithTag(Constants.GAME_MANAGER_TAG).GetComponent<GameManager>();

        buttonToClick = this.GetComponent<Button>();
    }
}
    