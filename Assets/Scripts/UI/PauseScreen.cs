using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private RectTransform panelOptionsRect;
    [SerializeField] private RectTransform trackNameRect;
    [SerializeField] private RectTransform raceGoalRect;

    public void Pause()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
        LeanTween.moveX(panelOptionsRect, 340.0f, 0.2f).setIgnoreTimeScale(true);
        LeanTween.moveX(trackNameRect, 147.3f, 0.2f).setDelay(0.05f).setIgnoreTimeScale(true);
        LeanTween.moveX(raceGoalRect, -137.0f, 0.2f).setDelay(0.1f).setIgnoreTimeScale(true);
    }

    public void Resume(System.Action onComplete)
    {
        LeanTween.moveX(panelOptionsRect, 624.0f, 0.2f).setIgnoreTimeScale(true);
        LeanTween.moveX(trackNameRect, -166.0f, 0.2f).setDelay(0.05f).setIgnoreTimeScale(true);
        LeanTween.moveX(raceGoalRect, -637.2f, 0.2f).setDelay(0.1f).setIgnoreTimeScale(true).setOnComplete(() =>
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
            onComplete();
        });
    }
}
