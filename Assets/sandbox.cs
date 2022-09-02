using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sandbox : HoverableObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(mouseOnObject && Input.GetMouseButtonDown(0))
        {
            // Debug.Log("Horizonal: " + this.gameObject.GetComponent<GridLayoutGroup>().CalculateLayoutInputHorizontal() + "," this.gameObject.GetComponent<GridLayoutGroup>().CalculateLayoutInputVertical()
            int col = this.gameObject.transform.GetSiblingIndex() % 5;
            int row = this.gameObject.transform.GetSiblingIndex() / 5;
            
            Debug.Log("Row: " + row.ToString() + "Col: " + col.ToString());
        } 
    }
}
