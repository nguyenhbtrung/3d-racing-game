using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static GameData instance;
    public GameObject playerVehicle;
    private string playerName = "Player";
    private bool haveNormalBot = true;

    public static GameData Instance { get => instance; set => instance = value; }
    public GameObject PlayerVehicle { get => playerVehicle; set => playerVehicle = value; }
    public string PlayerName { get => playerName; set => playerName = value; }
    public bool HaveNormalBot { get => haveNormalBot; set => haveNormalBot = value; }

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
