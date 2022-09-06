using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleBoard : UserGraphicController
{
    public List<int> battleSquareIndiciesLit = new List<int>();

    private Color originalColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(userGraphicsUp && Input.GetMouseButtonDown(0))
        {
            CleanLitSquares();
        }
    }

    public void CleanLitSquares()
    {
        foreach(int i in battleSquareIndiciesLit)
        {
            this.transform.GetChild(i).GetComponent<Image>().color = originalColor;
        }
        userGraphicsUp = false;
    }
}
