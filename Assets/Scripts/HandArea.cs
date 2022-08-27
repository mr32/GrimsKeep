using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandArea : HoverableObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        print("OnMouseEnter");
    }

    public override void OnPointerExit(PointerEventData eventData){
        print("Mouse Exit");
    }
}
