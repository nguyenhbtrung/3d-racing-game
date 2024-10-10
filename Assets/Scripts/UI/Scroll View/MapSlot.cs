using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSlot : MonoBehaviour
{
    private string mapName;

    public string MapName { get => mapName; set => mapName = value; }

    public void SelectMap()
    {
        MapLoader.Instance.LoadMap(mapName);
    }
}
