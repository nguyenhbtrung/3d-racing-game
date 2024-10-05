using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] private GameObject[] racers;
    [SerializeField] private Vector3[] startPositions;
    [SerializeField] private RectTransform speedometerUIParent;
    [SerializeField] private Waypoint startWaypoint;

    private VehicleController playerVehicleController;
    private bool isGameActive = false;

    public static GameManager Instance { get => instance; set => instance = value; }
    public Waypoint StartWaypoint { get => startWaypoint; set => startWaypoint = value; }
    public bool IsGameActive { get => isGameActive; set => isGameActive = value; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;

        int playerStartLane = GameData.Instance.PlayerVehicle.GetComponent<VehicleController>().StartLane;
        racers[playerStartLane] = GameData.Instance.PlayerVehicle;

        for (int i = 0; i < racers.Length; i++)
        {
            Instantiate(racers[i], startPositions[i], Quaternion.Euler(0, 180, 0));
        }
        //Instantiate(GameData.Instance.PlayerVehicle, new Vector3(-1039.9f, 1.83f, 2122.1f), Quaternion.Euler(0, 180, 0));

        playerVehicleController = GameObject.FindGameObjectWithTag("Player").GetComponent<VehicleController>();
        Instantiate(playerVehicleController.SpeedometerUI, speedometerUIParent.position, Quaternion.identity, speedometerUIParent);
    }

    private void Start()
    {
        StartCoroutine(CountDown());
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
}
