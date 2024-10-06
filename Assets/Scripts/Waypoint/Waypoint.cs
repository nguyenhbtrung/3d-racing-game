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
    [SerializeField] private bool isHandBraking = false;    
    

    public Waypoint[] Neighbours { get => neighbours; private set => neighbours = value; }
    public ThrottleStatus ThrottleStt { get => throttleStatus; set => throttleStatus = value; }
    public bool IsHandBraking { get => isHandBraking; set => isHandBraking = value; }

    public Waypoint GetRandomNeighbour()
    {
        if (neighbours == null)
        {
            return null;
        }
        int i = Random.Range(0, Neighbours.Length);
        return Neighbours[i];
    }
}
