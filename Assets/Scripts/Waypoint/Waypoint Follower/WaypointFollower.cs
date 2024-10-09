using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private Waypoint targetWaypoint;
    protected int currentLaneIndex = -1;

    public Waypoint TargetWaypoint { get => targetWaypoint; set => targetWaypoint = value; }
    public int CurrentLaneIndex { get => currentLaneIndex; set => currentLaneIndex = value; }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Waypoint>() == TargetWaypoint)
        {
            TargetWaypoint = TargetWaypoint.GetRandomNeighbour(ref currentLaneIndex);
        }
    }

    public int GetThrottleStatus()
    {
        return (int)targetWaypoint.ThrottleStt;
    }

    public bool IsHandBraking()
    {
        return targetWaypoint.IsHandBraking;
    }

    public Vector3 GetTargetPosition()
    {
        return targetWaypoint.GetTargetPosition(CurrentLaneIndex);
    }
}
