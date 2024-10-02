using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static GameData instance;
    public GameObject playerVehicle;

    public static GameData Instance { get => instance; set => instance = value; }
    public GameObject PlayerVehicle { get => playerVehicle; set => playerVehicle = value; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
