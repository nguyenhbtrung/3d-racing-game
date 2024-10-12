using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButtonAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform selected;
    [SerializeField] private Transform decorate;
    [SerializeField] private TMPro.TextMeshProUGUI optionText;

    public void Select()
    {
        optionText.color = Color.black;
        LeanTween.moveX(selected, 0, 0.1f).setOnComplete(ShowDecorate);
    }
    public void Deselect()
    {
        HideDecorate();
        optionText.color = Color.white;
        LeanTween.moveX(selected, 150, 0.1f);
    }

    private void ShowDecorate()
    {
        decorate.gameObject.SetActive(true);
    }

    private void HideDecorate()
    {
        decorate.gameObject.SetActive(false);
    }


}
