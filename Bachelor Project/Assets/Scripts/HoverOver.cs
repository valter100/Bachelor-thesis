using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string tipToShow;
    [SerializeField] float timeToWait;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        HoverTip.OnMouseLostFocus();
    }

    public void ShowMessage()
    {
        HoverTip.OnMouseHover(tipToShow, Input.mousePosition, GetComponent<RectTransform>().rect.size.x);
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timeToWait);

        ShowMessage();
    }
}
