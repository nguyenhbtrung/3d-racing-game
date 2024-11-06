using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [Serializable]
    public struct RacerInfo
    {
        public string name;
        public RacerWaypointFollower waypointFollower;
        public bool isFinished;
        public float time;
    }
    [SerializeField] private RacerInfo[] racerInfos;

    [SerializeField] private GameObject[] racers;
    [SerializeField] private Vector3[] startPositions;
    [SerializeField] private RectTransform speedometerUIParent;
    [SerializeField] private Waypoint startWaypoint;
    [SerializeField] private GameObject mainUI;

    [Header("Leaderboard")]
    [SerializeField] private TMPro.TextMeshProUGUI playerRankText;
    [SerializeField] private TMPro.TextMeshProUGUI playerTimeText;
    [SerializeField] private GameObject rankingSlot;
    [SerializeField] private GameObject rankingPanel;
    [SerializeField] private Transform rankingContent;
    [SerializeField] private Color[] rankingColors;

    [Header("Racer marks")]
    [SerializeField] private GameObject racerMarkPrefab;
    [SerializeField] private Transform racerMarkParent;
    [SerializeField] private int totalRacerWaypoint;

    [Header("Countdown")]
    [SerializeField] private CountdownAnimation1[] countdownAnimations;

    [Header("Spawn bot")]
    [SerializeField] private GameObject spawnSystem;

    private VehicleController playerVehicleController;
    public List<RacerInfo> finishList;
    private bool isGameActive = false;
    private float time = 0;

    public static GameManager Instance { get => instance; set => instance = value; }
    public Waypoint StartWaypoint { get => startWaypoint; set => startWaypoint = value; }
    public bool IsGameActive { get => isGameActive; set => isGameActive = value; }
    public int TotalRacerWaypoint { get => totalRacerWaypoint; private set => totalRacerWaypoint = value; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;

        if (GameData.Instance.HaveNormalBot)
        {
            spawnSystem.SetActive(true);
        }

        int playerStartLane = GameData.Instance.PlayerVehicle.GetComponent<VehicleController>().StartLane;
        racers[playerStartLane] = GameData.Instance.PlayerVehicle;
        racerInfos[playerStartLane].name = GameData.Instance.PlayerName;

        for (int i = 0; i < racers.Length; i++)
        {
            var racer = Instantiate(racers[i], startPositions[i], Quaternion.Euler(0, 180, 0));
            racerInfos[i].waypointFollower = racer.GetComponent<RacerWaypointFollower>();
            racerInfos[i].isFinished = false;
            if (!racer.CompareTag("Player"))
            {
                var racerMark = Instantiate(racerMarkPrefab, racerMarkParent);
                racerMark.GetComponent<RacerMark>().Target = racer.transform;
                racerMark.GetComponentInChildren<TMPro.TextMeshProUGUI>().SetText(racerInfos[i].name);
            }
        }
        //Instantiate(GameData.Instance.PlayerVehicle, new Vector3(-1039.9f, 1.83f, 2122.1f), Quaternion.Euler(0, 180, 0));

        playerVehicleController = GameObject.FindGameObjectWithTag("Player").GetComponent<VehicleController>();
        Instantiate(playerVehicleController.SpeedometerUI, speedometerUIParent.position, Quaternion.identity, speedometerUIParent);
        finishList = new List<RacerInfo>();
    }

    private void Start()
    {
        StartCoroutine(CountDown());
    }

    private void Update()
    {
        SortRacerInfos();
        UpdateplayerRank();
        CalculateRacerTime();
    }

    private void CalculateRacerTime()
    {
        if (!isGameActive)
        {
            return;
        }
        //for (int i = 0; i < racerInfos.Length; i++)
        //{
        //    racerInfos[i].time += Time.deltaTime;
        //    if (racerInfos[i].name == "Player")
        //    {
        //        float time = racerInfos[i].time; 

        //        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        //        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D3}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);

        //        playerTimeText.SetText(formattedTime);
        //    }
        //}
        time += Time.deltaTime;
        string formattedTime = GetFormattedTime(time);

        playerTimeText.SetText(formattedTime);
    }

    private string GetFormattedTime(float time)
    {
        if (time < 0)
        {
            return "--:--:---";
        }
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D3}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        return formattedTime;
    }

    private void SortRacerInfos()
    {
        for (int i = 0; i < racerInfos.Length; i++)
        {
            for (int j = i + 1; j < racerInfos.Length; j++)
            {
                if (IsRankingHigher(racerInfos[i].waypointFollower, racerInfos[j].waypointFollower))
                {
                    continue;
                }
                RacerInfo temp = racerInfos[i];
                racerInfos[i] = racerInfos[j];
                racerInfos[j] = temp;
            }
        }
    }
    private IEnumerator CountDown()
    {
        var readyAnimation = countdownAnimations[countdownAnimations.Length - 1];
        readyAnimation.gameObject.SetActive(true);
        readyAnimation.ShowAnimation2();
        yield return new WaitForSeconds(1.9f);
        readyAnimation.gameObject.SetActive(false);


        int counter = 5;
        while (counter >= 0)
        {
            if (counter >= 0 && counter < countdownAnimations.Length)
            {
                if (counter < countdownAnimations.Length - 1)
                {
                    countdownAnimations[counter + 1].gameObject.SetActive(false);
                }
                countdownAnimations[counter].gameObject.SetActive(true);
                countdownAnimations[counter].ShowAnimation1();
            }
            yield return new WaitForSeconds(1);
            counter--;
        }
        IsGameActive = true;
    }

    private bool IsRankingHigher(RacerWaypointFollower follower1, RacerWaypointFollower follower2)
    {
        Tuple<int, float> distanceTravel1 = follower1.GetRacerDistanceTravel();
        Tuple<int, float> distanceTravel2 = follower2.GetRacerDistanceTravel();

        if (distanceTravel1.Item1 == distanceTravel2.Item1)
        {
            return distanceTravel1.Item2 <= distanceTravel2.Item2;
        }
        return distanceTravel1.Item1 > distanceTravel2.Item1;
    }

    private void UpdateplayerRank()
    {
        int rank = 1;
        for (int i = 0; i < racerInfos.Length; i++)
        {
            if (racerInfos[i].name == "Player")
            {
                rank = i + 1;
                break;
            }
        }
        playerRankText.SetText($"{rank}/{racerInfos.Length}");
    }

    public void AddToFinishList(RacerWaypointFollower waypointFollower)
    {
        for (int i = 0; i < racerInfos.Length; i++)
        {
            if (racerInfos[i].waypointFollower == waypointFollower && !racerInfos[i].isFinished)
            {
                racerInfos[i].isFinished = true;
                racerInfos[i].time = time;
                finishList.Add(racerInfos[i]);
                break;
            }
        }

        if (waypointFollower.CompareTag("Player"))
        {
            for (int i = 0; i < racerInfos.Length; i++)
            {
                if (!racerInfos[i].isFinished)
                {
                    racerInfos[i].isFinished = true;
                    racerInfos[i].time = -1;
                    finishList.Add(racerInfos[i]);
                }
            }
            if (isGameActive)
            {
                ShowRankingPanel();
            }
            IsGameActive = false;
        }
    }

    private void ShowRankingPanel()
    {
        mainUI.SetActive(false);
        rankingPanel.SetActive(true);
        var leaderboardAnimation = rankingPanel.GetComponent<LeaderboardAnimation>();
        int rank = 1;
        foreach (var racer in finishList)
        {
            string name = racer.name;
            GameObject slot = Instantiate(rankingSlot, rankingContent);
            slot.GetComponent<Image>().color = rankingColors[rank % 2];
            TMPro.TextMeshProUGUI rankText = slot.transform.Find("Text Rank").GetComponent<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI nameText = slot.transform.Find("Text Name").GetComponent<TMPro.TextMeshProUGUI>();
            TMPro.TextMeshProUGUI timeText = slot.transform.Find("Text Time").GetComponent<TMPro.TextMeshProUGUI>();
            rankText.SetText(rank.ToString());
            nameText.SetText(name);
            timeText.SetText(GetFormattedTime(racer.time));
            leaderboardAnimation.LeaderboardRows.Add(slot);
            rank++;
        }
        leaderboardAnimation.ShowLeaderboard();
    }
}
