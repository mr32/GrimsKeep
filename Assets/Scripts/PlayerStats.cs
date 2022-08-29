    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int playerHealth = 20;
    public int playerMana = 5;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.SendMessage(Constants.PLAYER_HEALTH_GRAPHICS_UPDATE_FUNCTION_NAME, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
