using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string tipToShow;
    [SerializeField] float timeToWait;
    Vector3 startScale;
    [SerializeField] float hoverScaleIncrease;
    [SerializeField] float scaleSpeed;

    private void Start()
    {
        startScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();

        StartCoroutine(HoverScaleLerp(transform.localScale, startScale * (hoverScaleIncrease / 100 + 1), scaleSpeed));

        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();

        StartCoroutine(HoverScaleLerp(transform.localScale, startScale, scaleSpeed));

        if (tipToShow != "")
            HoverTip.OnMouseLostFocus();
    }

    public void ShowMessage()
    {
        if (tipToShow != "")
            HoverTip.OnMouseHover(tipToShow, Input.mousePosition, GetComponent<RectTransform>().rect.size.x);
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timeToWait);

        ShowMessage();
    }

    IEnumerator HoverScaleLerp(Vector3 startScale, Vector3 endScale, float speed)
    {
        float progress = 0;

        while (progress < 1)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, progress);

            if (progress > 0.95)
            {
                progress = 1;
                transform.localScale = endScale;
            }

            progress += Time.deltaTime * speed;
            yield return null;
        }

        yield return 0;
    }
}
