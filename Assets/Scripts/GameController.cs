using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
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

    void Update(){
        if(activeObject){
            if(!mousePointer.activeSelf){
                mousePointer.SetActive(true);
                mousePointer.transform.SetAsLastSibling();
            }
            
            mousePointer.transform.position = Input.mousePosition;
        }

        if(battleSquareToPlayOn == null && Input.GetMouseButtonDown(0)){
            CleanController();
        }
    }

    public void CleanController(){
        activeObject = null;
        mousePointer.SetActive(false);
        //activeObject.GetComponent<CardMovement>().DestroyCardPreview();
    }
}
