using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerWaypointFollower : WaypointFollower
{
    public int waypointCount = 0;

    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            TargetWaypoint = GameManager.Instance.StartWaypoint;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (this.CompareTag("Player") && other.CompareTag("Player Waypoint"))
        {
            TargetWaypoint = other.GetComponent<Waypoint>().GetRandomNeighbour(ref currentLaneIndex);
            waypointCount++;
        }

        if (other.CompareTag("Finish Line") && waypointCount >= GameManager.Instance.TotalRacerWaypoint)
        {
            GameManager.Instance.AddToFinishList(this);
        }

        if (other.GetComponent<Waypoint>() == TargetWaypoint)
        {
            waypointCount++;
        }
        base.OnTriggerEnter(other);
    }

    public Tuple<int, float> GetRacerDistanceTravel()
    {
        float distanceToTargetWp = Vector3.Distance(transform.position, TargetWaypoint.transform.position);
        return Tuple.Create(waypointCount, distanceToTargetWp);
    }
}
