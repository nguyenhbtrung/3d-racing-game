using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [Serializable]
    public struct RacerInfo
    {
        public string name;
        public RacerWaypointFollower waypointFollower;
        public bool isFinished;
    }
    [SerializeField] private RacerInfo[] racerInfos;

    [SerializeField] private GameObject[] racers;
    [SerializeField] private Vector3[] startPositions;
    [SerializeField] private RectTransform speedometerUIParent;
    [SerializeField] private Waypoint startWaypoint;
    [SerializeField] private TMPro.TextMeshProUGUI playerRankText;

    [SerializeField] private int totalRacerWaypoint;

    private VehicleController playerVehicleController;
    public List<RacerInfo> finishList;
    private bool isGameActive = false;

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

        int playerStartLane = GameData.Instance.PlayerVehicle.GetComponent<VehicleController>().StartLane;
        racers[playerStartLane] = GameData.Instance.PlayerVehicle;
        racerInfos[playerStartLane].name = GameData.Instance.PlayerName;

        for (int i = 0; i < racers.Length; i++)
        {
            var racer = Instantiate(racers[i], startPositions[i], Quaternion.Euler(0, 180, 0));
            racerInfos[i].waypointFollower = racer.GetComponent<RacerWaypointFollower>();
            racerInfos[i].isFinished = false;
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
        int counter = 4;
        while (counter > 0)
        {
            Debug.Log(counter);
            yield return new WaitForSeconds(1);
            counter--;
        }
        Debug.Log("Start!!");
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
                    finishList.Add(racerInfos[i]);
                }
            }
            IsGameActive = false;
        }
    }

}
