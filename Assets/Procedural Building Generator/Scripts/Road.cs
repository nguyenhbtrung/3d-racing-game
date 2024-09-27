using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private bool upNeighbourRoad;
    [SerializeField] private bool downNeighbourRoad;
    [SerializeField] private bool leftNeighbourRoad;
    [SerializeField] private bool rightNeighbourRoad;

    public bool UpNeighbourRoad { get => upNeighbourRoad; set => upNeighbourRoad = value; }
    public bool DownNeighbourRoad { get => downNeighbourRoad; set => downNeighbourRoad = value; }
    public bool LeftNeighbourRoad { get => leftNeighbourRoad; set => leftNeighbourRoad = value; }
    public bool RightNeighbourRoad { get => rightNeighbourRoad; set => rightNeighbourRoad = value; }
}
