using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginBotSpawner : MonoBehaviour
{
    [Serializable]
    internal struct BeginBot
    {
        public Transform spawnPositions;
        public Waypoint targetWaypoint;
    }
    [SerializeField] private BeginBot[] beginBots;

    private void Start()
    {
        Invoke(nameof(SpawnBeginBot), 3);
    }

    private void SpawnBeginBot()
    {
        foreach (var beginBot in beginBots)
        {
            var botObj = VehiclesPool.Instance.GetPooledObject();
            if (botObj != null)
            {
                var waypointFollower = botObj.GetComponent<WaypointFollower>();
                waypointFollower.TargetWaypoint = beginBot.targetWaypoint;
                waypointFollower.CurrentLaneIndex = beginBot.targetWaypoint.GetRandomLane();
                botObj.transform.position = beginBot.spawnPositions.position;
                botObj.transform.LookAt(beginBot.targetWaypoint.transform);
            }
        }
    }
}
