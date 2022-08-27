using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool cardBeingPlayed = false;
    public GameObject activeCard;
    public GameObject mousePointer;

    public GameObject battleSquareToPlayOn;
    void Awake(){
        mousePointer = GameObject.FindGameObjectWithTag(Constants.MOUSE_POINTER_TAG);
        mousePointer.gameObject.SetActive(false);
    }

    void Update(){
        if (!cardBeingPlayed && activeCard != null){
            activeCard = null;
        }

        if(cardBeingPlayed){
            if(!mousePointer.gameObject.activeSelf){
                mousePointer.gameObject.SetActive(true);
                mousePointer.transform.SetAsLastSibling();
            }
            
            mousePointer.transform.position = Input.mousePosition;
        }

        if(cardBeingPlayed && battleSquareToPlayOn == null && Input.GetMouseButtonDown(0)){
            CleanController();
        }
    }

    public void CleanController(){
        mousePointer.gameObject.SetActive(false);
        cardBeingPlayed = false;
        activeCard.GetComponent<CardMovement>().DestroyCardPreview();
    }
}
