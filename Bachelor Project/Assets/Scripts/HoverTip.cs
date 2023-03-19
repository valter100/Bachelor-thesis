using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverTip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tipText;
    [SerializeField] RectTransform tipWindow;

    public static Action<string, Vector2> OnMouseHover;
    public static Action OnMouseLostFocus;

    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLostFocus += HideTip;
    }

    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLostFocus -= HideTip;
    }

    private void Start()
    {
        HideTip();
    }

    public void ShowTip(string tip, Vector2 mousePos)
    {
        tipText.text = tip;

        tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 200 ? 200 : tipText.preferredWidth, tipText.preferredHeight);
        tipWindow.gameObject.SetActive(true);
        tipWindow.transform.position = new Vector2(mousePos.x + tipWindow.sizeDelta.x * 2, mousePos.y);
    }

    public void HideTip()
    {
        tipText.text = default;
        tipWindow.gameObject.SetActive(false);
    }
}
