using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Waypoint[] neighbours;
    public enum ThrottleStatus
    {
        Release = 0,
        Increase = 1,
        Decrease = -1
    }
    [SerializeField] private ThrottleStatus throttleStatus = ThrottleStatus.Increase;

    public enum RoadType
    {
        OneLane = 1,
        FourLane = 4,
        TwoLane = 2
    }
    [SerializeField] public RoadType roadType = RoadType.FourLane;
    [SerializeField] private float laneWidth;
    [SerializeField] private bool isHandBraking = false;    
    

    public Waypoint[] Neighbours { get => neighbours; private set => neighbours = value; }
    public ThrottleStatus ThrottleStt { get => throttleStatus; set => throttleStatus = value; }
    public bool IsHandBraking { get => isHandBraking; set => isHandBraking = value; }

    public Waypoint GetRandomNeighbour(ref int laneIndex)
    {
        if (neighbours == null)
        {
            return null;
        }
        int i = Random.Range(0, Neighbours.Length);
        laneIndex = Neighbours[i].GetRandomLane();
        return Neighbours[i];
    }

    public int GetRandomLane()
    {
        if (roadType == RoadType.OneLane) return -1;
        int totalLane = (int)roadType;
        return Random.Range(0, totalLane);
    }

    public Vector3 GetTargetPosition(int laneIndex)
    {
        int totalLane = (int)roadType;
        if (laneIndex < 0 || laneIndex >= totalLane)
        {
            return transform.position;
        }
        float offset = laneWidth * (totalLane / 2) - laneWidth * (laneIndex + 0.5f);
        return transform.position + transform.right * offset;
    }
}
