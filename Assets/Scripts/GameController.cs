using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : UserGraphicController
{
    public GameObject activeObject;
    public GameObject mousePointer;

    private GameObject cardPreviewArea;

    public GameObject battleSquareToPlayOn;
    void Awake(){
        mousePointer = GameObject.FindGameObjectWithTag(Constants.MOUSE_POINTER_TAG);
        mousePointer.SetActive(false);

        cardPreviewArea = GameObject.FindGameObjectWithTag(Constants.CARD_PREVIEW_TAG);

        activeObject = null;
    }

    public override void Update(){
        if(activeObject){
            if(!mousePointer.activeSelf){
                mousePointer.SetActive(true);
                mousePointer.transform.SetAsLastSibling();
            }
            
            mousePointer.transform.position = Input.mousePosition;
        }

        base.Update();
    }

    public void CleanController(){
        if (activeObject && activeObject.GetComponent<CardMovement>())
            activeObject.GetComponent<CardMovement>().DestroyCardPreview();

        activeObject = null;
        mousePointer.SetActive(false);

        
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
}
