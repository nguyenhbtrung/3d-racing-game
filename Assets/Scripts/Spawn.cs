using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float spawnInterval = 2f; 
    public Vector3 arrowRotation; 

    void Start()
    {
        InvokeRepeating("SpawnArrow", 0f, spawnInterval);
    }

    void SpawnArrow()
    {
        Quaternion rotation = Quaternion.Euler(arrowRotation);
        Instantiate(arrowPrefab, transform.position, rotation);
    }
}
