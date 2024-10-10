using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private Waypoint[] waypoints;
    [SerializeField] private float startTime = 5;
    [SerializeField] private float spawnRate = 3;

    private Transform player;
    private Transform[] botRacers;

    private void Start()
    {
        FindRacers();
        InvokeRepeating(nameof(SpawnBot), startTime, spawnRate);
    }

    private void FindRacers()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject[] botRacerObjs = GameObject.FindGameObjectsWithTag("Racer");
        botRacers = new Transform[botRacerObjs.Length];
        for (int i = 0; i < botRacers.Length; i++)
        {
            botRacers[i] = botRacerObjs[i].transform;
        }
    }

    private void SpawnBot()
    {
        if (!IsSpawnValid())
        {
            return;
        }
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

    private bool IsSpawnValid()
    {
        if (player == null || botRacers == null)
        {
            return true;
        }
        float playerDistance = 250;
        float botRacerDistance = -20;
        if (Vector3.Distance(transform.position, player.position) < playerDistance)
        {
            return false;
        }
        foreach (Transform botRacer in botRacers)
        {
            if (Vector3.Distance(transform.position, botRacer.position) < botRacerDistance)
            {
                return false;
            }
        }
        return true;
    }
}
