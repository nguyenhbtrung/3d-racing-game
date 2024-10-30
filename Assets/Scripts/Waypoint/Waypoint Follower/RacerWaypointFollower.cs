using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerWaypointFollower : WaypointFollower
{
    private Waypoint lastWaypoint;
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

        if (this.CompareTag("Player"))
        {
            if (other.CompareTag("Root Waypoint"))
            {
                other.GetComponent<RootWaypoint>().CalculateBranchesWaypointCount(waypointCount + 1);
            }
            if (other.CompareTag("Branch Waypoint"))
            {
                var branchWaypoint = other.GetComponent<BranchWaypoint>();
                TargetWaypoint = branchWaypoint.GetRandomNeighbour(ref currentLaneIndex);
                waypointCount = branchWaypoint.WaypointCount;
            }
        }

        if (other.CompareTag("Finish Line") && waypointCount >= GameManager.Instance.TotalRacerWaypoint)
        {
            GameManager.Instance.AddToFinishList(this);
        }

        if (other.GetComponent<Waypoint>() == TargetWaypoint)
        {
            lastWaypoint = TargetWaypoint;
            waypointCount++;
        }
        base.OnTriggerEnter(other);
    }

    public Tuple<int, float> GetRacerDistanceTravel()
    {
        float distanceToTargetWp = Vector3.Distance(transform.position, TargetWaypoint.transform.position);
        return Tuple.Create(waypointCount, distanceToTargetWp);
    }

    public void ReturnTrack()
    {
        if (lastWaypoint == null)
        {
            return;
        }
        Vector3 newPosition = lastWaypoint.transform.position;
        newPosition.y = 2.0f;
        transform.position = newPosition;
        transform.LookAt(TargetWaypoint.transform.position);

        Camera.main.transform.GetComponent<CameraController>().MoveToConstraintPosition();
    }
}
