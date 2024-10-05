using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private Waypoint targetWaypoint;

    public Waypoint TargetWaypoint { get => targetWaypoint; set => targetWaypoint = value; }

    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            TargetWaypoint = GameManager.Instance.StartWaypoint;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Waypoint>() == TargetWaypoint)
        {
            TargetWaypoint = TargetWaypoint.GetRandomNeighbour();
        }
    }
}
