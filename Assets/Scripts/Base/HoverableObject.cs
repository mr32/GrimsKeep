using UnityEngine.EventSystems;
using UnityEngine;

public abstract class HoverableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected bool mouseOnObject;
    public virtual void OnPointerEnter(PointerEventData eventData){
        mouseOnObject = true;
    }
    public virtual void OnPointerExit(PointerEventData eventData){
        mouseOnObject = false;
    }
}
