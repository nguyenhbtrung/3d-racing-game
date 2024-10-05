using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerWaypointFollower : WaypointFollower
{
    public int waypointCount = 0;

    protected override void OnTriggerEnter(Collider other)
    {
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
