using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform Button;
    
    // Start is called before the first frame update
    void Start()
    {
        Button.GetComponent<Animator>().Play("StartHoverOff");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Button.GetComponent<Animator>().Play("StartHoverOn");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Button.GetComponent<Animator>().Play("StartHoverOff");
    }
}
