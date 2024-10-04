using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Waypoint[] neighbours;

    public Waypoint[] Neighbours { get => neighbours; private set => neighbours = value; }

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
