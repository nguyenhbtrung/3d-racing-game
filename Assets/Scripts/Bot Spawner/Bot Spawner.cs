using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private Waypoint[] waypoints;
    [SerializeField] private float spawnRate = 3;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnBot), 2, spawnRate);
    }

    private void SpawnBot()
    {

        var bot = VehiclesPool.Instance.GetPooledObject();
        if (bot != null )
        {
            int index = Random.Range(0, waypoints.Length);
            var waypointFollower = bot.GetComponent<WaypointFollower>();
            waypointFollower.TargetWaypoint = waypoints[index];
            waypointFollower.CurrentLaneIndex = waypoints[index].GetRandomLane();
            bot.transform.position = transform.position;
            bot.transform.LookAt(waypoints[index].transform);
        }
    }
}
