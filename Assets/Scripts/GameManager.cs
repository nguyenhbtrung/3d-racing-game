using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] racers;
    [SerializeField] private Vector3[] startPositions;
    [SerializeField] private RectTransform speedometerUIParent;
    private VehicleController playerVehicleController;

    private void Awake()
    {
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
}
