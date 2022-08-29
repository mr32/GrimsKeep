using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsGraphics : MonoBehaviour
{
    private Text healthText;
    private Text manaText;
    // Start is called before the first frame update
    void Start()
    {
        healthText = GameObject.FindGameObjectWithTag(Constants.PLAYER_HEALTH_TAG).GetComponentInChildren<Text>();
        manaText = GameObject.FindGameObjectWithTag(Constants.PLAYER_MANA_TAG).GetComponentInChildren<Text>();
    }

    public void UpdatePlayerStatsGraphics(PlayerStats playerStats)
    {
        healthText.text = playerStats.playerHealth.ToString();
        manaText.text = playerStats.playerMana.ToString();
    }
}
