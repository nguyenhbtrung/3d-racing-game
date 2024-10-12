using System;
using UnityEngine;
using UnityEngine.UI;

public class CountdownAnimation1 : MonoBehaviour
{
    [SerializeField] private Image imageObject; 
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Vector2[] startPositions;

    [Header("Animation 3")]
    [SerializeField] private RectTransform imageFollow;



    public void ShowAnimation1()
    {
        if (sprites.Length >= 3)
        {
            ShowSprite0();
        }
        else if (sprites.Length >= 1)
        {
            Popup();
        }
        
    }

    public void ShowAnimation2()
    {
        if (sprites.Length < 1) return;
        ComeIn();
    }

    public void ShowAnimation3()
    {
        if (sprites.Length < 1) return;
        Popup();
    }

    private void ComeIn()
    {
        imageObject.sprite = sprites[0];
        Vector2 startPos = new(-Screen.width / 2, 0);
        imageObject.rectTransform.anchoredPosition = startPos;
        startPos.y = imageFollow.anchoredPosition.y;
        imageFollow.anchoredPosition = startPos;
        imageFollow.gameObject.SetActive(true);
        LeanTween.moveX(imageObject.rectTransform, 0, 0.5f).setDelay(0.3f).setFrom(-Screen.width / 2).setEase(LeanTweenType.easeInOutQuart).setOnComplete(GoOut);    
        LeanTween.moveX(imageFollow, -160, 0.5f).setDelay(0.4f).setFrom(-Screen.width / 2).setEase(LeanTweenType.easeInOutQuart);
    }

    private void GoOut()
    {
        LeanTween.moveX(imageObject.rectTransform, Screen.width / 2, 0.5f).setDelay(0.5f).setEase(LeanTweenType.easeInOutQuart).setOnComplete(GoOut);
        LeanTween.moveX(imageFollow, Screen.width / 2, 0.5f).setDelay(0.6f).setEase(LeanTweenType.easeInOutQuart).setOnComplete(DeactiveImageFollow);
    }

    private void DeactiveImageFollow()
    {
        imageFollow.gameObject.SetActive(false);
    }

    private void Popup()
    {
        imageObject.sprite = sprites[0];
        LeanTween.scale(imageObject.gameObject, new Vector3(1, 1, 1), 0.15f).setEase(LeanTweenType.easeOutBack).setFrom(Vector3.zero);
    }

    private void ShowSprite0()
    {
        
        var color = imageObject.color;
        color.a = 1;
        imageObject.sprite = sprites[0];
        imageObject.rectTransform.anchoredPosition = startPositions[0];
        LeanTween.alpha(imageObject.rectTransform, 1f, 0.07f).setOnComplete(ShowSprite1); 
    }

    private void ShowSprite1()
    {
        imageObject.sprite = sprites[1];
        imageObject.rectTransform.anchoredPosition = startPositions[1];
        LeanTween.alpha(imageObject.rectTransform, 1f, 0.07f).setOnComplete(ShowSprite2);
    }

    private void ShowSprite2()
    {
       
        imageObject.sprite = sprites[2];
        imageObject.rectTransform.anchoredPosition = startPositions[2]; 

        LeanTween.moveX(imageObject.rectTransform, startPositions[2].x + 20, 0.2f).setDelay(0.5f).setEase(LeanTweenType.easeInOutQuad); 
        LeanTween.alpha(imageObject.rectTransform, 0f, 0.1f).setDelay(0.6f).setOnComplete(ResetSprite); 
    }

    private void ResetSprite()
    {
        imageObject.sprite = null;
    }

    
}
