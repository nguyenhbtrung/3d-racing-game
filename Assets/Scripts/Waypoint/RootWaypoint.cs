using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootWaypoint : Waypoint
{
    private bool isCalculated = false;

    public void CalculateBranchesWaypointCount(int waypointCount)
    {
        if (isCalculated) return;
        foreach (var waypoint in Neighbours)
        {
            if (waypoint is not BranchWaypoint)
            {
                continue;
            }
            var branch = (BranchWaypoint)waypoint;
            branch.WaypointCount = waypointCount;
        }
        isCalculated = true;
    }

}
