using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string tipToShow;
    [SerializeField] float timeToWait;
    [SerializeField] Vector3 scaleIncrease;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());

        transform.localScale += scaleIncrease;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        HoverTip.OnMouseLostFocus();

        transform.localScale -= scaleIncrease;
    }

    public void ShowMessage()
    {
        HoverTip.OnMouseHover(tipToShow, Input.mousePosition);
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timeToWait);

        ShowMessage();
    }
}
