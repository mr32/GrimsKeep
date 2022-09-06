using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool objectBeingPlayed = false;
    public GameObject activeObject;
    public GameObject mousePointer;

    public GameObject battleSquareToPlayOn;
    void Awake(){
        mousePointer = GameObject.FindGameObjectWithTag(Constants.MOUSE_POINTER_TAG);
        mousePointer.gameObject.SetActive(false);
    }

    void Update(){
        if (!objectBeingPlayed && activeObject != null){
            activeObject = null;
        }

        if(objectBeingPlayed){
            if(!mousePointer.gameObject.activeSelf){
                mousePointer.gameObject.SetActive(true);
                mousePointer.transform.SetAsLastSibling();
            }
            
            mousePointer.transform.position = Input.mousePosition;
        }

        if(objectBeingPlayed && battleSquareToPlayOn == null && Input.GetMouseButtonDown(0)){
            CleanController();
        }
    }

    public void CleanController(){
        mousePointer.gameObject.SetActive(false);
        objectBeingPlayed = false;
        //activeObject.GetComponent<CardMovement>().DestroyCardPreview();
    }
}
