using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private List<Road> roadPrefabs;
    [SerializeField] private GameObject land;

    public GameObject GetRoadPrefabs(bool up, bool down, bool left, bool right)
    {
        Road road = roadPrefabs.Find(road 
            => road.UpNeighbourRoad == up 
            && road.DownNeighbourRoad == down
            && road.LeftNeighbourRoad == left
            && road.RightNeighbourRoad == right);
        if (road == null)
        {
            return land;
        }
        return road.gameObject;
    }
}
