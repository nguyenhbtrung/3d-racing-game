using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;

public class LeaderboardAnimation : MonoBehaviour
{
    public TMPro.TextMeshProUGUI leaderboardTitle;
    private List<GameObject> leaderboardRows = new();

    public List<GameObject> LeaderboardRows { get => leaderboardRows; set => leaderboardRows = value; }

    public void ShowLeaderboard()
    {

        transform.position = new Vector3(gameObject.transform.position.x, Screen.height * 1.5f, gameObject.transform.position.z);
        gameObject.SetActive(true);

        LeanTween.moveY(gameObject, Screen.height / 2, 0.7f).setEase(LeanTweenType.easeInOutQuart).setOnComplete(ShowTitle);
    }

    void ShowTitle()
    {

        LeanTween.scale(leaderboardTitle.gameObject, new Vector3(1, 1, 1), 0.5f).setEase(LeanTweenType.easeOutBack).setFrom(Vector3.zero).setOnComplete(ShowRows);
    }

    void ShowRows()
    {

        for (int i = 0; i < LeaderboardRows.Count; i++)
        {
            float delay = i * 0.1f;
            LeanTween.rotateX(LeaderboardRows[i], 0, 0.5f).setEase(LeanTweenType.easeOutBack).setFrom(90).setDelay(delay);
        }
    }
}
