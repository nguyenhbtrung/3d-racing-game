using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class VehiclesPool : MonoBehaviour
{
    public static VehiclesPool Instance;
    public List<GameObject> pooledObject;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private int maxObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        pooledObject = new List<GameObject>();
        int average = maxObject / prefabs.Length;
        GameObject temp;
        for (int i = 0; i < average; i++)
        {
            foreach (GameObject prefab in prefabs)
            {
                temp = Instantiate(prefab, transform);
                temp.SetActive(false);
                pooledObject.Add(temp);
            }
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < maxObject; i++)
        {
            if (!pooledObject[i].activeInHierarchy)
            {
                return pooledObject[i];
            }
        }
        return null;
    }

    public void ReturnToPool(GameObject pooledObject)
    {
        pooledObject.SetActive(false);
    }
}
